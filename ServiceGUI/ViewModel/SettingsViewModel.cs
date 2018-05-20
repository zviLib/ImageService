using ServiceGUI.Model;
using SharedInfo.Commands;
using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ServiceGUI.ViewModel
{
    class SettingsViewModel : ISettingsViewModel
    {
        public string OutputDir { private set; get; }
        public string SourceName { private set; get; }
        public string LogName { private set; get; }
        public string ThumbSize { private set; get; }
        public ObservableCollection<String> Handlers { private set; get; }

        private ISettingsModel model;

        public SettingsViewModel()
        {
            OutputDir = SourceName = LogName = ThumbSize ="Unknown";

            Handlers = new ObservableCollection<string>();
            Object thisLock = new object();
            BindingOperations.EnableCollectionSynchronization(Handlers, thisLock);

            model = new SettingsModel();
            model.ListenForChanges(this);
        }

        public void CloseDirectory(string path)
        {
            model.CloseDirectory(path);
        }

        public void DirectoryClosed(object sender, DirectoryCloseEventArgs args)
        {
            Handlers.Remove(args.Path);
        }

        public void UpdateAppConfig()
        {
            
            Dictionary<int, string> values = model.getAppConfig();

            foreach (KeyValuePair<int, string> val in values)
            {
                switch (val.Key)
                {
                    case (int)AppConfigValuesEnum.OutputDirectory:
                        OutputDir = val.Value;
                        break;
                    case (int)AppConfigValuesEnum.SourceName:
                        SourceName = val.Value;
                        break;
                    case (int)AppConfigValuesEnum.LogName:
                        LogName = val.Value;
                        break;
                    case (int)AppConfigValuesEnum.ThumbnailSize:
                        ThumbSize = val.Value;
                        break;
                    default:
                        Handlers.Add(val.Value);
                        break;
                }
            }
        }
    }
}
