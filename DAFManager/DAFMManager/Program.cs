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

            if (Process.GetProcessesByName("DAFManager").Length > 1) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0 || !args.Contains("/s"))
            {
                if (Directory.Exists(Constants.SETTINGS_PATH) && !File.Exists("startup"))
                {
                    if(File.Exists(Path.Combine(Constants.PROG_DIR, "add_column_history")))
                    {
                        File.Delete(Path.Combine(Constants.PROG_DIR, "add_column_history"));
                        dbm_lib.DataBaseManager dbm = new dbm_lib.DataBaseManager();
                        dbm.AddColumn("history", "TEXT DEFAULT ''");
                        dbm.Close();
                        dbm = null;
                    }

                    Application.Run(new DAFMManager.Auth());
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
            }
        }

        public static Log Log = new Log(Constants.PROG_DIR + "\\" + "log.txt");

        public static SWorker Settings = new SWorker();
    }

    public static class Constants
    {
        public static readonly string PROG_VERSION = "103";
        public static readonly int UPDATE_CHECK_INTERVAL = 1000 * 60 * 60 * 2;

        public static string SETTINGS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VNCompany", "DAFM");
        public static string CURRENT_PATH = Environment.CurrentDirectory;
        public static string PROG_DIR = Path.GetDirectoryName(Application.ExecutablePath);
    }
}
