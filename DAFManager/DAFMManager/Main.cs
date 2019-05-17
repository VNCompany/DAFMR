using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using lm_lib;

namespace DAFManager
{
    public partial class Main : Form
    {
        DAFMManager.Auth AUTH;
        public Main(DAFMManager.Auth auth)
        {
            InitializeComponent();

            AUTH = auth;

            try
            {
                InitializeVariables();

                PrintAllDebts();
                UpdateCounters();
            } catch (Exception ex)
            {
                GetExceptionMessage(ex, "Main.cs:24");
                Close();
            }
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
            updateTh.Abort();
            AUTH.Close();
        }

        private void ДобавитьДолгToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
        }

        private void ПриоритетыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Priorities_Manager pm = new Priorities_Manager(Priorities.ToArray(), dbm);
            pm.ShowDialog();
            if (pm.UpdatePriorities) { UpdatePriorities(); UpdateDebts(); PrintAllDebts(); }
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
        }

        public void CloseSearch()
        {
            Button2_Click(this, new EventArgs());
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
                    if (ed.ShowForm(out dbm_lib.UpdateConstructor uc)) { dbm.EditDebt(uc); UpdateDebts(); PrintAllDebts(); }
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
            Close();
            Main_FormClosed(sender, new FormClosedEventArgs(CloseReason.UserClosing));
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
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "XLSX Files|*.xlsx";
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string[][] vals = ex_lib.ExcelWorker.Parse(ofd.FileName);
                        List<dbm_lib.InsertConstructor> insert = new List<dbm_lib.InsertConstructor>();
                        for(int i = 0; i < vals.Length; i++)
                        {
                            dbm_lib.InsertConstructor ins = new dbm_lib.InsertConstructor();
                            ins.TableName = "Debts";
                            ins.Values.Add(new dbm_lib.DBKV("name", vals[i][0]));
                            if (!dbm.UserExists(vals[i][0])) dbm.AddUser(new dbm_lib.components.User(0, vals[i][0], Priorities[0], ""));
                            ins.Values.Add(new dbm_lib.DBKV("debt", vals[i][1]));
                            ins.Values.Add(new dbm_lib.DBKV("priority", vals[i][2]));
                            string date = vals[i][3] == "-" ? DateTime.Now.ToString() : vals[i][3];
                            ins.Values.Add(new dbm_lib.DBKV("date", date));
                            ins.Values.Add(new dbm_lib.DBKV("desc", vals[i][4]));
                            insert.Add(ins);
                        }
                        string val = string.Join(";", insert.Select(t => t.ToString()));
                        dbm.SQLITECMD_NONE(val);
                        UpdateDebts();
                        UpdateCounters();
                        PrintAllDebts();
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Не удалось экспортировать данные.");
            }
        }
    }
}
