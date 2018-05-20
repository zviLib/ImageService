using ImageService.Server;
using System;
using System.Collections.Generic;
using SharedInfo.Messages;
using ImageService.Server.ClientHandlers;
using ImageService.Modal;
using SharedInfo.Commands;

namespace ImageService.Commands
{
    class GetLogsCommand : ICommand
    {
        private ClientHandler m_client;
        private LogArchive m_archive;
        private ILoggingModal ilogging;

        public GetLogsCommand(ClientHandler client, LogArchive archive, ILoggingModal logging)
        {
            m_archive = archive;
            m_client = client;
            ilogging = logging;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<MessageRecievedEventArgs> list = m_archive.Logs;

                
                //send every entry
                foreach (MessageRecievedEventArgs log in list)
                {
                    m_client.Write((int)CommandEnum.NewLog);
                    m_client.Write((int)log.Status);
                    m_client.Write(log.Message);
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
