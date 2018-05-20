using ServiceServer.Streamer;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace ServiceServer.Commands
{
    class LogTrackerCommand : IServerCommand
    {

        public void Execute(ImageServer service, TcpClient client)
        {
            List<MessageRecievedEventArgs> logs = service.GetLogs();

            //sent old logs
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                foreach (MessageRecievedEventArgs m in logs)
                {
                    writer.Write((int)m.Status);
                    writer.Write(m.Message);
                }
            }

            //creates streamer for new messages
            LogStreamer streamer = new LogStreamer(client);
            ILoggingModal logging = service.GetLoggingModal();
            logging.MessageRecieved += streamer.OnMsg;
        }
    }
}
