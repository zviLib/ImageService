using ImageService.Modal;
using ImageService.Server;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServiceGUI.Server
{
    class GUIServer
    {
        #region Members
        private ImageServer m_service;
        private readonly string IP = "127.0.0.1";
        private readonly int PORT = 8080;
        private bool alive;
        #endregion Members

        public GUIServer(ImageServer service)
        {
            m_service = service;
            alive = true;

            // create server and listen for connections on different thread
            Task t = new Task(() =>
            {
                CreateAndListen();
            });
            t.Start();
        }

        private void CreateAndListen()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            TcpListener listener = new TcpListener(ep);
            listener.Start();
            //listen for connection
            while (alive)
            {
                // accept connection
                TcpClient client = listener.AcceptTcpClient();
                // handle in different thread
                Task t = new Task(() =>
                {
                    HandleClient(client);
                });
                t.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                //read commend enum
                int type = reader.ReadInt32();
                //execute command
                
            }
        }
    }
}
