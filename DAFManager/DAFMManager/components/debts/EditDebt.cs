using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbm_lib.components;
using dbm_lib;

namespace DAFManager
{
    public partial class EditDebt : Form
    {
        bool OK = false;
        Priority[] p_li;
        DataBaseManager dbm;
        User[] users;

        string debt_history;

        #region Свойства
        User duser;
        int damount;
        Priority dp;
        DateTime ddate;
        string ddesc;
        string[] dargs;

        public User d_User
        {
            get
            {
                return duser;
            }

            set
            {
                if (value != null)
                    duser = value;
                else
                    throw new Exception("Имя задолжика оказалось пустым. User");
            }
        }
        public int d_Amount
        {
            get
            {
                return damount;
            }

            set
            {
                if (value <= 999999999)
                    damount = value;
                else
                    throw new Exception("Сумма долга не может быть больше 999999999");
            }
        }
        public Priority d_Priority
        {
            get
            {
                return dp;
            }

            set
            {
                if (value == null) throw new Exception("Priority is null");
                dp = value;
            }
        }
        public DateTime d_Date
        {
            get
            {
                return ddate;
            }

            set
            {
                ddate = value;
            }
        }
        public string d_Description
        {
            get
            {
                return ddesc;
            }

            set
            {
                if (value == null || value == String.Empty)
                    ddesc = "";
                else
                    ddesc = value;
            }
        }
        public string[] d_Arguments
        {
            get
            {
                return dargs;
            }

            set
            {
                if (value == null)
                    dargs = new string[0];
                else
                    dargs = value;
            }
        }
        public int ID { get; set; }
        #endregion

        public EditDebt(Debt debt, DataBaseManager @base, Priority[] priorities)
        {
            InitializeComponent();

            dbm = @base;
            p_li = priorities;

            InitializeValues();
            try
            {
                d_User = debt.Debtor;
                debtorName.Text = d_User.Name;

                d_Amount = debt.Amount;
                amount.Value = Convert.ToDecimal(debt.Amount);

                d_Priority = debt.Priority;
                priority.SelectedItem = d_Priority.Name + " " + d_Priority.Label;

                d_Date = debt.CreateDate;
                createDate.Value = d_Date;

                d_Description = debt.Desc;
                desc.Text = d_Description;

                ID = debt.ID;

                d_Arguments = debt.Arguments;

                debt_history = debt.GetHistory();
            }catch(Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InitializeValues()
        {
            priority.Items.AddRange(p_li.Select(t => t.Name + " " + t.Label).ToArray());
            users = dbm.GetDebtors(p_li).ToArray();
            debtorName.Items.AddRange(users.Select(t => t.Name).ToArray());
        }

        public bool ShowForm(out UpdateConstructor update)
        {
            ShowDialog();
            if (!OK) { update = null; return OK; }

            update = new UpdateConstructor();
            update.TableName = "Debts";
            update.Wheres = "`id`='" + ID + "'";
            update.Values.Add(new DBKV("name", d_User.Name));
            update.Values.Add(new DBKV("debt", d_Amount.ToString()));
            update.Values.Add(new DBKV("priority", d_Priority.Name.ToString()));
            update.Values.Add(new DBKV("date", d_Date.ToString()));
            update.Values.Add(new DBKV("desc", d_Description));
            update.Values.Add(new DBKV("arguments", d_Arguments.JoinStr(";")));
            update.Values.Add(new DBKV("history", debt_history));

            return OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sel_username = debtorName.Text;
                if (string.IsNullOrWhiteSpace(sel_username)) throw new Exception("Введите имя должника!");
                User sel_user;
                if (users.Where(t => t.Name == sel_username).Count() == 0)
                {
                    sel_user = new User(0, sel_username, p_li[0], "");
                    dbm.AddUser(sel_user);
                }
                else
                    sel_user = users.First(t => t.Name == sel_username);

                d_User = sel_user;

                d_Amount = Convert.ToInt32(amount.Value);

                d_Priority = p_li.First(t => t.Name == Convert.ToInt32(priority.SelectedItem.ToString().Split(' ')[0]));

                d_Date = createDate.Value;

                d_Description = desc.Text;

                OK = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
