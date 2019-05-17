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

namespace DAFMManager
{
    public partial class Auth : Form
    {
        DataBaseManager dbm;
        public Auth()
        {
            InitializeComponent();
            dbm = new DataBaseManager();
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbm.Login(textBox1.Text, textBox2.Text))
                {
                    Hide();
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
    }
}
