using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace csharp_win_svc
{
    public partial class csharp_win_svc : ServiceBase
    {
        private ClsCronJob job;
        private Timer startTimer;
        private TimerCallback timerDelegate;

        private ClsScriptServer ScriptServer;

        public csharp_win_svc()
        {
            InitializeComponent();
            this.ServiceName = "csharp_win_svc";
            this.CanStop = true;
            this.AutoLog = true;
            this.CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            // open a port, listen for event.
            ScriptServer = new ClsScriptServer();
            ScriptServer.Start();

            // start a periodic event.
            job = new ClsCronJob();
            timerDelegate = new TimerCallback(job.DoSomething);
            startTimer = new Timer(timerDelegate, null, 1000, 5000);
        }

        protected override void OnStop()
        {
            ScriptServer.Stop();
            startTimer.Dispose();
        }

        protected override void OnShutdown()
        {
            ScriptServer.Stop();
            startTimer.Dispose();
        }
    }
}
