using ImageService.Commands;
using System;

namespace ImageService.Controller.Handlers
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public CommandEnum Type { get; set; }
        public string Path { get; set; }
    }
}
