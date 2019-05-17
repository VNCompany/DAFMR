using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

using dbm_lib;
using dbm_lib.components;
using sm_lib;
using lm_lib;

namespace DAFManager
{
    public partial class Main
    {
        DataBaseManager dbm;
        List<Priority> Priorities = new List<Priority>();
        List<Debt> Debts = new List<Debt>();
        public UpdateManager updater;
        public Thread updateTh;

        public void InitializeVariables()
        {
            UpdateExisted += Update_Existed;

            dbm = new DataBaseManager();
            updater = new UpdateManager(Path.GetDirectoryName(Application.ExecutablePath));
            updateTh = new Thread(new ParameterizedThreadStart(TH_Update));
            updateTh.Start(updater);

            timerUpdate.Interval = Constants.UPDATE_CHECK_INTERVAL;
            timerUpdate.Tick += Tick_Update;
            timerUpdate.Start();

            Program.Settings.Path = Constants.SETTINGS_PATH + "\\" + "config.ini";
            Program.Settings.Open();
            dbm.CheckTables();
            UpdateAll();
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
            foreach (Debt item in Debts)
            {
                view.Items.Add(new ListViewItem(new string[] { item.ID.ToString(), item.Debtor.Name, item.Amount.ToString() + GetCurrency(), item.Desc, item.CreateDate.ToString() }, 0, Tools.GetForeColorFromThis(item.Priority.Color), item.Priority.Color, new Font("Verdana", 9, FontStyle.Regular)));
            }
        }

        public void UpdateCounters()
        {
            fullAmount.Text = dbm.GetSumAmounts().ToString() + GetCurrency();
            debtorsCount.Text = dbm.GetCount(DataBaseManager.GetCount_Bases.Users).ToString();
            debtsCount.Text = dbm.GetCount(DataBaseManager.GetCount_Bases.Debts).ToString();
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
            MessageBox.Show("Произошла непредвиденная ошибка.\n Программа активно разрабатывается и в некоторых случаях требует доработки. Информация об ошибке сохранена в файл log.txt - отправьте его администратору. Извините за неудобства.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (!(ex is ThreadAbortException))
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
                    UpdateExisted?.Invoke(url, version);
                }
                else
                {
                    MessageBox.Show("Обновлений не найдено.");
                    timerUpdate.Start();
                }
            }
            catch (Exception ex)
            {
                GetExceptionMessage(ex, "Main_Func.cs:118");
            }
        }

        public string GetVersion(string int_ver)
        {
            return "v" + int_ver[0] + "." + int_ver.Substring(1);
        }

        public delegate void UpdateProgHandler(string url, string version);
        public event UpdateProgHandler UpdateExisted;
    }
}
