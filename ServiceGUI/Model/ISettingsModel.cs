using ServiceGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGUI.Model
{
    interface ISettingsModel
    {
        Dictionary<int, string> getAppConfig();
        void ListenForChanges(ISettingsViewModel svm);
        void CloseDirectory(string path);
    }
}
