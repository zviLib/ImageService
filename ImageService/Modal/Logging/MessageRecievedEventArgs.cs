using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
