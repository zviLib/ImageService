using ImageService.Controller.Handlers;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using SharedInfo.Commands;
using SharedInfo.Messages;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageModal m_controller;
        public ILoggingModal M_logging { get; }
        private List<string> watchedDirs;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<DirectoryCloseEventArgs> CloseCommandRecieved;          // The event that notifies about an Handler being closed
        #endregion

        /// <summary>
        /// Initiate a watching server
        /// </summary>
        /// <param name="logModal">The logging modal used by the server</param>
        /// <param name="outputFolder">Where to save our backup images</param>
        /// <param name="thumbnailSize">To size of thumbnail to create</param>
        public ImageServer(ILoggingModal logModal,string outputFolder,int thumbnailSize)
        {
            M_logging = logModal;
            m_controller = new ImageModal(outputFolder, thumbnailSize);
            watchedDirs = new List<string>();
        }

        /// <summary>
        /// Start watching to directory for new files
        /// </summary>
        /// <param name="path">path to the directory</param>
        /// <returns>the success of the procedure</returns>
        public bool WatchDirectory(string path)
        { 
            DirectoryHandler dh = new DirectoryHandler(m_controller, M_logging);
            try
            {
                dh.StartHandleDirectory(path);
                CommandRecieved += dh.OnCommandRecieved;
                dh.DirectoryClose += CloseServer;
                watchedDirs.Add(path);
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
            watchedDirs.Remove(args.Path);
            //notify about closure
            CloseCommandRecieved.Invoke(dh, args);
        }

        /// <summary>
        /// sends command to watched directories
        /// </summary>
        /// <param name="arg"></param>
        public void SendCommand(CommandRecievedEventArgs arg)
        {
            try
            {
                CommandRecieved.Invoke(this, arg);
            } catch (Exception e)
            {
                M_logging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.FAIL,
                    Message = e.Message
                });
            }
        }

        /// <summary>
        /// reads app config and puts in a dictionary
        /// </summary>
        /// <returns>dictionary with the app configs</returns>
        public Dictionary<int, string> GetAppConfig()
        {
            Dictionary<int, string> configs = new Dictionary<int, string>();
            // add output directory
            configs.Add((int)AppConfigValuesEnum.OutputDirectory, ConfigurationManager.AppSettings["OutputDir"]);
            configs.Add((int)AppConfigValuesEnum.SourceName, ConfigurationManager.AppSettings["SourceName"]);
            configs.Add((int)AppConfigValuesEnum.LogName, ConfigurationManager.AppSettings["LogName"]);
            configs.Add((int)AppConfigValuesEnum.ThumbnailSize, ConfigurationManager.AppSettings["ThumbnailSize"]);
 
            // add watched directories
            int i = 4;
            foreach (string s in watchedDirs)
            {
                configs.Add(i, s);
                i++;
            }
            return configs;
        }
        
        
    }
}   
