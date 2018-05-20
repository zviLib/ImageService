﻿using SharedInfo.Commands;
using SharedInfo.Messages;
using System;
using System.IO;
using System.Net.Sockets;

namespace ImageService.Server.ClientHandlers
{
    class ClientHandler
    {
        private static NetworkStream stream;
        private static BinaryWriter writer;
        private static BinaryReader reader;

        public ClientHandler(TcpClient client)
        {
            stream = client.GetStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }

        /// <summary>
        /// notifies gui that a directory has been closed
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="message">path of the closed directory</param>
        public void CloseCommand(object sender, DirectoryCloseEventArgs message)
        {
                //write the path of the closed handler
                writer.Write((int)CommandEnum.CloseCommand);
                writer.Write(message.Path);

        }
        /// <summary>
        /// notifies gui that a new log has been writen
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="message">the log content</param>
        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
                writer.Write((int)CommandEnum.NewLog);
                writer.Write((int)message.Status);
                writer.Write(message.Message);

        }
        
        public CommandEnum ReadCommand()
        {
            return (CommandEnum)reader.ReadInt32();
        }

        public void Write(int i)
        {
            writer.Write(i);
        }

        public void Write(string s)
        {
            writer.Write(s);
        }

        public string ReadString()
        {
            return reader.ReadString();
        }

        public void Close(object sender, DirectoryCloseEventArgs args)
        {
            try
            {
                writer.Close();
                reader.Close();
                stream.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}