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
        private IImageModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            NewFileCommand newFile = new NewFileCommand(modal);
            CreateFolder create = new CreateFolder(modal);
            commands = new Dictionary<int, ICommand>
            {
                { 0, newFile },
                { 1, create}
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}
