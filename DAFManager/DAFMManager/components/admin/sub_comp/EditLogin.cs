using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAFManager.components.admin.sub_comp
{
    public partial class EditLogin : Form
    {
        public string Login
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        bool OK = false;
        public EditLogin()
        {
            InitializeComponent();
        }

        public bool ShowForm(out string new_login)
        {
            ShowDialog();
            if (!OK) { new_login = null; return OK; }

            new_login = Login;

            return OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login)) { MessageBox.Show("Введите логин!"); return; }

            OK = true;

            Close();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Button1_Click(sender, new EventArgs());
        }
    }
}
