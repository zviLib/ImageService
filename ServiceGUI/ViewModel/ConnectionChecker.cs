using ServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGUI.ViewModel
{
    class ConnectionChecker
    {
        public static bool IsConnected()
        {
            return Client.TryConnection();
        }
    }
}
