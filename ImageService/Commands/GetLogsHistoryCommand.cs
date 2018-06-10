using ImageService.Modal;
using ImageService.Server;
using ImageService.Server.ClientHandlers;
using SharedInfo.Commands;
using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetLogsHistoryCommand : ICommand
    {
        private ClientHandler m_client;
        private LogArchive m_archive;
        private ILoggingModal ilogging;

        public GetLogsHistoryCommand(ClientHandler client, LogArchive archive, ILoggingModal logging)
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

                //send size of list
                m_client.Write(list.Count);

                //send every entry
                foreach (MessageRecievedEventArgs log in list)
                {
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
