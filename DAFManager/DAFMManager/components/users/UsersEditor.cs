using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbm_lib;
using dbm_lib.components;
using lm_lib;

namespace DAFManager.components.users
{
    public partial class UsersEditor : Form
    {
        List<Debt> debts;
        DataBaseManager dbm;
        Main main;
        public UsersEditor(DataBaseManager dbm, List<Debt> debts, Main th)
        {
            InitializeComponent();
            this.debts = debts;
            this.dbm = dbm;
            main = th;

            PrintAllUsers();
        }

        public void PrintAllUsers()
        {

            if (!dbm.isBusy) { Close(); return; }

            listView1.Items.Clear();
            foreach (User user in dbm.GetDebtors(dbm.GetPriorities()))
            {
                var uu = debts.Where(t => t.Debtor.Name == user.Name);
                listView1.Items.Add(new ListViewItem(new string[] { user.ID.ToString(), user.Name, uu.Sum(t => t.Amount).ToString(), uu.Count().ToString() }, 0, Tools.GetForeColorFromThis(user.Priority.Color), user.Priority.Color, new Font("Verdana", 9, FontStyle.Regular)));
            }
        }

        private void ДобавитьНовогоToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!dbm.isBusy) { Close(); return; }

            var f = dbm.GetPriorities();
            UserAdd ua = new UserAdd(dbm, (from t in dbm.GetDebtors(f) select t.Name).ToArray());
            ua.Construct();
            if (ua.ShowForm())
            {
                dbm.AddUser(new User(0, ua.UserName, ua.SelectedPriority, ""));
                PrintAllUsers();
                sync_manager.Synchronization.Changes += 1;
            }
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!dbm.isBusy) { Close(); return; }

            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lwi in listView1.SelectedItems)
                {
                    if(lwi.SubItems[3].Text != "0")
                    {
                        if (MessageBox.Show("У пользователя ещё имеются долги! Удаление " +
                            "пользователя понесёт удаление всех его долгов из базы. Вы " +
                            "уверены, что хотите удалить \"" + lwi.SubItems[1].Text + "\"?", "Info", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            return;

                    }
                    dbm.DeleteUser(Convert.ToInt32(lwi.SubItems[0].Text));
                    main.UpdateAll();
                    main.UpdateCounters();
                    main.PrintAllDebts();
                    PrintAllUsers();
                    sync_manager.Synchronization.Changes += 1;
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одного пользователя!");
            }
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) УдалитьToolStripMenuItem_Click(sender, new EventArgs());
        }

        private void ИзменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!dbm.isBusy) { Close(); return; }

            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem selItem = listView1.SelectedItems[0];
                User[] users = dbm.GetDebtors(dbm.GetPriorities());
                User selUser = users.FirstOrDefault(t => t.ID.ToString() == selItem.SubItems[0].Text);
                using (UserAdd ua = new UserAdd(dbm, users.Select(t => t.Name).ToArray()) {
                    EType = UserAdd.EditType.Edit,
                    UserName = selUser.Name,
                    SelectedPriority = selUser.Priority
                })
                {
                    ua.Construct();
                    if (ua.ShowForm())
                    {
                        UpdateConstructor uc = new UpdateConstructor();
                        uc.Wheres = "`id`='" + selUser.ID + "'";
                        uc.Values.Add(new DBKV("name", ua.UserName));
                        uc.Values.Add(new DBKV("priority", ua.SelectedPriority.Name.ToString()));
                        dbm.EditUser(uc);
                        PrintAllUsers();
                        sync_manager.Synchronization.Changes += 1;
                    }
                }
            }
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {

            if (!dbm.isBusy) { Close(); return; }

            if (listView1.SelectedItems.Count == 1)
            {
                if (debts.Count != main.GetElementsCount())
                    main.CloseSearch();

                main.Search(listView1.SelectedItems[0].SubItems[1].Text, Main.SearchType.Debtor);
                main.Focus();
            }
        }
    }
}
