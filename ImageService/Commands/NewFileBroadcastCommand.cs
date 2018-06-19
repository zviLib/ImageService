using ImageService.Modal;
using ImageService.Server.ClientHandlers;
using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class NewFileBroadcastCommand
    {
        private ClientHandler handler;
        private string path;

        public NewFileBroadcastCommand(ClientHandler client,string path)
        {
            this.handler = client;
            this.path = path;
        }

        public string Execute(string[] args, out bool result)
        {
            // read image name
            int size = handler.ReadInt();

            byte[] nameByte = handler.ReadByteArr(size);
            string name = System.Text.Encoding.UTF8.GetString(nameByte);
 
            // read image size
            size = handler.ReadInt();

            // read image
            byte[] picByte = handler.ReadByteArr(size);

            //convert byte array to iamge
            Image im = null;
            using (MemoryStream mStream = new MemoryStream(picByte))
            {
                im = Image.FromStream(mStream);
            }

            im.Save(path+"\\"+name);

            result = true;
            return name;
        }
    }
}
