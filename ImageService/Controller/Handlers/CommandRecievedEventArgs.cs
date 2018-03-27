using ImageService.Modal;
using System;

namespace ImageService.Controller.Handlers
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public int Type { get; set; }
        public string[] Args { get; set; }
    }
}