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
            bool result;
            string newPath = m_controller.ExecuteCommand(e.Type, e.Args, out result);
            if (result)
                m_logging.Log(newPath, MessageTypeEnum.INFO);
            else
                m_logging.Log("Command Failed", MessageTypeEnum.FAIL);
        }

        public bool StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            try { 
            m_dirWatcher = new FileSystemWatcher(dirPath);
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        // Implement Here!

    }
}
