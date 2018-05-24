using SharedInfo;
using SharedInfo.Commands;
using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceGUI.Model
{
    class Client
    {
        #region members
<<<<<<< HEAD
        private static bool connected = false; // used to check if a connection has been established
=======
        private static TcpClient m_client = null; // used to check if a connection has been established
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
        private static NetworkStream stream;    // used for communication with the server
        private static BinaryWriter writer;     // used for writing to server
        private static BinaryReader reader;     // used for reading from server
        public static event EventHandler<MessageRecievedEventArgs> LogRecieved; // notifies listeners on new logs
        public static event EventHandler<DirectoryCloseEventArgs> DirectoryClosed;  // notifies listeners on directory closure
        #endregion members

        /// <summary>
        /// tries to connect to the server
        /// </summary>
        /// <returns>whether a connection has been established</returns>
<<<<<<< HEAD
        private static bool Connect()
=======
        public static bool Connect()
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
        {

            //connect to server
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerInfo.IP), ServerInfo.PORT);
<<<<<<< HEAD
            TcpClient m_client = new TcpClient();
=======
            m_client = new TcpClient();
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
            try
            {
                m_client.Connect(ep);
                Console.WriteLine("Connected");
                stream = m_client.GetStream();
                writer = new BinaryWriter(stream);
                reader = new BinaryReader(stream);

            }
            catch (Exception)
            {
                Console.WriteLine("can't connect");
                return false;
            }

            return true;
        }
<<<<<<< HEAD
        /// <summary>
        /// used to check if client can connect to server
        /// </summary>
        /// <returns>if the connection is successful</returns>
        public static bool TryConnection()
        {
            if (connected)
                return true;

            //if not connected - attempt to connect
            connected = Connect();

            return connected;
        }
=======
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd

        /// <summary>
        /// get the server's app config
        /// </summary>
        /// <returns>Dictionary with the configurations</returns>
        public static Dictionary<int, string> getAppConfig()
        {
            Dictionary<int, string> values = new Dictionary<int, string>();

<<<<<<< HEAD
            //check connection
            if (!TryConnection()) 
                return values;
            
=======
            if (m_client == null)
                if (!Connect())
                    return values;
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd

            //send appConfig command
            writer.Write((int)CommandEnum.GetAppConfig);

            //read configs to dictionary
            //get size of dictionary
            int size = reader.ReadInt32();
            int key;
            string value;
            for (int i = 0; i < size; i++)
            {
                //read entries
                key = reader.ReadInt32();
                value = reader.ReadString();
                //add to dictionary
                values.Add(key, value);
            }

            return values;
        }

<<<<<<< HEAD
        /// <summary>
        /// listen for new commands received from the server
        /// </summary>
        public static void ListenForCommands()
        {
            if (!TryConnection())
                return;

=======

        public static void ListenForCommands()
        {
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd
            //request for log history
            writer.Write((int)CommandEnum.TrackLogs);

            //will exit when stream is closed
            try
            {
                CommandEnum command;
                MessageTypeEnum type;
                string s;
                while (true)
                {
                    //read new command
                    command = (CommandEnum)reader.ReadInt32();

                    if (command == CommandEnum.NewLog)
                    {
                        type = (MessageTypeEnum)reader.ReadInt32();
                        s = reader.ReadString();
                        LogRecieved.Invoke(null, new MessageRecievedEventArgs
                        {
                            Status = type,
                            Message = s
                        });
                    }

                    if (command == CommandEnum.CloseCommand)
                    {
                        s = reader.ReadString();
                        DirectoryClosed.Invoke(null, new DirectoryCloseEventArgs
                        {
                            Path = s
                        });
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        public static void SendCommand(CommandRecievedEventArgs args)
        {
<<<<<<< HEAD
            if (!TryConnection())
                return;
=======
            if (m_client == null)
                if (!Connect())
                    return;
>>>>>>> 227561cf3af844d48e38108df6f7a63c80bd89fd

            writer.Write((int)args.Type);
            if (args.Type == CommandEnum.CloseCommand)
                writer.Write(args.Args[0]);
        }

        public static void Close()
        {
            writer.Close();
            reader.Close();
            stream.Close();
        }
    }
}
