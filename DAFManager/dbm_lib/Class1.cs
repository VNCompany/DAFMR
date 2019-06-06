using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Security.Cryptography;
using dbm_lib.components;

namespace dbm_lib
{
    public class DataBaseManager
    {
        SQLiteConnection lite;
        public bool isBusy = false;

        public DataBaseManager()
        {
            lite = new SQLiteConnection("Data Source=base.db;Version=3");
            lite.Open();
            isBusy = true;
        }

        public void Open()
        {
            if (!isBusy)
            {
                lite.Open();
                isBusy = true;
            }
        }

        public void CheckTables()
        {
            using(SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Settings`(`key` VARCHAR(32), `value` VARCHAR(64))";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Users`(`id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `name` TEXT, `priority` INT(2) DEFAULT '0', `arguments` TEXT)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Priorities`(`id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `name` INT(2) NOT NULL, `label` VARCHAR(64), `color` VARCHAR(64))";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS `Debts`(`id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                                                     "`name` TEXT," +
                                                                     "`debt` INT DEFAULT '0'," +
                                                                     "`priority` INT(2) DEFAULT '0'," +
                                                                     "`date` TEXT," +
                                                                     "`desc` TEXT," +
                                                                     "`arguments` TEXT," +
                                                                     "`history` TEXT DEFAULT '')";
                cmd.ExecuteNonQuery();
            }
        }

        public void AddColumn(string columnName, string columnType, string table="Debts")
        {
            SQLITECMD_NONE($"ALTER TABLE `{table}` ADD COLUMN `{columnName}` {columnType}");
        }

        public void SetDefPriorities()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "INSERT INTO `Priorities`(`name`, `label`, `color`) VALUES ('0', 'Обычный', 'white');" +
                                  "INSERT INTO `Priorities`(`name`, `label`, `color`) VALUES ('1', 'Нормальный', 'darkblue');" +
                                  "INSERT INTO `Priorities`(`name`, `label`, `color`) VALUES ('2', 'Средний', 'darkgoldenrod');" +
                                  "INSERT INTO `Priorities`(`name`, `label`, `color`) VALUES ('3', 'Важный', 'red');";
                cmd.ExecuteNonQuery();
            }
        }

        public void AdminAddOrUpdate(string login, string password)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM `Settings` WHERE `key`='login' OR `key`='password'";
                if(Convert.ToInt32(cmd.ExecuteScalar()) == 2)
                {
                    cmd.CommandText = $"UPDATE `Settings` SET `value`='{login}' WHERE `key`='login';UPDATE `Settings` SET `value`='{MD5_Hash(password)}' WHERE `key`='password';";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = $"INSERT INTO `Settings`(`key`,`value`) VALUES ('login', '{login}');" +
                                      $"INSERT INTO `Settings`(`key`,`value`) VALUES ('password', '{MD5_Hash(password)}');";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLogin(string new_login)
        {
            SQLITECMD_NONE($"UPDATE `Settings` SET `value`='{new_login}' WHERE `key`='login'");
        }
        public bool UpdatePass(string old_pass, string new_pass)
        {
            string pw_md = SQLITECMD_OBJ("SELECT `value` FROM `Settings` WHERE `key`='password'").ToString();
            if(MD5_Hash(old_pass) == pw_md)
            {
                SQLITECMD_NONE($"UPDATE `Settings` SET `value`='{MD5_Hash(new_pass)}' WHERE `key`='password'");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Login(string login, string password)
        {
            using(SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM `Settings` WHERE `key`='login' AND `value`='{login}'";
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if(count > 0)
                {
                    cmd.CommandText = "SELECT `value` FROM `Settings` WHERE `key`='password'";
                    string pw = cmd.ExecuteScalar().ToString();
                    if (pw == MD5_Hash(password))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public string GetLogin()
        {
            return SQLITECMD_OBJ("SELECT `value` FROM `Settings` WHERE `key`='login'").ToString();
        }

        public int GetSumAmounts(string condition="")
        {
            string val = SQLITECMD_OBJ("SELECT SUM(Debt) FROM `Debts` " + condition).ToString();
            int sum = val != string.Empty ? Convert.ToInt32(val) : 0;
            return sum;
        }

        public void SQLITECMD_NONE(string commandString)
        {
            using(SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = commandString;
                cmd.ExecuteNonQuery();
            }
        }

        public object SQLITECMD_OBJ(string commandString)
        {
            using(SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = commandString;
                return cmd.ExecuteScalar();
            }
        }

        #region ************************ Работа с долгами ************************

        public void AddDebt(Debt debt)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = string.Format("INSERT INTO `Debts`(`name`,`debt`,`priority`,`date`,`desc`,`arguments`,`history`) VALUES" +
                                                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                debt.Debtor.Name.Replace("'","\\'"),
                                                debt.Amount,
                                                debt.Priority.Name,
                                                debt.CreateDate.ToString(),
                                                debt.Desc.Replace("'", "\\'"),
                                                new ArgumentsWorker(debt.Arguments).ToString().Replace("'","\\'"),
                                                debt.GetHistory());
                cmd.ExecuteNonQuery();
            }
        }

        public Debt[] GetDebts(Priority[] priorities)
        {
            List<Debt> debts = new List<Debt>();
            User[] users = GetDebtors(priorities);

            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "SELECT * FROM `Debts` ORDER BY datetime(date) DESC";
                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        var cp = priorities.FirstOrDefault(t => t.Name == Convert.ToInt32(sdr["priority"]));
                        debts.Add(new Debt(
                            sdr["id"],
                            sdr["desc"],
                            sdr["debt"],
                            users.First(t => t.Name == sdr["name"].ToString()),
                            cp != null ? cp : new Priority(0, "NONE", System.Drawing.Color.White),
                            sdr["date"].ToString(),
                            sdr["arguments"].ToString(),
                            sdr["history"].ToString()
                            ));
                    }
                }
            }

            GC.Collect();
            return debts.ToArray();
        }

        public void DeleteDebt(int id)
        {
            SQLITECMD_NONE($"DELETE FROM `Debts` WHERE `id`='{id}'");
        }

        public void DeleteDebts(int[] ids)
        {
            string[] commands = new string[ids.Length];
            for (int i = 0; i < commands.Length; i++) commands[i] = $"DELETE FROM `Debts` WHERE `id`='{ids[i]}'";
            SQLITECMD_NONE(commands.JoinStr(";"));
        }
        
        public void EditDebt(UpdateConstructor update)
        {
            SQLITECMD_NONE(update.ToString());
        }

        #endregion


        #region ************************ Работа с задолжниками ************************

        public User[] GetDebtors(Priority[] priorities)
        {

            List<User> users = new List<User>();

            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "SELECT * FROM `Users` ORDER BY `id` DESC";
                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        users.Add(new User(
                            Convert.ToInt32(sdr["id"]),
                            sdr["name"].ToString(),
                            priorities.First(t => t.Name == Convert.ToInt32(sdr["priority"])),
                            sdr["arguments"].ToString()
                            ));
                    }
                }
            }

            return users.ToArray();
        }

        public void AddUser(User user)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = string.Format("INSERT INTO `Users`(`name`,`priority`,`arguments`) VALUES" +
                                                "('{0}','{1}','{2}')",
                                                user.Name,
                                                user.Priority.Name,
                                                new ArgumentsWorker(user.Arguments).ToString());
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            string username = SQLITECMD_OBJ($"SELECT `name` FROM `Users` WHERE `id`='{id}'").ToString();
            SQLITECMD_NONE($"DELETE FROM `Users` WHERE `id`='{id}'");
            SQLITECMD_NONE($"DELETE FROM `Debts` WHERE `name`='{username}'");
        }

        public void EditUser(UpdateConstructor update)
        {
            update.TableName = "Users";
            SQLITECMD_NONE(update.ToString());
        }

        public bool UserExists(string user)
        {
            return Convert.ToInt32(SQLITECMD_OBJ($"SELECT COUNT(*) FROM `Users` WHERE `name`='{user}'")) > 0;
        }

        #endregion

        #region ************************ Работа с приоритетами ************************

        public void AddPriority(Priority priority)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = string.Format("INSERT INTO `Priorities`(`name`,`label`,`color`) VALUES" +
                                                "('{0}','{1}','{2}')",
                                                priority.Name,
                                                priority.Label,
                                                System.Drawing.ColorTranslator.ToHtml(priority.Color));
                cmd.ExecuteNonQuery();
            }
        }

        public Priority[] GetPriorities()
        {
            List<Priority> priorities = new List<Priority>();

            using (SQLiteCommand cmd = new SQLiteCommand(lite))
            {
                cmd.CommandText = "SELECT * FROM `Priorities` ORDER BY `name` ASC";
                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        priorities.Add(new Priority(sdr["name"], sdr["label"], sdr["color"].ToString()));
                    }
                }
            }

            return priorities.ToArray();
        }

        public void DeletePriority(int id)
        {
            SQLITECMD_NONE($"DELETE FROM `Priorities` WHERE `name`='{id}'");
        }

        public void EditPriority(UpdateConstructor insert)
        {
            insert.TableName = "Priorities";
            SQLITECMD_NONE(insert.ToString());
        }

        

        #endregion


        public enum GetCount_Bases { Debts, Users, Priorities }
        public int GetCount(GetCount_Bases @base)
        {
            return Convert.ToInt32(SQLITECMD_OBJ($"SELECT COUNT(*) FROM `{@base.ToString()}`"));
        }


        public void Close()
        {
            lite.Close();
            isBusy = false;
        }
        
        public static string MD5_Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
