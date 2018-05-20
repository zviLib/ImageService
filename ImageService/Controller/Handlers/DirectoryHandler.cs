using ImageService.Modal;
using System;
using System.IO;
using System.Threading.Tasks;
using SharedInfo.Commands;
using SharedInfo.Messages;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingModal m_logging;                    // The logging unit
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private string[] trackedExt;                        // The extantions that we are currently tracking
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoryHandler(IImageModal imageModal, ILoggingModal logger)
        {
            m_controller = new ImageController(imageModal);
            m_logging = logger;
            this.trackedExt = new string[] { "jpg", "png", "gif", "bmp" };
        }

        /// <summary>
        /// handles commands sent to the handler - currently only Close command
        /// </summary>
        /// <param name="sender">where this command came from - unused</param>
        /// <param name="e">information about the command</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {

            //close command
            if (e.Type == CommandEnum.CloseCommand && (e.Args[0]==m_path || e.Args[0]=="All"))
            {
                //remove from server
                DirectoryClose.Invoke(this, new DirectoryCloseEventArgs
                {
                    Path = m_path
                });
                //close watcher
                m_dirWatcher.Dispose();
                m_dirWatcher.EnableRaisingEvents = false;

                //log 
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.WARNING,
                    Message = "Stopped watching:" + m_path
                });
            }
        }


        /// <summary>
        /// Start watching for new files in the directory
        /// </summary>
        /// <param name="dirPath">the directory to watch</param>
        /// <returns>whether the watcher launched successfully</returns>
        public bool StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            try
            {
                m_dirWatcher = new FileSystemWatcher(dirPath);
                m_dirWatcher.Created += new FileSystemEventHandler(StartThread);
                m_dirWatcher.Filter = "*.*";
                m_dirWatcher.EnableRaisingEvents = true;
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.INFO,
                    Message = "Started watching directory:" + dirPath,
                });
                return true;
            }
            catch (Exception e)
            {
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.FAIL,
                    Message = e.Message,
                });
                return false;
            }
        }
        /// <summary>
        /// checks if the new file's extention is being tracked.
        /// moves file handling to a new thread.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="args">information about the new file</param>
        private void StartThread(object sender, FileSystemEventArgs args)
        {
            string filePath = args.FullPath;

            //get newFile extention
            if (filePath.LastIndexOf('.') == -1)
                return;

            string extension = filePath.Substring(filePath.LastIndexOf('.') + 1);

            //check if extenstion is tracked
            bool tracked = false;
            foreach (string ext in trackedExt)
                if (extension.Equals(ext))
                {
                    tracked = true;
                    break;
                }
            //execute only if file type is tracked
            if (tracked)
            {
                Task t = new Task(() =>
                {
                    FileCreated(sender, args);
                });

                t.Start();
            }
        }
        /// <summary>
        /// Executing newFile command from the Modal.
        /// If procedure fails - try again twice after suspension.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="args">the new file to handle</param>
        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            //notify about start of the handle
            m_logging.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "New file detected: " + args.FullPath
            });

            //wait that file transfer to the watched folder will be completed so file will be available to use
            System.Threading.Thread.Sleep(10000);

            bool b = false;
            int count = 0;
            string[] arg = { args.FullPath };

            //while procedure is failing and we tried less than 3 times
            while (!b && count < 3)
            {
                //initiate command
                string res = m_controller.ExecuteCommand(CommandEnum.NewFileCommand, arg, out b);
                //log result
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.INFO,
                    Message = res
                });
                //if action failed - notify about reschedule
                count++;
                if (!b && count != 3)
                {

                    m_logging.Log(new MessageRecievedEventArgs
                    {
                        Status = MessageTypeEnum.FAIL,
                        Message = String.Format("{0} : Attempt no. {1} schduled in {2} seconds", args.Name, count + 1, 30 * count)
                    });
                    System.Threading.Thread.Sleep(count * 30000);
                }
            }
        }
    }
}
