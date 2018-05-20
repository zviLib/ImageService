using ServiceGUI.ViewModel;
using System.Collections.Generic;
using SharedInfo.Commands;
namespace ServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public Dictionary<int,string> getAppConfig()
        {
            return Client.getAppConfig();
        }

        public void ListenForChanges(ISettingsViewModel svm)
        {
            Client.DirectoryClosed += svm.DirectoryClosed;
        }

        public void CloseDirectory(string path)
        {
            Client.SendCommand(new CommandRecievedEventArgs
            {
                Type = CommandEnum.CloseCommand,
                Args = new string[] { path }
            });
        }

    }
}
