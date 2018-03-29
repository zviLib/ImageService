using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
   public interface ILoggingModal
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(MessageRecievedEventArgs msg);           // Logging the Message
    }
}
