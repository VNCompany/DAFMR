using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

using dbm_lib;
using dbm_lib.components;
using sm_lib;
using lm_lib;
using sync_manager;

namespace DAFManager
{
    public partial class Main
    {
        DataBaseManager dbm;
        List<Priority> Priorities = new List<Priority>();
        List<Debt> Debts = new List<Debt>();
        public UpdateManager updater;
        public Thread updateTh;
        public Synchronization synchronization;
        bool synced = false;

        public void InitializeVariables()
        {
            UpdateExisted += Update_Existed;

            if(File.Exists(Constants.PROG_DIR + "\\changes.html"))
            {
                new ChangesDialog(File.ReadAllText(Constants.PROG_DIR + "\\changes.html")).ShowDialog();
                File.Delete(Constants.PROG_DIR + "\\changes.html");
            }

            dbm = new DataBaseManager();
            updater = new UpdateManager(Path.GetDirectoryName(Application.ExecutablePath));
            updateTh = new Thread(new ParameterizedThreadStart(TH_Update));
            updateTh.Start(updater);

            timerUpdate.Interval = Constants.UPDATE_CHECK_INTERVAL;
            timerUpdate.Tick += Tick_Update;
            timerUpdate.Start();

            OpenWinProp();

            dbm.CheckTables();
            UpdateAll();

            PrintAllDebts();

            synchronization = new Synchronization();
            if (SyncEnabled())
            {
                timer1.Start();
            }
            checkFormState.Start();

            HideZeroDebts();
        }

        public bool SyncEnabled()
        {
            SettingsCollection sc = Program.Settings.Items;
            if (sc["sync_enabled"] != null)
            {
                if (bool.TryParse(sc["sync_enabled"].Value, out bool res))
                {
                    if (res && sc["sync_path"] != null)
                        return Directory.Exists(sc["sync_path"].Value);
                }
            }
            return false;
        }

        public void UpdateAll()
        {
            UpdatePriorities();
            UpdateDebts();
        }

        public void UpdatePriorities()
        {
            if (Priorities.Count > 0)
                Priorities.Clear();
            Priorities.AddRange(dbm.GetPriorities());
        }

        public void UpdateDebts()
        {
            if (Debts.Count > 0)
                Debts.Clear();
            Priority[] pr = Priorities.ToArray();
            Debts.AddRange(dbm.GetDebts(pr).OrderByDescending(t => t.CreateDate));
        }

        public string GetCurrency()
        {
            if (Program.Settings.Items["Currency"] != null) return " " + Program.Settings.Items["Currency"].Value;

            return " р.";
        }

        public void PrintAllDebts()
        {
            view.Items.Clear();
            List<ListViewItem> lwis = new List<ListViewItem>();
            foreach (Debt item in Debts)
            {
                lwis.Add(new ListViewItem(new string[] { item.ID.ToString(), item.Debtor.Name, item.Amount.ToString() + GetCurrency(), item.Desc, item.CreateDate.ToString() }, 0, Tools.GetForeColorFromThis(item.Priority.Color), item.Priority.Color, new Font("Verdana", 9, FontStyle.Regular)));
            }
            lwis = SSort(lwis, checkBox1.Checked);

            foreach(var lwi in lwis)
            {
                view.Items.Add(lwi);
            }

            if (checkBox2.Checked)
                HideZeroDebts();
            else
                UpdateCounters();
        }

        public void UpdateCounters()
        {
            if (!checkBox2.Checked)
            {
                fullAmount.Text = dbm.GetSumAmounts().ToString() + GetCurrency();
                debtorsCount.Text = dbm.GetCount(DataBaseManager.GetCount_Bases.Users).ToString();
                debtsCount.Text = dbm.GetCount(DataBaseManager.GetCount_Bases.Debts).ToString();
            }
            else
                GetManualCounter();
        }

        public void UpdateCounters(object debts_count, object debts_sum, object debtors_count)
        {
            if (!checkBox2.Checked)
            {
                if (debts_count.ToString() != "-1") debtsCount.Text = debts_count.ToString();
                if (debtors_count.ToString() != "-1") debtorsCount.Text = debtors_count.ToString();
                if (debts_sum.ToString() != "-1") fullAmount.Text = debts_sum.ToString();
            }
            else
                GetManualCounter();
        }

        public void DeleteDebtOs()
        {
            try
            {
                if (view.SelectedItems.Count > 1)
                {
                    int[] ids = new int[view.SelectedItems.Count];

                    for (int i = 0; i < ids.Length; i++)
                    {
                        ids[i] = Convert.ToInt32(view.SelectedItems[i].SubItems[0].Text);
                    }

                    dbm.DeleteDebts(ids);
                }
                else
                {
                    dbm.DeleteDebt(Convert.ToInt32(view.SelectedItems[0].SubItems[0].Text));
                }
            }
            catch (Exception ex)
            {
                GetExceptionMessage(ex, "Main_Func.cs:75");
            }
        }

        public static void GetExceptionMessage(Exception ex, string dop = "")
        {
            Program.Log.WriteLineF("{0} - {1}:" + dop, Log.LogType.ERR, ex.GetType().FullName, ex.Message);
            MessageBox.Show(string.Format("Fatal error. \n{0}: \"{1}\". \nError saved in the log.", ex.GetType().FullName, ex.Message), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void TH_Update(object um)
        {
            try
            {
                UpdateManager th_up = um as UpdateManager;
                th_up.GetElements();
                if (th_up.CheckUpdates(out string url, out string version))
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        UpdateExisted?.Invoke(th_up.URL + "/" + url, version);
                    }));
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException) && !(ex is System.Net.WebException))
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        Thread_Error(this, new EventArgs());
                        GetExceptionMessage(ex, "Main_Func.cs:118");
                    }));
                }
            }
        }

        private void Tick_Update(object sender, EventArgs e)
        {
            if (updateTh.ThreadState == ThreadState.Stopped)
            {
                updateTh = new Thread(new ParameterizedThreadStart(TH_Update));
                updateTh.Start(updater);
            }
        }

        private void Thread_Error(object sender, EventArgs e)
        {
            timerUpdate.Stop();
        }

        private void Update_Existed(string url, string version)
        {
            string str_version = GetVersion(version);
            if (MessageBox.Show($"Доступна новая версия программы - {str_version}! \nРекомендуется установить их. Установить обновления?", "Доступны обновления!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(Path.Combine(Constants.PROG_DIR, "Updater.exe"), $"/up {str_version} {url}");
                Close();
            }
        }

        public void CheckUpdates()
        {
            timerUpdate.Stop();
            try
            {
                updater.GetElements();
                if (updater.CheckUpdates(out string url, out string version))
                {
                    if (updateTh.ThreadState != ThreadState.Stopped) updateTh.Abort();
                    UpdateExisted?.Invoke(updater.URL + "/" + url, version);
                }
                else
                {
                    MessageBox.Show("Обновлений не найдено.");
                    timerUpdate.Start();
                }
            }
            catch (Exception ex)
            {
                if(ex is System.Net.WebException)
                {
                    MessageBox.Show("Подключение к интернету отсутствует.");
                }
                else
                {
                    GetExceptionMessage(ex, "Main_Func.cs:118");
                }
            }
        }

        public string GetVersion(string int_ver)
        {
            StringBuilder str_build = new StringBuilder();
            str_build.Append("v");
            str_build.Append(int_ver.Substring(0, int_ver.Length - 2));
            str_build.Append("." + int_ver.Substring(int_ver.Length - 2));
            return str_build.ToString();
        }

        public void SaveWinProp()
        {
            SaveWindowProperties swp = new SaveWindowProperties(Width, Height, Location.X, Location.Y, checkBox2.Checked);

            BinaryFormatter binary = new BinaryFormatter();
            using(FileStream fs = new FileStream(Path.Combine(Constants.PROG_DATA_DIR, "win_prop.bin"), FileMode.OpenOrCreate))
            {
                binary.Serialize(fs, swp);
            }
        }

        public void OpenWinProp()
        {
            FileInfo bin = new FileInfo(Path.Combine(Constants.PROG_DATA_DIR, "win_prop.bin"));
            if (bin.Exists)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using(FileStream fs = new FileStream(bin.FullName, FileMode.OpenOrCreate))
                {
                    SaveWindowProperties swp = (SaveWindowProperties)bf.Deserialize(fs);

                    Width = swp.Width;
                    Height = swp.Height;

                    Location = new Point(swp.LocationX, swp.LocationY);

                    checkBox2.Checked = swp.HideZeroDebt;
                }
            }
        }

        public void HideZeroDebts()
        {
            if (checkBox2.Checked)
            {
                bool exists = false;
                foreach(ListViewItem lwi in view.Items)
                {
                    if (lwi.SubItems[2].Text.Split(' ')[0] == "0")
                    {
                        lwi.Remove();
                        exists = true;
                    }
                }

                if (exists)
                {
                    GetManualCounter();
                }
            }
        }

        private void GetManualCounter()
        {
            List<string> user_history = new List<string>();
            int sum = 0;
            int debts_count = 0;
            int debtors_count = 0;

            foreach (ListViewItem lwi in view.Items)
            {
                sum += Convert.ToInt32(lwi.SubItems[2].Text.Split(' ')[0]);
                debts_count += 1;
                if (!user_history.Contains(lwi.SubItems[1].Text))
                {
                    user_history.Add(lwi.SubItems[1].Text);
                    debtors_count += 1;
                }
            }

            fullAmount.Text = sum.ToString() + GetCurrency();
            debtorsCount.Text = debtors_count.ToString();
            debtsCount.Text = debts_count.ToString();
        }

        public void Icon_Downloading()
        {
            notifyIcon1.Icon = Properties.Resources.sync_icon;
        }

        public void Icon_Default()
        {
            notifyIcon1.Icon = Properties.Resources.default_icon;
        }

        public delegate void UpdateProgHandler(string url, string version);
        public event UpdateProgHandler UpdateExisted;
    }
}
