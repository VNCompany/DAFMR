using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAFManager.components.admin.sub_comp;
using sm_lib;
using dbm_lib;

namespace DAFManager
{
    public partial class Settings : Form
    {
        DataBaseManager dbm;

        sm_lib.SWorker Set;
        enum ch_type { config, database, file }

        bool cfg_ch = false;

        public Settings(DataBaseManager @base)
        {
            InitializeComponent();
            Set = Program.Settings;
            InitializeValues();
            dbm = @base;
        }

        private void InitializeValues()
        {
            if (Set.Items["close_exit"] == null) Set.Items.Add("close_exit", "False");
            checkBox1.Checked = Set.Items["close_exit"] == null ? false : Convert.ToBoolean(Set.Items["close_exit"].Value);

            if (Set.Items["auto_update"] == null || Set.Items["auto_update"].Value.ToLower() == "true")
            {
                autoupdateCheck.Checked = true;
                autoupdateInterval.Value = Constants.UPDATE_CHECK_INTERVAL / (1000 * 60 * 60);

                if (Set.Items["auto_update"] == null) Set.Items.Add("auto_update", "True");
                else
                    autoupdateCheck.Checked = Convert.ToBoolean(Set.Items["auto_update"].Value);
                if (Set.Items["auto_update_interval"] == null) Set.Items.Add("auto_update_interval", "2");
                else
                    autoupdateInterval.Value = Convert.ToDecimal(Set.Items["auto_update_interval"].Value);
            }
            else
            {
                autoupdateCheck.Checked = Convert.ToBoolean(Set.Items["auto_update"].Value);
                autoupdateInterval.Enabled = autoupdateCheck.Checked;
                if (Set.Items["auto_update_interval"] == null) Set.Items.Add("auto_update_interval", "2");
                else
                    autoupdateInterval.Value = Convert.ToDecimal(Set.Items["auto_update_interval"].Value);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using(EditLogin el = new EditLogin())
            {
                if(el.ShowForm(out string login))
                {
                    dbm.UpdateLogin(login);
                    MessageBox.Show("Логин успешно изменён!");
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            using (EditPass el = new EditPass())
            {
                if (el.ShowForm(out string old_p, out string new_p))
                {
                    if(dbm.UpdatePass(old_p, new_p))
                    {
                        MessageBox.Show("Пароль успешно изменён!");
                    }
                    else
                    {
                        MessageBox.Show("Введённый вами старый пароль не соответствует валидному!");
                    }
                }
            }
        }

        private void ch(ch_type type = ch_type.config)
        {
            switch (type)
            {
                case ch_type.config:
                    if (!cfg_ch)
                    {
                        cfg_ch = true;
                        save.Enabled = true;
                    }
                    break;
                case ch_type.database:
                    // TODO
                    break;
                case ch_type.file:
                    // TODO
                    break;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (cfg_ch) Set.Save();
            Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(Set.Items["close_exit"] == null)
            {
                Set.Items.Add("close_exit", checkBox1.Checked.ToString());
            }
            else
            {
                Set.Items["close_exit"].Value = checkBox1.Checked.ToString();
            }
            ch();
        }

        private void AutoupdateCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoupdateInterval.Enabled = autoupdateCheck.Checked;
            Set.Items["auto_update"].Value = (autoupdateCheck.Checked.ToString() == "True").ToString();
            ch();
        }

        private void AutoupdateInterval_ValueChanged(object sender, EventArgs e)
        {
            Set.Items["auto_update_interval"].Value = autoupdateInterval.Value.ToString();
            ch();
        }
    }
}
