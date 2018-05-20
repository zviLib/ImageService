using System;

namespace SharedInfo.Messages
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
