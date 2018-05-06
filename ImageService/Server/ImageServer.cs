using ImageService.Controller.Handlers;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageModal m_controller;
        private ILoggingModal m_logging;
        private LogArchive m_archive;
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

            //create logging archive
            m_archive = new LogArchive();
            m_logging.MessageRecieved += m_archive.OnMsg;
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

        public List<ConfigValue> GetAppConfig()
        {
            List<ConfigValue> configs = new List<ConfigValue>();
            // add output directory
            configs.Add(new ConfigValue
            {
                Name = "Output Directory",
                Value = ConfigurationManager.AppSettings["OutputDir"]
            });
            // add source name
            configs.Add(new ConfigValue
            {
                Name = "Source Name",
                Value = ConfigurationManager.AppSettings["SourceName"]
            });
            // add log name
            configs.Add(new ConfigValue
            {
                Name = "Log Name",
                Value = ConfigurationManager.AppSettings["LogName"]
            });
            // add thumbnail size
            configs.Add(new ConfigValue
            {
                Name = "Thumbnail Size",
                Value = ConfigurationManager.AppSettings["ThumbnailSize"]
            });

            // add watched directories
            string[] paths = ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string s in paths)
            {
                configs.Add(new ConfigValue
                {
                    Name = "Handler",
                    Value = ConfigurationManager.AppSettings["s"]
                });
            }
            return configs;
        }

        public List<MessageRecievedEventArgs> GetLogs()
        {
            return m_archive.Logs;
        }

        public ILoggingModal GetLoggingModal()
        {
            return m_logging;
        }
    }
}   
