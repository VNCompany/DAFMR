using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAFManager
{
    public partial class EditPass : Form
    {
        public string Old_Pass
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
        public string New_Pass
        {
            get
            {
                return textBox2.Text;
            }
            set
            {
                textBox2.Text = value;
            }
        }

        bool OK = false;
        public EditPass()
        {
            InitializeComponent();
        }

        public bool ShowForm(out string old_pass, out string new_pass)
        {
            ShowDialog();
            if (!OK) { old_pass = null; new_pass = null; return OK; }

            old_pass = Old_Pass;
            new_pass = New_Pass;

            return OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Old_Pass == null) { MessageBox.Show("Введите старый пароль!"); return; }
            if (New_Pass == null) { MessageBox.Show("Введите новый пароль!"); return; }

            OK = true;

            Close();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Button1_Click(sender, new EventArgs());
        }
    }
}
