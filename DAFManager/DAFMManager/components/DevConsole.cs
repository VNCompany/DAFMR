using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using dbm_lib;
using dbm_lib.components;
using sm_lib;
using System.IO;

namespace DAFManager
{
    public partial class DevConsole : Form
    {
        DataBaseManager dbm;
        Main main;
        public DevConsole(Main mn, DataBaseManager db)
        {
            InitializeComponent();
            dbm = db;
            main = mn;
        }

        private void CmdText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CommandWorker(cmdText.Text);
                cmdText.Text = "";
                cmdText.Focus();
            }
        }

        public void WriteLine(string input)
        {
            console.Text += input + Environment.NewLine;
            console.SelectionStart = console.Text.Length;
            console.ScrollToCaret();
        }

        public void Write(string input)
        {
            console.Text += input;
        }

        public void Clear()
        {
            console.Text = "";
        }

        public void CommandWorker(string cmd)
        {
            cmd = cmd.Replace("date()", DateTime.Now.ToShortDateString())
               .Replace("time()", DateTime.Now.ToShortTimeString().Replace(":", "."));

            // Main
            if (cmd == "clear") Clear();
            if (cmd.Length > 7 && cmd.Substring(0, 5) == "print") WriteLine(cmd.Substring(6));
            if (cmd == "up_debts") { main.UpdateDebts(); WriteLine("Долги обновлены"); }
            if (cmd == "up_controls") { main.PrintAllDebts(); main.UpdateCounters(); WriteLine("Мониторы обновлены"); }
            if (cmd == "up_priorities") { main.UpdatePriorities(); WriteLine("Приоритеты обновлены"); }
            if(cmd.Contains("echo "))
            {
                string[] vals = cmd.Split(' ');
                string text = string.Join(" ", vals, 1, vals.Length - 1);
                WriteLine(text);
            }

            // Settings
            Regex rx = new Regex("set \"([a-zA-Z0-9_]+?)\" \"(.*?)\"");
            Match m = rx.Match(cmd);
            if (m.Success)
            {
                string key = m.Groups[1].Value;
                string value = m.Groups[2].Value;
                if(Program.Settings.Items[key] != null)
                {
                    Program.Settings.Items[key].Value = value;
                    Program.Settings.Save();
                    WriteLine($"Ключ \"{key}\" успешно изменён!");
                }
                else
                {
                    Program.Settings.Items.Add(key, value);
                    Program.Settings.Save();
                    WriteLine($"Ключ \"{key}\" успешно создан!");
                }
            }
            else
            {
                m = null;
            }

            rx = new Regex("set del \"(.+?)\"");
            m = rx.Match(cmd);
            if (m.Success)
            {
                Program.Settings.Items.Delete(m.Groups[1].Value);
                Program.Settings.Save();
            }

            rx = new Regex("set get \"(.+?)\"");
            m = rx.Match(cmd);
            if (m.Success)
            {
                var val = Program.Settings.Items[m.Groups[1].Value];
                if (val != null)
                {
                    WriteLine(val.Value);
                }
                else
                {
                    WriteLine("null");
                }
            }

            // Recovery and Synchronization
            rx = new Regex(@"sync\(([0-9]{1,2}?)\)");
            m = rx.Match(cmd);
            if (m.Success)
            {
                sync_manager.Synchronization.Changes += Convert.ToInt32(m.Groups[1].Value);
            }

            if(cmd == "save list")
            {
                foreach(string file in Directory.GetFiles(Constants.PROG_DIR, "*.db"))
                {
                    FileInfo fi = new FileInfo(file);
                    if(fi.Name != "base.db")
                    {
                        WriteLine("  - " + fi.Name);
                    }
                    WriteLine("");
                }
            }

            if(cmd.Contains("save del "))
            {
                string[] vals = cmd.Split(' ');
                string text = string.Join(" ", vals, 2, vals.Length - 2);

                if (File.Exists(Path.Combine(Constants.PROG_DIR, text)))
                {
                    File.Delete(Path.Combine(Constants.PROG_DIR, text));
                    WriteLine($"Точка восстановления \"{text}\" удалена.");
                }
            }

            if(cmd.Contains("save create "))
            {
                string[] vals = cmd.Split(' ');
                string text = string.Join(" ", vals, 2, vals.Length - 2);

                if (!File.Exists(Path.Combine(Constants.PROG_DIR, text)))
                {
                    FileInfo one = new FileInfo(Path.Combine(Constants.PROG_DIR, "base.db"));
                    FileInfo two = new FileInfo(Path.Combine(Constants.PROG_DIR, text));

                    one.CopyTo(two.FullName);

                    WriteLine($"Точка восстановления \"{text}\" успешно создана.");
                }
                else
                {
                    WriteLine("Такой файл уже существует.");
                }
            }

            if(cmd.Contains("save export "))
            {
                string[] vals = cmd.Split(' ');
                string text = string.Join(" ", vals, 2, vals.Length - 2);

                FileInfo one = new FileInfo(Path.Combine(Constants.PROG_DIR, "base.db"));
                FileInfo two = new FileInfo(Path.Combine(Constants.PROG_DIR, text));

                if (File.Exists(Path.Combine(Constants.PROG_DIR, text)))
                {
                    Close();
                    System.Diagnostics.Process.Start("SyncUploader.exe", two.FullName);
                }
                else
                {
                    WriteLine("Точка восстановления не найдена.");
                }
            }
        }

        private void DevConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbm = null;
            main = null;
            Dispose();
        }
    }
}
