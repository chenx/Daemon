using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace csharp_win_svc
{
    /// <summary>
    /// A C# windows service template.
    /// </summary>
    public class Listener : System.MarshalByRefObject
    {
        private string _cmd;
        private string _arg;

        public Listener()
        {
            getConfig();
        }
        
        ~Listener() {}

        public string Greeting(string msg)
        {
            return "" + msg;
        }

        private void getConfig()
        {
            _cmd = "";
            _arg = "";

            if (ConfigurationSettings.AppSettings["cmd"] != null)
            {
                _cmd = ConfigurationSettings.AppSettings["cmd"].ToString();
            }
            if (ConfigurationSettings.AppSettings["arg"] != null)
            {
                _arg = "\"" + ConfigurationSettings.AppSettings["arg"].ToString() + "\"";
            }
        }


        /// <summary>
        /// Reference:
        /// http://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
        /// </summary>
        public string doCmd()
        {
            string output = "";
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(_cmd);
                psi.Arguments = _arg;
                psi.RedirectStandardOutput = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; 
                psi.UseShellExecute = false;
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = proc.StandardOutput;
                output = myOutput.ReadToEnd();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                output = "doCmd() error: " + ex.Message;
            }
            return output;
        }
    }
}
