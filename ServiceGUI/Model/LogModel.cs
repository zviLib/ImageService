using ServiceGUI.ViewModel;
using System.Threading.Tasks;

namespace ServiceGUI.Model
{
    class LogModel : ILogModel
    {
        
        public void StartListening(ILogViewModel viewModel)
        {
            Client.LogRecieved += viewModel.OnMsg;
            //listen for new logs
            Task t = new Task(() => Client.ListenForCommands());
            t.Start();
        }
    }
}
