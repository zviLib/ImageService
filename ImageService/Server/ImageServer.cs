using ImageService.Controller.Handlers;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ImageServer(ILoggingModal logModal,string outputFolder,int thumbnailSize)
        {
            m_logging = logModal;
            m_controller = new ImageModal(outputFolder, thumbnailSize);
        }

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
            catch (Exception e)
            {
                return false;
            }
        }

        public void CloseServer(object sender, DirectoryCloseEventArgs args)
        {
            DirectoryHandler dh = (DirectoryHandler)sender;
            CommandRecieved -= dh.OnCommandRecieved;
            dh.DirectoryClose -= CloseServer;
        }

        public void SendCommand(CommandRecievedEventArgs arg)
        {
            CommandRecieved.Invoke(this, arg);
        }

    }
}
