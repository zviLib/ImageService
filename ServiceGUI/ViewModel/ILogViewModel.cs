using SharedInfo.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGUI.ViewModel
{
    interface ILogViewModel
    {
        void OnMsg(object sender, MessageRecievedEventArgs message);
    }
}
