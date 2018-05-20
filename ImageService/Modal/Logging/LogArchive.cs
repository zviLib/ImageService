using ImageService.Modal;
using System.Collections.Generic;
using SharedInfo.Messages;

namespace ImageService.Server
{

    class LogArchive
    {
        #region members
        public List<MessageRecievedEventArgs> Logs { get; private set; }
        
        #endregion members

        public LogArchive()
        {
            Logs = new List<MessageRecievedEventArgs>();
        }

        /// <summary>
        /// Add every log entry to the archive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            Logs.Add(message);
        }

    }
}
