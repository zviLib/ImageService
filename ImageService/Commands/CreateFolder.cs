using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CreateFolder : ICommand
    {
        private IImageModal m_modal;

        public CreateFolder(IImageModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            return m_modal.CreateFolder(args[0], args[1], out result);
        }
    }
}
