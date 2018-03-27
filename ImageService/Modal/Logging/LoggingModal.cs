using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class LoggingModal : ILoggingModal
    {


        public LoggingModal()
        {

        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Log(string message, MessageTypeEnum type)
        {
            //create argument
            MessageRecievedEventArgs m = new MessageRecievedEventArgs
            {
                Status = type,
                Message = message
            };
            //invoke event
            MessageRecieved.Invoke(this, m);
        }
    }
}
