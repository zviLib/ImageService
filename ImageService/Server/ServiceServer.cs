using ImageService.Commands;
using ImageService.Server.ClientHandlers;
using SharedInfo.Messages;
using SharedInfo.Commands;
using SharedInfo;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ImageService.Modal;

namespace ImageService.Server
{
    class ServiceServer
    {
        public bool Alive { set; get; }
        private ImageServer m_server;
        private LogArchive m_archive;
        private TcpListener listener;
        private ILoggingModal ilogging;
        private string newFilePath;
        public static event EventHandler<DirectoryCloseEventArgs> ServerClosed; //notifes client hanlder that the server is closing

        public ServiceServer(ImageServer server, LogArchive archive, ILoggingModal logging, string newFilePath)
        {
            Alive = true;
            m_server = server;
            m_archive = archive;
            ilogging = logging;
            this.newFilePath = newFilePath;
        }

        /// <summary>
        /// listen to connections and handle on different thread
        /// </summary>
        public void Listen()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerInfo.IP), ServerInfo.PORT);
            listener = new TcpListener(ep);
            listener.Start();
            //listen for connection
            while (Alive)
            {
                // accept connection
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    
                    // handle in different thread
                    Task t = new Task(() => HandleClient(client));
                    t.Start();
                }
                catch (Exception e)
                {
                    ilogging.Log(new MessageRecievedEventArgs
                    {
                        Status = MessageTypeEnum.FAIL,
                        Message = e.Message
                    });
                }
            }

            listener.Stop();
        }

        public void Close()
        {
            Alive = false;
        }

        private void HandleClient(TcpClient client)
        {
            CommandEnum type;
            ClientHandler handler = new ClientHandler(client);
            // register handler on server closure
            ServerClosed += handler.Close;
            // register handler on directory colsure
            m_server.CloseCommandRecieved += handler.CloseCommand;
            while (Alive)
            {
                //read command enum
                type = handler.ReadCommand();

                bool res = false;
                if (type == CommandEnum.TrackLogs)
                {
                    //send old logs
                    new GetLogsCommand(handler, m_archive, ilogging).Execute(new String[] { "" }, out res);
                    // client listens for new logs
                    m_server.M_logging.MessageRecieved += handler.OnMsg;
                }

                else if (type == CommandEnum.GetAppConfig)
                {
                    //send app config
                    new GetAppConfigCommand(handler, m_server, ilogging).Execute(new String[] { "" }, out res);
                }

                else if (type == CommandEnum.CloseCommand)
                {
                    string s = handler.ReadString();
                    m_server.SendCommand(new CommandRecievedEventArgs
                    {
                        Type = CommandEnum.CloseCommand,
                        Args = new string[] { s }
                    });
                }
                else if (type == CommandEnum.ServiceStatus)
                {
                    new GetServiceStatusCommand(handler).Execute(null, out res);
                }
                else if (type == CommandEnum.GetLogHistory)
                {
                    new GetLogsHistoryCommand(handler, m_archive, ilogging).Execute(new String[] { "" }, out res);
                } else if (type == CommandEnum.NewFileCommand)
                {
                    new NewFileBroadcastCommand(handler,newFilePath).Execute(null, out res);
                    break;
                }
            }

            ServerClosed -= handler.Close;
            m_server.CloseCommandRecieved -= handler.CloseCommand;
            client.Close();
        }
    }
}
