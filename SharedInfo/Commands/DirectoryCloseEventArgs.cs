using System;

namespace SharedInfo.Commands
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public CommandEnum Type { get; set; }
        public string Path { get; set; }
    }
}
