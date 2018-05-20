using ServiceServer.Commands;
using SharedInfo;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        #region Members
        private bool alive;
        #endregion Members

        public CommunicationServer()
        {
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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerInfo.IP), ServerInfo.GUI_PORT);
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
            //connect to service
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerInfo.IP), ServerInfo.SERVICE_PORT);
            TcpClient client2 = new TcpClient();
            client2.Connect(ep);

            CommandEnum type;
            string s ="";
            using (NetworkStream stream1 = client.GetStream())
            using (NetworkStream stream2 = client2.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream2))
            using (BinaryReader reader = new BinaryReader(stream1))
            {

                //read commend enum from gui
                type = (CommandEnum)reader.ReadInt32();
                //read args if needed
                if (type == CommandEnum.CloseCommand)
                    s = reader.ReadString();

                //send comment to service
                writer.Write((int)type);
                if (type == CommandEnum.CloseCommand)
                    writer.Write(s);
            }

        }
    }
}
