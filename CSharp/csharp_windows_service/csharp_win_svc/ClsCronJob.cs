using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace csharp_win_svc
{
    class ClsCronJob
    {
        public void DoSomething(object State) {
            string _src = "Windows Service Test";
            string _log = "CronJob";
            string _event = "Test Event: I'm alive";

            if (! EventLog.SourceExists(_src)) {
                EventLog.CreateEventSource(_src, _log);
            }

            EventLog.WriteEntry(_src, _event);
        }
    }
}
