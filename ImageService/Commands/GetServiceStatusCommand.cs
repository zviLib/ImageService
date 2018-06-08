using ImageService.Server.ClientHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetServiceStatusCommand : ICommand
    {
        private ClientHandler handler;

        public GetServiceStatusCommand(ClientHandler client)
        {
            this.handler = client;
        }

        public string Execute(string[] args, out bool result)
        {
            bool res = ImageService.started;
            if (res)
                handler.Write(1);
            else
                handler.Write(0);

            result = true;

            return res.ToString();
        }
    }
}
