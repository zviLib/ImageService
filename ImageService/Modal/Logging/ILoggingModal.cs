using System;
using SharedInfo.Messages;

namespace ImageService.Modal
{
    public interface ILoggingModal
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;  
        void Log(MessageRecievedEventArgs msg);           // Logging the Message
    }
}
