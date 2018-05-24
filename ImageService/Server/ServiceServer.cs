﻿using ImageService.Commands;
using ImageService.Server.ClientHandlers;
using SharedInfo.Messages;
using SharedInfo.Commands;
using SharedInfo;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
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
        public static event EventHandler<DirectoryCloseEventArgs> ServerClosed; //notifes client hanlder that the server is closing

        public ServiceServer(ImageServer server, LogArchive archive, ILoggingModal logging)
        {
            Alive = true;
            m_server = server;
            m_archive = archive;
            ilogging = logging;
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
            if (listener != null)
                listener.Stop();

<<<<<<< HEAD
            // if server has listeners - norify them about closure
            if (ServerClosed != null)
                ServerClosed.Invoke(null, null);
=======
            ServerClosed.Invoke(null,null);
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
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
<<<<<<< HEAD

=======
                
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
                bool res = false;
                if (type == CommandEnum.TrackLogs)
                {
                    //send old logs
<<<<<<< HEAD
                    new GetLogsCommand(handler, m_archive, ilogging).Execute(new String[] { "" }, out res);
=======
                    new GetLogsCommand(handler, m_archive,ilogging).Execute(new String[] { "" }, out res);
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
                    // client listens for new logs
                    m_server.M_logging.MessageRecieved += handler.OnMsg;
                }

                if (type == CommandEnum.GetAppConfig)
                {
                    //send app config
                    new GetAppConfigCommand(handler, m_server, ilogging).Execute(new String[] { "" }, out res);
                    //listen for future closures of handlers
                    m_server.CloseCommandRecieved += handler.CloseCommand;
                }

<<<<<<< HEAD
                if (type == CommandEnum.CloseCommand)
=======
                if(type == CommandEnum.CloseCommand)
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
                {
                    string s = handler.ReadString();
                    m_server.SendCommand(new CommandRecievedEventArgs
                    {
                        Type = CommandEnum.CloseCommand,
                        Args = new string[] { s }
                    });
                }
            }

            client.Close();
        }
    }
}