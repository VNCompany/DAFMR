using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace SyncUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("Path was null");
                Console.ReadKey();
                return;
            }

            Process[] dafm = Process.GetProcessesByName("DAFManager");
            Console.WriteLine("Killing DAFManager process...");
            foreach (Process proc in dafm)
            {
                proc.Kill();
            }

            while(Process.GetProcessesByName("DAFManager").Length != 0)
            {
                Thread.Sleep(4000);
            }
            Console.WriteLine("Synchronization...");
            FileInfo n = new FileInfo(args[0]);
            if (File.Exists("base.db")) File.Delete("base.db");
            if (n.Exists)
            {
                n.CopyTo("base.db");
            }
            Console.WriteLine("Starting DAFManager...");
            Thread.Sleep(2000);
            Process.Start("DAFManager.exe");
        }
    }
}
