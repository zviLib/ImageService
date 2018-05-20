using System;

namespace SharedInfo.Commands
{
    public class CommandRecievedEventArgs : EventArgs
    {
        public CommandEnum Type { get; set; }
        public string[] Args { get; set; }
    }
}