using ServiceGUI.Model;
using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServiceGUI.ViewModel
{

    public class LogViewModel : ILogViewModel
    {
        public ObservableCollection<MessageRecievedEventArgs> LogList { private set; get; }
        private MessageRecievedEventArgs last;

        public LogViewModel()
        {
            last = null;

            //initialize log list
            LogList = new ObservableCollection<MessageRecievedEventArgs>();
            Object thisLock = new object();
            BindingOperations.EnableCollectionSynchronization(LogList, thisLock);

            //start listening to logs
            LogModel logModel = new LogModel();
            logModel.StartListening(this);

        }

        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            if (last == null || last.Message != message.Message || message.Status != last.Status)
            {
                LogList.Add(message);
                last = message;
            }
        }
        
    }
}

