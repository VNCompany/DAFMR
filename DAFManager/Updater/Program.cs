using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3 || !args.Contains("/up")) return;
                string version = args[1];
                string url = args[2];

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Установка обновления. Новая версия: {version}. \n(c)2019 VNCompany / Виктор Незнанов.");
                Console.ForegroundColor = ConsoleColor.Green;
                Thread.Sleep(1000);
                Console.WriteLine("Подготовка к установке новой версии...");
                Thread.Sleep(3000);
                Console.WriteLine("Получение информации...");
                Thread.Sleep(1500);
                Console.WriteLine("Информация получена. Загрузка данных с сервера...");

                Console.ForegroundColor = ConsoleColor.DarkGreen;

                string temp_path = Path.Combine(Path.GetTempPath(), "dafm_update_data");
                if (Directory.Exists(temp_path)) Directory.Delete(temp_path, true);
                Directory.CreateDirectory(temp_path);

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(url), Path.Combine(temp_path, "data.zip"));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Thread.Sleep(2000);
                Console.WriteLine("Загрузка файлов завершена. Распаковка...");
                ZipFile.ExtractToDirectory(temp_path + "\\data.zip", temp_path);
                Thread.Sleep(2000);
                Console.WriteLine("Распаковка завершена. Удаление архива...");
                File.Delete(temp_path + "\\data.zip");
                Thread.Sleep(1000);
                Console.WriteLine("Подготовка к установке новой версии...");

                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("DAFManager");
                foreach (System.Diagnostics.Process proc in ps)
                {
                    proc.Kill();
                }

                if (File.Exists(temp_path + "\\delete.txt"))
                {
                    string[] delete = File.ReadAllLines(temp_path + "\\delete.txt");
                    File.Delete(temp_path + "\\delete.txt");

                    foreach (string file in delete)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            Console.WriteLine("Удаление файла: " + file + "...");
                        }
                    }
                }
                Thread.Sleep(3000);

                foreach (string file in Directory.GetFiles(temp_path))
                {
                    try
                    {
                        if (File.Exists(Path.GetFileName(file))) File.Delete(Path.GetFileName(file));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    Console.WriteLine("Копирование: " + Path.GetFileName(file));
                    Thread.Sleep(10);
                    File.Copy(file, Path.GetFileName(file));
                }

                foreach (string dir in Directory.GetDirectories(temp_path))
                {
                    CopyDir(dir, Path.GetFileName(dir));
                }
                Thread.Sleep(4000);
                Console.WriteLine("Копирование завершено!");

                Thread.Sleep(500);

                Console.WriteLine("Завершение установки...");
                Directory.Delete(temp_path, true);

                Thread.Sleep(2000);

                if(File.Exists("DAFManager.exe"))
                    System.Diagnostics.Process.Start("DAFManager.exe", "dafm_admin /show /non-auth");
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType().FullName, ex.Message);
                File.WriteAllText("fail.txt", $"{args[1]}:{args[2]}");
                Console.ReadKey();
            }
        }

        static void CopyDir(string old_dir, string new_dir)
        {
            if (Directory.Exists(new_dir)) Directory.Delete(new_dir, true);
            Directory.CreateDirectory(new_dir);

            foreach(string file in Directory.GetFiles(old_dir))
            {
                Console.WriteLine("Копирование: " + Path.GetFileName(file));
                Thread.Sleep(4);
                File.Copy(file, Path.Combine(new_dir, Path.GetFileName(file)));
            }

            foreach(string dir in Directory.GetDirectories(old_dir))
            {
                CopyDir(dir, Path.Combine(new_dir, Path.GetFileName(dir)));
            }
        }
    }

    public static class CLS
    {
        public static string SymJoin(this string th, int count)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                res.Append(th);
            }
            return res.ToString();
        }
    }
}
