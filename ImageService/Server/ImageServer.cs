using ImageService.Controller.Handlers;
using ImageService.Modal;
using System;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageModal m_controller;
        private ILoggingModal m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        /// <summary>
        /// Initiate a watching server
        /// </summary>
        /// <param name="logModal">The logging modal used by the server</param>
        /// <param name="outputFolder">Where to save our backup images</param>
        /// <param name="thumbnailSize">To size of thumbnail to create</param>
        public ImageServer(ILoggingModal logModal,string outputFolder,int thumbnailSize)
        {
            m_logging = logModal;
            m_controller = new ImageModal(outputFolder, thumbnailSize);
        }

        /// <summary>
        /// Start watching to directory for new files
        /// </summary>
        /// <param name="path">path to the directory</param>
        /// <returns>the success of the procedure</returns>
        public bool WatchDirectory(string path)
        {
            DirectoryHandler dh = new DirectoryHandler(m_controller, m_logging);
            try
            {
                dh.StartHandleDirectory(path);
                CommandRecieved += dh.OnCommandRecieved;
                dh.DirectoryClose += CloseServer;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// used to stop watching a directory.
        /// </summary>
        /// <param name="sender">The directory handler that we want to stop watching</param>
        /// <param name="args">unused</param>
        public void CloseServer(object sender, DirectoryCloseEventArgs args)
        {
            DirectoryHandler dh = (DirectoryHandler)sender;
            CommandRecieved -= dh.OnCommandRecieved;
            dh.DirectoryClose -= CloseServer;
        }

        /// <summary>
        /// sends command to watched directories
        /// </summary>
        /// <param name="arg"></param>
        public void SendCommand(CommandRecievedEventArgs arg)
        {
            CommandRecieved.Invoke(this, arg);
        }

    }
}
