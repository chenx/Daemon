using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using csharp_win_svc; // For this, add resources/csharp_win_svc.exe to References.

namespace csharp_win_svc_client
{
    /// <summary>
    /// Talk to csharp_win_svc service, invoke a vbscript, read its stdout output.
    /// 
    /// @Author: X. Chen
    /// @Created on: 7/2/2015
    /// @Last modified: 7/2/2015
    /// </summary>
    class Program
    {
        private static int timeout = 0; // In milliseconds. 0 or -1 means infinite.
        private static bool useLog;
        private static string logDir;
        private static string logFile;
        private static string[] targets = null;

        /// <summary>
        /// Get command line options.
        /// </summary>
        /// <param name="args"></param>
        private static bool getOpt(string[] args)
        {
            string arg;
            foreach (string _arg in args)
            {
                arg = _arg.ToLower();
                if (arg == "-h" || arg == "--help")
                {
                    showUsage();
                }
                else {
                    Console.WriteLine("Unknown command: " + arg);
                    showUsage();
                    return false;
                }
            }

            readConfig();

            if (useLog)
            {
                logFile = logDir + "csharp_win_svc_client_" + DateTime.Today.Year + "-" +
                    DateTime.Today.Month + "-" + DateTime.Today.Day + ".log";
            }

            if (targets == null) return false;
            return true;
        }

        /// <summary>
        /// Read configuration.
        /// </summary>
        private static void readConfig() {
            if (!CheckConfigFileIsPresent())
            {
                Console.WriteLine("Configuration file does not exist.");
                createConfigFile();
            }

            try { useLog = Convert.ToBoolean(ConfigurationSettings.AppSettings["UseLog"].ToString()); }
            catch (Exception ex) { Console.WriteLine(ex.Message); useLog = true; Environment.Exit(0); }

            try { logDir = ConfigurationSettings.AppSettings["LogDir"].ToString(); }
            catch (Exception ex) { Console.WriteLine(ex.Message); logDir = ""; Environment.Exit(0); }

            try { timeout = Convert.ToInt32(ConfigurationSettings.AppSettings["Timeout"].ToString()); }
            catch { timeout = 0; }

            try
            {
                char[] splitter = { ',' };
                targets = ConfigurationSettings.AppSettings["Targets"].ToString().Split(splitter);
            }
            catch { targets = null; }    
        }

        /// <summary>
        /// Create default configuration file.
        /// </summary>
        private static void createConfigFile() {
            string s = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
<appSettings>
    <add key=""UseLog"" value=""True"" />
    <add key=""LogDir"" value=""log/"" />
    <!--timeout unit: millisecond-->
    <add key=""Timeout"" value=""5"" />
    <!--targets are separated by comma "",""-->
    <add key=""Targets"" value=""localhost:9092"" />
</appSettings>
</configuration>
";
            string file = System.Reflection.Assembly.GetExecutingAssembly().Location + ".config";
            //Console.WriteLine(file); return;
            File.WriteAllText(file, s, System.Text.Encoding.ASCII);
            Thread.Sleep(2000); // wait for config file to be created.
        }
        
        /// <summary>
        /// Check if configuration file exists.
        /// </summary>
        /// <returns></returns>
        public static bool CheckConfigFileIsPresent()
        {
            return File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        /// <summary>
        /// Display usage information.
        /// </summary>
        private static void showUsage()
        {            
            Console.WriteLine("Usage: " + getAppName() + " [-h|--help]");
            Environment.Exit(0);
        }

        /// <summary>
        /// Get name of this executable.
        /// </summary>
        /// <returns></returns>
        private static string getAppName() {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString();
            name = name.Split(new char[] { ',' })[0];
            return name;
        }

        /// <summary>
        /// Output to DOS stdout.
        /// </summary>
        /// <param name="s"></param>
        private static void doOutput(string s)
        {
            Console.WriteLine(s);
            if (useLog) { File.AppendAllText(logFile, DateTime.Now + ": " + s + "\r\n"); }
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!getOpt(args)) return;
            
            IDictionary props = new Hashtable();
            props["timeout"] = timeout; // 0 or -1 means infinite.
            TcpClientChannel tc = new TcpClientChannel(props, null);
            ChannelServices.RegisterChannel(tc, true);

            foreach (string _target in targets)
            {
                string target = _target.Trim();
                if (target == "" || target.StartsWith("#")) continue;

                string service = "tcp://" + target + "/csharp_win_svc";
                Listener obj = (Listener)Activator.GetObject(typeof(Listener), service);
                doOutput("Connecting to " + service);
                if (obj == null)
                {
                    doOutput("Could not locate server: " + target);
                    return;
                }

                try
                {
                    doOutput(obj.Greeting("Please wait for result.."));
                    doOutput(obj.Greeting("\r\n-- remote output: start --"));
                    doOutput(obj.doCmd().Trim());
                    doOutput(obj.Greeting("-- remote output: end --\r\n"));
                }
                catch (Exception ex)
                {
                    doOutput("Error: " + ex.Message);
                }
                doOutput("End for server: " + target);
                doOutput("");
                Console.WriteLine("Press any key to continue..");
                Console.ReadLine();
            }
        }
    }
}
