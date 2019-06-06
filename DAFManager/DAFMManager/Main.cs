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

using lm_lib;
using System.IO;

namespace DAFManager
{
    public partial class Main : Form
    {
        DAFMManager.Auth AUTH;
        int sort_type = -1;
        Panel thinking;
        public Main(DAFMManager.Auth auth=null)
        {
            InitializeComponent();

            AUTH = auth;

            try
            {
                InitializeVariables();
                UpdateCounters();
            } catch (Exception ex)
            {
                GetExceptionMessage(ex, "Main.cs:24");
                Close();
            }

            thinking = new Panel();
            thinking.Dock = DockStyle.Fill;
            thinking.BackColor = Color.White;
            Label label = new Label();
            label.Text = "Синхронизация...";
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font("Verdana", 36, FontStyle.Bold);
            thinking.Visible = false;
            thinking.Controls.Add(label);
            Controls.Add(thinking);
        }

        public void ShowThink()
        {
            menuStrip1.Hide();
            thinking.BringToFront();
            thinking.Show();
        }

        public void HideThink()
        {
            menuStrip1.Show();
            thinking.Hide();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (view.SelectedItems.Count > 0)
            {
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                contextMenuStrip1.Enabled = false;
            }
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (view.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить выделенные долг(и)?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    DeleteDebtOs();
                    UpdateDebts();
                    UpdateCounters();
                    PrintAllDebts();
                    sync_manager.Synchronization.Changes += 1;
                }
            }
        }

        private void ПользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new components.users.UsersEditor(dbm, Debts, this).Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            components.debts.AddDebt form = new components.debts.AddDebt(dbm);
            form.ShowDialog();

            if (form.OK)
            {
                UpdateDebts();
                PrintAllDebts();
                UpdateCounters();
                sync_manager.Synchronization.Changes += 1;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            UpdateAll();
            UpdateCounters();
            PrintAllDebts();
        }

        private void Main_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = true;
                WindowState = FormWindowState.Normal;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Settings.Save();
            if(synced)
                // TODO: остановка синхронизации
            updateTh.Abort();
            if(AUTH != null)
                AUTH.Close();
            checkFormState.Stop();

            SaveWinProp();
        }

        private void ДобавитьДолгToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
        }

        private void ПриоритетыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Priorities_Manager pm = new Priorities_Manager(Priorities.ToArray(), dbm);
            pm.ShowDialog();
            if (pm.UpdatePriorities) { UpdatePriorities(); UpdateDebts(); PrintAllDebts(); sync_manager.Synchronization.Changes += 1; }
            pm.Dispose();
            pm = null;
            GC.Collect();
        }

        private void St_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Stb_Click(sender, new EventArgs());
        }

        private void Stb_Click(object sender, EventArgs e)
        {
            Search(st.Text);

            button3.Show();
            st.Text = "";
            st.Focus();
        }

        public enum SearchType { ID = 0, Debtor = 1, Summa = 2, Desc = 3, Date = 4, All = 5 }
        public void Search(string input, SearchType type = SearchType.All)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            List<ListViewItem> lwis = new List<ListViewItem>();
            foreach (ListViewItem lwi in view.Items) lwis.Add(lwi);

            lwis = SSort(lwis, checkBox1.Checked);

            view.Items.Clear();
            foreach (ListViewItem lwi in lwis)
            {
                if(type == SearchType.All)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in lwi.SubItems)
                    {
                        if (subItem.Text.ToLower().Contains(input.ToLower()))
                        {
                            view.Items.Add(lwi);
                            break;
                        }
                    }
                }
                else
                {
                    if (lwi.SubItems[(int)type].Text.ToLower().Contains(input.ToLower()))
                    {
                        view.Items.Add(lwi);
                    }
                }
            }
            UpdateCounters(GetElementsCount(), ViewSum(), -1);
        }

        public void CloseSearch()
        {
            Button2_Click(this, new EventArgs());
        }

        public int ViewSum()
        {
            int sum = 0;
            foreach (ListViewItem lwi in view.Items)
            {
                sum += Convert.ToInt32(lwi.SubItems[2].Text.Split(' ')[0]);
            }
            return sum;
        }

        public int GetElementsCount()
        {
            return view.Items.Count;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            CloseSearch();
            button3.Hide();
        }

        private void ИзменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(view.SelectedItems.Count == 1)
            {
                using (EditDebt ed = new EditDebt(Debts.First(t => t.ID.ToString() == view.SelectedItems[0].SubItems[0].Text), dbm, Priorities.ToArray()))
                {
                    if (ed.ShowForm(out dbm_lib.UpdateConstructor uc)) { dbm.EditDebt(uc); UpdateDebts(); PrintAllDebts(); sync_manager.Synchronization.Changes += 1; }
                }
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Settings(dbm).ShowDialog();
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Control && e.KeyCode == Keys.N)
                Button1_Click(sender, new EventArgs());

            if (e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                new DevConsole(this, dbm).Show();
            }
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AUTH != null)
            {
                Close();
                Main_FormClosed(sender, new FormClosedEventArgs(CloseReason.UserClosing));
            }
            else
            {
                //Program.Settings.Save();
                //if (synced)
                //    synchronization.Stop(true);
                //updateTh.Abort();
                Close();
            }
        }

        private void УплатаДолгаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(view.SelectedItems.Count == 1)
            {
                UpdateDebts();
                dbm_lib.components.Debt debt = Debts.FirstOrDefault(t => t.ID.ToString() == view.SelectedItems[0].SubItems[0].Text);
                if(debt != null)
                {
                    using(ResDebt rd = new ResDebt(debt.Amount))
                    {
                        if (rd.ShowForm())
                        {
                            debt.Amount -= rd.AmountSum;

                            dbm_lib.UpdateConstructor uc = new dbm_lib.UpdateConstructor();
                            uc.TableName = "Debts";
                            uc.Wheres = $"`id`={debt.ID}";

                            if (rd.HistoryEnabled)
                            {
                                debt.History.Add($"{rd.AmountSum}^{rd.CreateDate}^{rd.Description}");
                                uc.Values.Add(new dbm_lib.DBKV("history", debt.GetHistory()));
                            }
                            uc.Values.Add(new dbm_lib.DBKV("debt", debt.Amount.ToString()));

                            dbm.EditDebt(uc);

                            UpdateDebts();
                            UpdateCounters();
                            PrintAllDebts();
                            sync_manager.Synchronization.Changes += 1;
                        }
                    }
                }
            }
        }

        private void View_DoubleClick(object sender, EventArgs e)
        {
            if(view.SelectedItems.Count == 1)
            {
                dbm_lib.components.Debt dt = Debts.FirstOrDefault(t => t.ID.ToString() == view.SelectedItems[0].SubItems[0].Text);
                if (dt != null)
                {
                    using(InfoDebt inf = new InfoDebt(dt))
                    {
                        inf.ShowDialog();
                    }
                }
            }
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (DAFManager.components.help help = new DAFManager.components.help(GetVersion(Constants.PROG_VERSION)))
            {
                help.ShowDialog();
            }
        }

        private void ПроверитьОбновленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.Settings.Items["close_exit"] == null || Program.Settings.Items["close_exit"].Value.ToLower() == "false")
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                Main_Deactivate(sender, new EventArgs());
            }
        }

        private void ЭкспортироватьДолгиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(Constants.PROG_DIR, "base.db"));
                if (fi.Exists)
                {
                    System.IO.FileInfo fc = new System.IO.FileInfo(System.IO.Path.Combine(Constants.PROG_DIR, $"base_{DateTime.Now.ToShortDateString()}.db"));
                    if (fc.Exists)
                        fc.Delete();
                    fi.CopyTo(fc.FullName);
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "DAFM Recovery";
                    notifyIcon1.BalloonTipText = $"Точка восстановления \"{fc.Name}\" успешно создана.";
                    notifyIcon1.ShowBalloonTip(3000);
                }
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "XLSX Files|*.xlsx";
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string[][] vals = ex_lib.ExcelWorker.Parse(ofd.FileName);
                        List<dbm_lib.InsertConstructor> insert = new List<dbm_lib.InsertConstructor>();
                        List<string> errors = new List<string>();
                        for(int i = 0; i < vals.Length; i++)
                        {
                            try
                            {
                                dbm_lib.InsertConstructor ins = new dbm_lib.InsertConstructor();
                                ins.TableName = "Debts";
                                if (string.IsNullOrWhiteSpace(vals[i][0])) continue;
                                ins.Values.Add(new dbm_lib.DBKV("name", vals[i][0]));
                                if (!dbm.UserExists(vals[i][0])) dbm.AddUser(new dbm_lib.components.User(0, vals[i][0], Priorities[0], ""));
                                ins.Values.Add(new dbm_lib.DBKV("debt", string.IsNullOrWhiteSpace(vals[i][1]) ? "100" : vals[i][1]));
                                ins.Values.Add(new dbm_lib.DBKV("priority", string.IsNullOrWhiteSpace(vals[i][2]) ? "0" : vals[i][2]));
                                string date; // vals[i][3]
                                try
                                {
                                    Regex rx_date = new Regex(@"([0-9]{1,2}?)\.([0-9]{1,2}?)\.([0-9]{4}?)");
                                    Regex rx_time = new Regex(@"([0-9]{1,2}?):([0-9]{1,2})");

                                    Match m1 = rx_date.Match(vals[i][3]);
                                    if (m1.Success)
                                    {
                                        int d = Convert.ToInt32(m1.Groups[1].Value);
                                        int m = Convert.ToInt32(m1.Groups[2].Value);
                                        int y = Convert.ToInt32(m1.Groups[3].Value);

                                        DateTime dt;

                                        Match m2 = rx_time.Match(vals[i][3]);
                                        if (m2.Success)
                                        {
                                            int h = Convert.ToInt32(m2.Groups[1].Value);
                                            int min = Convert.ToInt32(m2.Groups[2].Value);
                                            try
                                            {
                                                dt = new DateTime(y, m, d, h, min, 0);
                                            }
                                            catch (ArgumentOutOfRangeException ex)
                                            {
                                                errors.Add("358: " + ex.Message + ". Date: " + vals[i][3]);
                                                dt = DateTime.Now;
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                dt = new DateTime(y, m, d, 0, 0, 0);
                                            }
                                            catch (ArgumentOutOfRangeException ex)
                                            {
                                                errors.Add("370: " + ex.Message + ". Date: " + vals[i][3]);
                                                dt = DateTime.Now;
                                            }
                                        }
                                        date = dt.ToString();
                                    }
                                    else
                                    {
                                        date = DateTime.Now.ToString();
                                        errors.Add("Main.cs_351: Из-за ошибки парсинга даты для долга установлена текущая дата. Входная строка: " + vals[i][3] + ".");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    date = DateTime.Now.ToString();
                                    errors.Add(string.Format("{0}: \"{1}\"", ex.GetType().FullName, ex.Message));
                                }
                                ins.Values.Add(new dbm_lib.DBKV("date", date));
                                ins.Values.Add(new dbm_lib.DBKV("desc", vals[i][4]));
                                insert.Add(ins);
                            }
                            catch (Exception ex)
                            {
                                errors.Add(string.Format("{0}: \"{1}\"", ex.GetType().FullName, ex.Message));
                            }
                        }
                        Program.Log.WriteList(from t in errors select new Log.WriteLineElement(t, Log.LogType.ERR));
                        MessageBox.Show(string.Format($"Операция импорта завершена. \nКоличество ошибок: {errors.Count}."));
                        string val = string.Join(";", insert.Select(t => t.ToString()));
                        dbm.SQLITECMD_NONE(val);
                        UpdateDebts();
                        UpdateCounters();
                        PrintAllDebts();
                        sync_manager.Synchronization.Changes += 1;
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Не удалось импортировать данные.");
                Close();
                Application.Exit();
            }
            finally
            {
                MessageBox.Show("На случай неудачного экспорта резервная копия сохраняется в папку с программой. При необходимости можно восстановить данные.");
            }
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SyncEnabled()) MessageBox.Show("Служба синхронизации выключена");

            Icon_Downloading();
            ShowThink();
            dbm.Close();
            synchronization.SyncGetAsync((result) =>
            {
                dbm.Open();
                HideThink();

                if(result == null)
                {
                    MessageBox.Show("Служба синхронизации работает неправильно, отключена или не была установлена. Исправить проблему " +
                        "можно перезапуском программы.");
                }

                if (result.Contains("успешно"))
                {
                    Button2_Click(sender, e);
                    Icon_Default();
                }
                else
                {
                    Icon_Default();
                    MessageBox.Show(result);
                }
            });
        }

        private void CheckFormState_Tick(object sender, EventArgs e)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Constants.PROG_DIR + "\\" + "showform");
            if (fi.Exists)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                fi.Delete();
            }
        }

        public List<ListViewItem> SSort(List<ListViewItem> lwis, bool reverse = false)
        {
            switch (sort_type)
            {
                case -1:
                    return lwis;
                case 0:
                    lwis.Sort((a, b) => Convert.ToInt32(a.SubItems[0].Text).CompareTo(Convert.ToInt32(b.SubItems[0].Text)));
                    break;
                case 2:
                    lwis.Sort((a, b) => Convert.ToInt32(a.SubItems[2].Text.Split(' ')[0]).CompareTo(Convert.ToInt32(b.SubItems[2].Text.Split(' ')[0])));
                    break;
                case 4:
                    lwis.Sort((a, b) => DateTime.Parse(a.SubItems[4].Text).CompareTo(DateTime.Parse(b.SubItems[4].Text)));
                    break;
            }

            if (reverse) lwis.Reverse();

            return lwis;
        }

        public void SSort()
        {
            if(sort_type != -1)
            {
                List<ListViewItem> lwis = new List<ListViewItem>();
                foreach (ListViewItem lwi in view.Items) lwis.Add(lwi);
                view.Items.Clear();

                lwis = SSort(lwis, checkBox1.Checked);

                foreach (ListViewItem lwi in lwis) view.Items.Add(lwi);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    sort_type = -1;
                    break;
                case 1:
                    sort_type = 0;
                    break;
                case 2:
                    sort_type = 2;
                    break;
                case 3:
                    sort_type = 4;
                    break;
            }
            SSort();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            PrintAllDebts();
        }

        int sync_timer_error_count = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            var ser_base = Path.Combine(Program.Settings.Items["sync_path"].Value, "base.db");
            if (SyncEnabled() && (sync_manager.Synchronization.Changes > 0 || !File.Exists(ser_base)))
            {
                Icon_Downloading();
                synchronization.SyncSendAsync((res) =>
                {
                    Icon_Default();
                    sync_manager.Synchronization.Changes = 0;
                    if (res == null)
                    {
                        MessageBox.Show("Служба синхронизации работает неправильно, отключена или не была установлена. Исправить проблему " +
                                        "можно перезапуском программы.");
                        timer1.Stop();
                        return;
                    }
                    if (!res.Contains("успешно"))
                    {
                        sync_timer_error_count += 1;
                        MessageBox.Show(res);
                        timer1.Stop(); // По желанию можно убрать
                    }
                });
            }
        }
    }
}
