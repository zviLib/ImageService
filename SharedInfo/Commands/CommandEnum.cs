using System;
using System.Collections.Generic;
using System.Text;

namespace SharedInfo.Commands
{
    public enum CommandEnum : int
    {
        NewFileCommand,
        CloseCommand,
        TrackLogs,
        GetAppConfig,
        NewLog,
        EmptyCommand,
        ServiceStatus
    }
}
