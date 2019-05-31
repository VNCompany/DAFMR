using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace sync_manager
{
    public class Synchronization
    {
        public enum WorkState { Working, Paused, Stopped }

        Thread th;
        WorkState state;

        int sleep_time = 10000;

        public string LocalBase { get; set; }
        public string ServerBase { get; set; }

        public static int changes = 0;

        public WorkState State
        {
            get
            {
                return state;
            }
        }

        public Synchronization()
        {
            state = WorkState.Stopped;
            th = new Thread(SyncMethod);
        }

        public Synchronization(string local, string sync)
        {
            LocalBase = local;
            ServerBase = sync;
            state = WorkState.Stopped;
            th = new Thread(SyncMethod);
        }

        public void Start()
        {
            state = WorkState.Working;
            if(th.ThreadState == ThreadState.Unstarted || th.ThreadState == ThreadState.Stopped)
            {
                th.Start();
            }
            else
            {
                th.Abort();
                Thread.Sleep(200);
                th.Start();
            }
        }

        public void Pause()
        {
            if(th.ThreadState == ThreadState.Running)
            {
                state = WorkState.Paused;
            }
        }

        public void Stop(bool except = false)
        {
            if (except)
            {
                th.Abort();
            }
            else
            {
                state = WorkState.Stopped;
            }
        }

        private void SyncMethod()
        {
            FileInfo cur_base = new FileInfo(LocalBase);
            FileInfo ser_base = new FileInfo(ServerBase);

            while (true)
            {
                if (State == WorkState.Stopped) break;
                if (State == WorkState.Paused)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (File.Exists(ser_base.FullName))
                {
                    if (changes > 0)
                    {
                        if (File.Exists(Path.Combine(ser_base.DirectoryName, "base_old.db")))
                            File.Delete(Path.Combine(ser_base.DirectoryName, "base_old.db"));
                        ser_base.MoveTo(Path.Combine(ser_base.DirectoryName, "base_old.db"));
                        cur_base.CopyTo(Path.Combine(ser_base.DirectoryName, "base.db"));
                        changes = 0;
                    }
                }
                else
                {
                    cur_base.CopyTo(Path.Combine(ser_base.DirectoryName, "base.db"));
                }
                Thread.Sleep(sleep_time);
            }
        }

        public void GetFile()
        {
            if(File.Exists(ServerBase))
                System.Diagnostics.Process.Start("SyncUploader.exe", ServerBase);
        }
    }
}
