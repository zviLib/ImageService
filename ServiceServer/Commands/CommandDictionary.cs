using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServiceServer.Commands
{
    class CommandDictionary
    {
        private static Dictionary<ServerCommandEnum, IServerCommand> dictionary;

        public static void Execute(ServerCommandEnum type, ImageServer service, TcpClient client)
        {
            if (dictionary == null)
                initialize();

            if (dictionary.ContainsKey(type))
                dictionary[type].Execute(service, client);
        }

        private static void initialize()
        {
            dictionary.Add(ServerCommandEnum.LOG_TRACKER, new LogTrackerCommand());
        }
    }
}
