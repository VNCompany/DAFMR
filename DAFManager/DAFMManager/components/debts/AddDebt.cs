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
using System.Text.RegularExpressions;


namespace DAFManager.components.debts
{
    public partial class AddDebt : Form
    {
        public bool OK = false;
        User[] users;
        Priority[] priorities;
        DataBaseManager dbm;
        public AddDebt(DataBaseManager dbm)
        {
            InitializeComponent();
            if (checkBox1.Checked) priority.Enabled = false;
            priorities = dbm.GetPriorities();
            users = dbm.GetDebtors(priorities);
            debtorName.Items.AddRange(users.Select(t => t.Name).ToArray());
            priority.Items.AddRange(priorities.Select(t => $"{t.Name} {t.Label}").ToArray());
            this.dbm = dbm;
            if(priority.Items.Count == 0)
            {
                priority.Items.Add("0 Обычный");
                priorities = new Priority[1] { new Priority(0, "Обычный", "White") };
            }
                

            priority.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(debtorName.Text)) { MessageBox.Show("Введите имя задолжника!"); return; }
            if (priority.SelectedItem == null && !checkBox1.Checked) { MessageBox.Show("Выберите приоритет!"); return; }

            #region Regex Parser
            Regex rx1 = new Regex("(.+?)->([0-9]+)");
            Match mt = rx1.Match(debtorName.Text);
            if (mt.Success)
            {
                try
                {
                    decimal ss1 = Convert.ToDecimal(mt.Groups[2].Value);
                    if (ss1 > 9999999) return;
                    amount.Value = ss1;
                    debtorName.Text = mt.Groups[1].Value;
                }
                catch(Exception)
                {
                    return;
                }
            }
            #endregion

            string selDebtor = debtorName.Text;
            int selPri = Convert.ToInt32(priority.SelectedItem.ToString().Split(' ')[0]);
            DateTime selDate = createDate.Value;
            string selDesc = desc.Text;
            int selAmount = Convert.ToInt32(amount.Value);
            User selUser;

            if (users.Where(t => t.Name == selDebtor).Count() > 0)
                selUser = users.First(t => t.Name == selDebtor);
            else
            {
                selUser = new User(0, selDebtor, priorities[0], "");
                dbm.AddUser(selUser);
            }

            dbm.AddDebt(new Debt(
                0,
                selDesc,
                selAmount,
                selUser,
                !checkBox1.Checked ? priorities.First(t => t.Name == selPri) : selUser.Priority,
                selDate,
                ""
                ));
            OK = true;
            Close();
        }

        private void DebtorName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Button1_Click(sender, new EventArgs());
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            priority.Enabled = !checkBox1.Checked;
        }
    }
}
