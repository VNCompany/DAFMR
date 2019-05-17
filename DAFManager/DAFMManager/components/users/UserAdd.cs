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

namespace DAFManager
{
    public partial class UserAdd : Form
    {
        public enum EditType { Create, Edit }

        DataBaseManager dbm;
        string[] users;
        Priority[] pri;

        public bool OK = false;

        public EditType etype = EditType.Create;
        public EditType EType
        {
            get { return etype; }
            set
            {
                etype = value;
            }
        }

        public string UserName { get; set; }
        public Priority SelectedPriority { get; set; }

        public UserAdd(DataBaseManager baseManager, string[] names)
        {
            InitializeComponent();
            dbm = baseManager;
            users = names;
            pri = dbm.GetPriorities();

            if(pri.Length == 0)
            {
                dbm.AddPriority(new Priority(0, "Обычный", Color.White));
                pri = new Priority[] { new Priority(0, "Обычный", Color.White) };
            }
            priorityname.Items.AddRange((from t in pri select t.Name + " " + t.Label).ToArray());
        }

        public void Construct()
        {
            if (EType == EditType.Create)
                priorityname.SelectedIndex = 0;
            else
            {
                button1.Text = "Изменить";
                Text = "Редактирование задолжника";
                username.Text = UserName;

                string val = SelectedPriority.Name + " " + SelectedPriority.Label;
                int id = 0;
                for (int i = 0; i < priorityname.Items.Count; i++)
                {
                    if (val == priorityname.Items[i].ToString()) { id = i; break; }
                }
                priorityname.SelectedIndex = id;
            }
        }

        public bool ShowForm()
        {
            ShowDialog();
            return OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text)) { MessageBox.Show("Введите имя!"); return; }
            if (users.Contains(username.Text) && EType == EditType.Create) { MessageBox.Show("Такой человек уже существует в базе."); return; }
            if (priorityname.SelectedItem == null) { MessageBox.Show("Выберите приоритет!"); return; }

            UserName = username.Text;
            SelectedPriority = pri.First(t => t.Name == Convert.ToInt32(priorityname.SelectedItem.ToString().Split(' ')[0]));

            OK = true;

            Close();
        }
    }
}
