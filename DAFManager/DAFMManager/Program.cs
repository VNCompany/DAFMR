using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using lm_lib;
using sm_lib;
using System.Diagnostics;
using System.Runtime.InteropServices;
using sync_manager;

namespace DAFManager
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int procID);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                CheckingService();
            }catch(Exception)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Directory.Exists(Path.Combine(Constants.PROG_DIR, "data"))) Directory.CreateDirectory(Path.Combine(Constants.PROG_DIR, "data"));
            if (Directory.Exists(Constants.SETTINGS_PATH)) Directory.CreateDirectory(Constants.SETTINGS_PATH);

            Settings.Path = Constants.SETTINGS_PATH + "\\" + "config.ini";
            Settings.Open();

            if (Process.GetProcessesByName("DAFManager").Length > 1)
            {
                File.Create(Constants.PROG_DIR + "\\data\\" + "showform");
                return;
            }
            if (args.Length == 0 || (!args.Contains("/s") && !args.Contains("dafm_admin")))
            {
                if (Directory.Exists(Constants.SETTINGS_PATH) && !File.Exists(Constants.PROG_DIR + "\\startup"))
                {
                    if (Settings.Items["auth_type"] == null)
                    {
                        Application.Run(new DAFMManager.Auth());
                    }else if(int.TryParse(Settings.Items["auth_type"].Value, out int auth_type))
                    {
                        try
                        {
                            Application.Run(new DAFMManager.Auth((AuthShowType)auth_type));
                        }catch(System.ObjectDisposedException)
                        {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        Application.Run(new DAFMManager.Auth());
                    }
                }
                else
                {
                    Application.Run(new Startup());
                }
            }
            else if (args.Length > 0 && args.Contains("/s"))
            {
                AttachConsole(-1);
                if (args.Contains("/ver"))
                {
                    Console.WriteLine(Constants.PROG_VERSION);
                }
            }else if(args.Length == 3 &&
                     args[0] == "dafm_admin" &&
                     args[1] == "/show" &&
                     args[2] == "/non-auth")
            {
                if (Directory.Exists(Constants.SETTINGS_PATH) && !File.Exists(Constants.PROG_DIR + "\\startup"))
                {
                    Application.Run(new Main());
                }
                else
                {
                    Application.Run(new Startup());
                }
            }
        }

        static void CheckingService()
        {
            try
            {
                Synchronization sc = new Synchronization();
                if (Synchronization.GetDAFMSState() == Synchronization.ServiceState.NotInstalled)
                {
                    string res = sc.DAFMSInstall();
                    if (res != "Success" && res != "AlreadyInstalled")
                    {
                        MessageBox.Show(res);
                        return;
                    }
                    sc.DAFMSStart();
                    Console.WriteLine("DAFMSINSTALLED");
                }
                else if (Synchronization.GetDAFMSState() == Synchronization.ServiceState.Stopped)
                {
                    sc.DAFMSStart();
                    Console.WriteLine("DAFMSStart");
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new Exception();
            }
        }

        public static Log Log = new Log(Constants.PROG_DIR + "\\" + "log.txt");

        public static SWorker Settings = new SWorker();
    }

    public static class Constants
    {
        public static readonly string PROG_VERSION = "104";
        public static readonly int UPDATE_CHECK_INTERVAL = 1000 * 60 * 60 * 2;

        public static string SETTINGS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VNCompany", "DAFM");
        public static string CURRENT_PATH = Environment.CurrentDirectory;
        public static string PROG_DIR = Path.GetDirectoryName(Application.ExecutablePath);
        public static string PROG_DATA_DIR = Path.Combine(PROG_DIR, "data");
    }

    public enum AuthShowType
    {
        None = 0,
        OneLogin = 1,
        AutoAuth = 2
    }
}
