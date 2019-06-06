using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Linq;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace sync_manager
{
    public class Synchronization
    {
        public int SleepTime = 10000;
        public static int Changes = 0;

        public enum ServiceState { NotInstalled, Stopped, Working }

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

        public async void SyncSendAsync(Action<string> callback)
        {
            string result = await Task.Run(() =>
            {
                return SendMessage("!sync");
            });

            callback(result);
        }

        public async void SyncGetAsync(Action<string> callback)
        {
            string result = await Task.Run(() =>
            {
                return SendMessage("!sync_get");
            });

            callback(result);
        }

        public static ServiceState GetDAFMSState()
        {
            if(ServiceController.GetServices().Any(t => t.ServiceName == "DAFMS"))
            {
                ServiceController sc = new ServiceController("DAFMS");
                if (sc.Status == ServiceControllerStatus.StartPending) Thread.Sleep(4000);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    return ServiceState.Working;
                }
                else
                    return ServiceState.Stopped;
            }
            else
            {
                return ServiceState.NotInstalled;
            }
        }

        public void DAFMSStart()
        {
            if(GetDAFMSState() == ServiceState.Stopped)
            {
                new ServiceController("DAFMS").Start();
                Thread.Sleep(5000);
            }
        }

        public void DAFMSStop()
        {
            if(GetDAFMSState() == ServiceState.Working)
            {
                new ServiceController("DAFMS").Stop();
                Thread.Sleep(5000);
            }
        }

        public string DAFMSInstall()
        {
            try
            {
                if(GetDAFMSState() == ServiceState.NotInstalled)
                {
                    string windowsFolder = @"C:\Windows\Microsoft.NET";
                    string frame = "Framework" + (Environment.Is64BitOperatingSystem ? "64" : "");
                    string ver = "v4.0.30319";
                    string installUtil = "InstallUtil.exe";

                    string Util = Path.Combine(windowsFolder, frame, ver, installUtil);
                    if (!File.Exists(Util)) return "На компьютере не установлена версия Framework 4.0!";

                    Process.Start(Util, Path.Combine(Environment.CurrentDirectory, "DAFMS.exe"));

                    Thread.Sleep(7000);

                    return "Success";
                }
                else
                {
                    return "AlreadyInstalled";
                }
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private string SendMessage(string msg)
        {
            if (GetDAFMSState() != ServiceState.Working) return null;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endPoint);
            socket.Send(Encoding.UTF8.GetBytes(msg));

            byte[] buffer = new byte[256];
            int size = 0;
            StringBuilder answer = new StringBuilder();

            do
            {
                size = socket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            } while (socket.Available > 0);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            return answer.ToString();
        }
    }
}
