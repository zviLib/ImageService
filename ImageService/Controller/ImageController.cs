using ImageService.Commands;
using ImageService.Modal;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private Dictionary<CommandEnum, ICommand> commands; // keeps a dictonary of available commands

        public ImageController(IImageModal modal)
        {
            NewFileCommand newFile = new NewFileCommand(modal);
            commands = new Dictionary<CommandEnum, ICommand>
            {
                { CommandEnum.NewFileCommand, newFile }
            };
        }

        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool resultSuccesful)
        {
          
            return commands[commandID].Execute(args, out resultSuccesful);
        }
        
    }
}
