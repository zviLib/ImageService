using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer.Streamer
{
    class LogStreamer
    {
        private TcpClient m_client;

        public LogStreamer(TcpClient client)
        {
            m_client = client;
        }

        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            using (NetworkStream stream = m_client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                int status = (int)message.Status;
                writer.Write(status);
                writer.Write(message.Message);
            }
        }
    }
}
