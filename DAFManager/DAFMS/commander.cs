using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using sm_lib;
using System.Threading;

namespace DAFMS
{
    class Commander
    {
        string SETTINGS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VNCompany", "DAFM");

        public string CmdWorker(string cmd_string)
        {
            //if (string.IsNullOrWhiteSpace(cmd_string)) return "Неверная или пустая команда";
            //string[] args;
            //string cmd;
            //if (!cmd_string.Contains(' '))
            //{
            //    args = new string[0];
            //    cmd = cmd_string;
            //}
            //else
            //{
            //    string[] cmd_split = cmd_string.Split(' ');
            //    if (string.IsNullOrWhiteSpace(cmd_split[0])) return "Неверная или пустая команда";
            //    cmd = cmd_split[0];
            //    args = new string[cmd_split.Length - 1];
            //    for (int i = 1, j = 0; i < cmd_split.Length; i++, j++)
            //    {
            //        args[j] = cmd_split[i];
            //    }
            //}

            switch (cmd_string)
            {
                case "!sync":
                    return Cmd_Sync();
                case "!close":
                    return "closed";
                case "!sync_get":
                    return Cmd_Sync2();
                default:
                    return "Неверная или пустая команда";
            }
        }

        public string Cmd_Sync()
        {
            try
            {
                SWorker sw = new SWorker();
                if (!File.Exists(Path.Combine(SETTINGS_PATH, "config.ini"))) return "Ошибка отправки. Отсутствует файл настроек. " + Path.Combine(SETTINGS_PATH, "config.ini");
                sw.Path = Path.Combine(SETTINGS_PATH, "config.ini");
                sw.Open();
                var set = sw.Items;

                if (set["program_path"] == null && !Directory.Exists(set["program_path"].Value)) return "Ошибка отправки. Не задан или ошибочный путь к папке с программой.";
                string prog_path = set["program_path"].Value;
                if (set["sync_path"] == null && !Directory.Exists(set["sync_path"].Value)) return "Ошибка отправки. Не задан или ошибочный путь к папке синхронизации.";
                string sync_path = set["sync_path"].Value;

                string local_base = Path.Combine(prog_path, "base.db");
                string shared_base = Path.Combine(sync_path, "base.db");

                if (File.Exists(local_base))
                {
                    if (File.Exists(shared_base))
                    {
                        File.Delete(shared_base);
                        Thread.Sleep(1000);
                    }

                    File.Copy(local_base, shared_base);
                    Thread.Sleep(2000);
                }
                else
                {
                    return local_base + " + " + shared_base;
                }
                return "Отправка прошла успешно.";
            }
            catch(Exception ex)
            {
                return $"Ошибка отправки. {ex.Message}";
            }
        }

        public string Cmd_Sync2()
        {
            try
            {
                SWorker sw = new SWorker();
                if (!File.Exists(Path.Combine(SETTINGS_PATH, "config.ini"))) return "Ошибка загрузки. Отсутствует файл настроек. " + Path.Combine(SETTINGS_PATH, "config.ini");
                sw.Path = Path.Combine(SETTINGS_PATH, "config.ini");
                sw.Open();
                var set = sw.Items;

                if (set["program_path"] == null && !Directory.Exists(set["program_path"].Value)) return "Ошибка отправки. Не задан или ошибочный путь к папке с программой.";
                string prog_path = set["program_path"].Value;
                if (set["sync_path"] == null && !Directory.Exists(set["sync_path"].Value)) return "Ошибка отправки. Не задан или ошибочный путь к папке синхронизации.";
                string sync_path = set["sync_path"].Value;

                string local_base = Path.Combine(prog_path, "base.db");
                string shared_base = Path.Combine(sync_path, "base.db");

                if (!Directory.Exists(Path.Combine(SETTINGS_PATH, "recovery"))) Directory.CreateDirectory(Path.Combine(SETTINGS_PATH, "recovery"));

                if (File.Exists(shared_base))
                {
                    if (File.Exists(local_base))
                    {
                        string local_rec_base = Path.Combine(SETTINGS_PATH, "recovery", "base_old.db");
                        if (File.Exists(local_rec_base))
                        {
                            File.Delete(local_rec_base);
                            Thread.Sleep(1000);
                        }
                        File.Copy(local_base, local_rec_base);
                        Thread.Sleep(1000);
                        File.Delete(local_base);
                    }
                    File.Copy(shared_base, local_base);
                }

                return "Загрузка прошла успешно.";
            }
            catch (Exception ex)
            {
                return $"Ошибка загрузки. {ex.Message}";
            }
        }
    }
}
