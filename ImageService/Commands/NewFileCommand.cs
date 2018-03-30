using ImageService.Modal;

namespace ImageService.Commands
{
    class NewFileCommand : ICommand
    {
        private IImageModal m_modal;

        public NewFileCommand(IImageModal modal)
        {
            m_modal = modal;    
        }
        //execute addFile method on registered modal
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result);
        }
    }
}
