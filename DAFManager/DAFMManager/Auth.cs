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
using DAFManager;

namespace DAFMManager
{
    public partial class Auth : Form
    {
        DataBaseManager dbm;
        public Auth(AuthShowType type = AuthShowType.None)
        {
            InitializeComponent();
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));

            switch (type)
            {
                case AuthShowType.AutoAuth:
                    Close();
                    Application.Run(new Main());
                    break;
                case AuthShowType.OneLogin:
                    dbm = new DataBaseManager();
                    textBox1.Text = dbm.GetLogin();
                    break;
                default:
                    dbm = new DataBaseManager();
                    break;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbm.Login(textBox1.Text, textBox2.Text))
                {
                    new DAFManager.Main(this).Show();
                    dbm.Close();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex)
            {
                DAFManager.Program.Log.WriteLineF("{0} - {1}:{2}:{3}", lm_lib.Log.LogType.ERR, ex.GetType().FullName, ex.Message, "Auth.cs", "27");
                Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Auth_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message msg = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref msg);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Button2_Click(sender, new EventArgs());
            }
        }
    }
}
