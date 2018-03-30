using ImageService.Commands;
using ImageService.Modal;
using System;

namespace ImageService.Controller.Handlers
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public CommandEnum Type { get; set; }
        public string[] Args { get; set; }
    }
}