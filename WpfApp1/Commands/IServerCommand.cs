using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGUI.Commands
{
    interface IServerCommand
    {
        void Execute(ImageServer service, TcpClient client);
    }
}
