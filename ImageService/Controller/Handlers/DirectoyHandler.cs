using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingModal m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoryHandler(IImageModal service, ILoggingModal logger)
        {
            m_controller = new ImageController(service);
            m_logging = logger;
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            //checks if picture is in handled directory
            if (!e.Args.Contains(m_path))
                return;

            //handles
            // 0 = close handler
            if (e.Type == 0)
            {
                //remove from server
                DirectoryClose.Invoke(this, new DirectoryCloseEventArgs
                {
                    Path = m_path
                });
                //close watcher
                m_dirWatcher.Dispose();
                //log 
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Message = "Stopped watching:" + m_path
                });
            }
        }

    

        public bool StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            try { 
            m_dirWatcher = new FileSystemWatcher(dirPath);
            m_dirWatcher.Created += FileCreated;
                m_logging.Log(new MessageRecievedEventArgs
                {
                    Message = "Started watching directory:" + dirPath,
                });
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            bool b;
            string[] arg = { args.Name };
            string res = m_controller.ExecuteCommand(0, arg, out b);
            //log result
            m_logging.Log(new MessageRecievedEventArgs
            {
                Message = res
            });

        public void CloseHandler()
        {
            //closes watcher
            m_dirWatcher.Dispose();

            //invoke close directory event
            DirectoryClose.Invoke(this, new DirectoryCloseEventArgs
            {
                Path = m_path
            });
        }

    }
}
