using SharedInfo.Commands;

namespace ServiceGUI.ViewModel
{
    interface ISettingsViewModel
    {
        void DirectoryClosed(object sender, DirectoryCloseEventArgs args);
        void UpdateAppConfig();
        void CloseDirectory(string path);
    }
}
