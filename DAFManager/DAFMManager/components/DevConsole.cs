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
            // Main
            if (cmd == "clear") Clear();
            if (cmd.Length > 7 && cmd.Substring(0, 5) == "print") WriteLine(cmd.Substring(6));
            if (cmd == "up_debts") { main.UpdateDebts(); WriteLine("Долги обновлены"); }
            if (cmd == "up_controls") { main.PrintAllDebts(); main.UpdateCounters(); WriteLine("Мониторы обновлены"); }
            if (cmd == "up_priorities") { main.UpdatePriorities(); WriteLine("Приоритеты обновлены"); }

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

        }

        private void DevConsole_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbm = null;
            main = null;
            Dispose();
        }
    }
}
