using ImageService.Modal;
using ImageService.Server;
using ImageService.Server.ClientHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SharedInfo.Messages;

namespace ImageService.Commands
{
    class GetAppConfigCommand : ICommand
    {
        private ClientHandler m_client;
        private ImageServer m_server;
        private ILoggingModal ilogging;

        public GetAppConfigCommand(ClientHandler client, ImageServer server, ILoggingModal logging)
        {
            m_client = client;
            m_server = server;
            ilogging = logging;
        }

        public string Execute(string[] args, out bool result)
        {
            Dictionary<int, string> list = m_server.GetAppConfig();

            try
            {
                //send number of entries
                m_client.Write(list.Count);
                //send every entry
                foreach (KeyValuePair<int, string> val in list)
                {
                    m_client.Write(val.Key);
                    m_client.Write(val.Value);

                }

                result = true;
            }
            catch (Exception e)
            {
                ilogging.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.FAIL,
                    Message = e.Message
                });
                result = false;
            }

            return "";

        }
    }
}
