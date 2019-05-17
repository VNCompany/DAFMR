using System;
using System.Windows.Forms;
using dbm_lib;
using lm_lib;
using sm_lib;
using System.IO;
using System.Threading;

namespace DAFManager
{
    public partial class Startup : Form
    {
        bool AllowClosing = true;
        public Startup()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(u_login.Text))
            {
                MessageBox.Show("В отличии от пароля, логин должен быть обязательно введён!");
                return;
            }

            try
            {
                THStart_Closed += OnClosed;
                AllowClosing = false;

                progressBar1.Show();
                label5.Show();
                statusText.Show();

                Thread th = new Thread(THStart);
                th.Start();
            }catch(Exception ex)
            {
                Main.GetExceptionMessage(ex, "Startup.cs:25");
            }
        }

        private void THStart()
        {
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Инициализация."; progressBar1.Value = 10; }));
            Thread.Sleep(2000);
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Инициализация. Сканирование системы..."; progressBar1.Value = 15; }));
            Thread.Sleep(4000);
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Инициализация. Получение информации..."; progressBar1.Value = 25; }));
            Thread.Sleep(8000);
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Установка настроек. Установка базы данных..."; progressBar1.Value = 50; }));
            DataBaseManager dbm = new DataBaseManager();
            Thread.Sleep(3000);
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Установка базы данных. Создание необходимых таблиц..."; progressBar1.Value = 60; }));
            dbm.CheckTables();
            Thread.Sleep(4000);
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Установка базы данных. Регистрация администратора..."; progressBar1.Value = 70; }));
            string login = "";
            string pw = "";
            Invoke((MethodInvoker)(() =>
            {
                login = u_login.Text;
                pw = u_pass.Text;
            }));
            dbm.AdminAddOrUpdate(login, pw);
            Thread.Sleep(2000);

            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Установка базы данных. Установка стандартных приоритетов..."; progressBar1.Value = 80; }));
            if(MessageBox.Show("Установить стандартные приоритеты?",
                "Установка приоритетов", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                dbm.SetDefPriorities();
                Thread.Sleep(3000);
            }
            statusText.Invoke((MethodInvoker)(() => { statusText.Text = "Установка настроек..."; progressBar1.Value = 98; }));

            if (File.Exists(Constants.PROG_DIR + "\\" + "startup"))
            {
                File.Delete(Constants.PROG_DIR + "\\" + "startup");
            }

            if (Directory.Exists(Constants.SETTINGS_PATH))
            {
                Directory.Delete(Constants.SETTINGS_PATH, true);
                Thread.Sleep(20);
            }

            Directory.CreateDirectory(Constants.SETTINGS_PATH);

            SWorker worker = new SWorker();
            worker.Path = Constants.SETTINGS_PATH + "\\" + "config.ini";
            worker.Open();
            worker.Items.Add("program_path", Path.GetDirectoryName(Application.ExecutablePath));
            worker.Save();

            Thread.Sleep(8000);


            THStart_Closed?.Invoke(this, new EventArgs());
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() =>
            {
                MessageBox.Show("Установка начальных параметров завершена. Программа автоматически перезагрузится.");
                AllowClosing = true;

                Thread th = new Thread(() =>
                {
                    Thread.Sleep(3000);
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                });
                th.Start();
                Close();
            }));
        }

        public event EventHandler THStart_Closed;

        private void Startup_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !AllowClosing;
        }
    }
}
