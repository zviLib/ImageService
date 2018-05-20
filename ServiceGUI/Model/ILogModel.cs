using ServiceGUI.ViewModel;

namespace ServiceGUI.Model
{
    interface ILogModel
    {
        void StartListening(ILogViewModel viewModel);
    }
}
