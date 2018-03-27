using ImageService.Modal;
using System;

namespace ImageService.Controller.Handlers
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        
        public MessageTypeEnum Status { get; set; }
        public string Path { get; set; }
    }
}
