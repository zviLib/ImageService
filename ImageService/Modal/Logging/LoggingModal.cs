using System;
using SharedInfo.Messages;

namespace ImageService.Modal
{
    public class LoggingModal : ILoggingModal
    {


        public LoggingModal()
        {

        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Log(MessageRecievedEventArgs msg)
        {
            MessageRecieved.Invoke(this, msg);
        }
    }
}
