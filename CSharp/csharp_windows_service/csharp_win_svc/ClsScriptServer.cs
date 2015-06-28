using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace csharp_win_svc
{
    /// <summary>
    /// A C# windows service template.
    /// 
    /// Ref: http://blogs.msdn.com/b/bclteam/archive/2005/03/15/396428.aspx
    /// 
    /// @By: X. Chen
    /// @Created: 
    /// @Last modified: 
    /// </summary>
    class ClsScriptServer
    {
        private int _port;
        private Thread listenerThread;

        public ClsScriptServer() {
            getConfig();
        }

        private void getConfig() {
            _port = 0;

            if (ConfigurationSettings.AppSettings["port"] != null) 
            {
                _port = Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString());
            }
        }

        public void Start()
        {
            listenerThread = new Thread(new ThreadStart(this.Listener));
            listenerThread.Start();
        }

        protected void Listener() {
            TcpServerChannel c = new TcpServerChannel(_port);
            ChannelServices.RegisterChannel(c, true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Listener), "csharp_win_svc", WellKnownObjectMode.SingleCall);
        }

        public void Stop() {
            listenerThread.Abort();
        }
    }
}
