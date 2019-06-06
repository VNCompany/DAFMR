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
using System.IO;

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

            if(Set.Items["sync_enabled"] != null)
            {
                checkBox2.Checked = Convert.ToBoolean(Set.Items["sync_enabled"].Value);
                panel1.Enabled = checkBox2.Checked;
                if (Set.Items["sync_path"] != null)
                {
                    sync_path.Text = Set.Items["sync_path"].Value;
                    if(Directory.Exists(sync_path.Text))
                        folderBrowserDialog1.SelectedPath = sync_path.Text;
                }
            }

            if (Set.Items["auth_type"] != null)
            {
                switch (Set.Items["auth_type"].Value)
                {
                    case "0":
                        auth_standart.Checked = true;
                        break;
                    case "1":
                        auth_pass_only.Checked = true;
                        break;
                    case "2":
                        auth_auto.Checked = true;
                        break;
                    default:
                        auth_standart.Checked = true;
                        break;
                }
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
                    sync_manager.Synchronization.Changes += 1;
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
                        sync_manager.Synchronization.Changes += 1;
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
            if (checkBox2.Checked)
            {
                if(Set.Items["sync_path"] == null || !Directory.Exists(Set.Items["sync_path"].Value))
                {
                    MessageBox.Show("При включенной синхронизации путь до общей папки обязательно должен быть введён!");
                    return;
                }
            }

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

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(Set.Items["sync_enabled"] != null)
            {
                Set.Items["sync_enabled"].Value = checkBox2.Checked.ToString();
            }
            else
            {
                Set.Items.Add("sync_enabled", checkBox2.Checked);
            }

            panel1.Enabled = checkBox2.Checked;
            ch();
        }

        private void Sp_ob_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                sync_path.Text = folderBrowserDialog1.SelectedPath;
                Sync_path_TextChanged(sender, e);
                ch();
            }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (Set.Items["sync_path"] == null || !Directory.Exists(Set.Items["sync_path"].Value))
                {
                    MessageBox.Show("При включенной синхронизации путь до общей папки обязательно должен быть введён!");
                    e.Cancel = true;
                }
            }
        }

        private void Sync_path_TextChanged(object sender, EventArgs e)
        {
            if(Set.Items["sync_path"] != null)
            {
                Set.Items["sync_path"].Value = sync_path.Text;
            }
            else
            {
                Set.Items.Add("sync_path", sync_path.Text);
            }
        }

        private void Auth_standart_CheckedChanged(object sender, EventArgs e)
        {
            ch();
            switch ((sender as Control).Name)
            {
                case "auth_standart":
                    Set.Items["auth_type"].Value = "0";
                    break;
                case "auth_pass_only":
                    Set.Items["auth_type"].Value = "1";
                    break;
                case "auth_auto":
                    Set.Items["auth_type"].Value = "2";
                    break;
            }
        }
    }
}
