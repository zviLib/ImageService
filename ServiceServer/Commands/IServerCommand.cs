using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServiceServer.Commands
{
    interface IServerCommand
    {
        void Execute(ImageServer service, TcpClient client);
    }
}
