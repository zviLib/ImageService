using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageModal modal)
        {
            NewFileCommand newFile = new NewFileCommand(modal);
            commands = new Dictionary<int, ICommand>
            {
                { 0, newFile }
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
          
            return commands[commandID].Execute(args, out resultSuccesful);
        }
        
    }
}
