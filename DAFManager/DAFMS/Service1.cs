using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using sm_lib;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace DAFMS
{
    public partial class Service1 : ServiceBase
    {
        BackgroundWorker bgw;
        bool isBusy = false;
        public Service1()
        {
            InitializeComponent();

            ServiceName = "DAFMS";
            CanPauseAndContinue = true;
            CanStop = true;

            bgw = new BackgroundWorker();
            bgw.DoWork += Work;
            bgw.WorkerSupportsCancellation = true;
        }

        private void Work(object sender, DoWorkEventArgs e)
        {
            string ip = "127.0.0.1";
            int port = 8080;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(5);

            while (true)
            {
                if (!isBusy) break;

                Socket listener = socket.Accept();
                byte[] buffer = new byte[256];
                int size = 0;
                StringBuilder data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                } while (listener.Available > 0);

                string status = new Commander().CmdWorker(data.ToString());

                listener.Send(Encoding.UTF8.GetBytes(status));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();

                if (status == "closed")
                    break;
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        protected override void OnStart(string[] args)
        {
            isBusy = true;
            bgw.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            isBusy = false;
            if(bgw.IsBusy)
                bgw.CancelAsync();
            bgw.Dispose();
        }
    }
}
