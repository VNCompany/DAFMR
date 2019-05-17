using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbm_lib.components
{
    public class Debt
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public int Amount { get; set; }
        public User Debtor { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreateDate { get; set; }
        public string[] Arguments { get; set; }
        public List<string> History { get; set; }

        public Debt() { }

        public Debt(object id, object desc, object amount, User debtor, Priority priority, object date, string arguments, string history="")
        {
            Func<object, DateTime> getDefDate = (object dt) =>
            {
                if (dt is string) return DateTime.Parse(dt.ToString());
                if (dt is DateTime) return (DateTime)dt;
                throw new ArgumentException();
            };

            ID = Convert.ToInt32(id);
            Debtor = debtor;
            Priority = priority;
            CreateDate = getDefDate(date);
            Desc = desc.ToString();
            Amount = Convert.ToInt32(amount);

            if (!string.IsNullOrWhiteSpace(arguments) && arguments.Contains(";"))
                Arguments = arguments.Split(';');
            else if (string.IsNullOrWhiteSpace(arguments))
                Arguments = new string[] { arguments };
            else
                Arguments = new string[0];

            if (!string.IsNullOrWhiteSpace(history))
            {
                string[] h = history.Split(';');
                History = new List<string>();
                History.AddRange(h);
            }
            else
            {
                History = new List<string>();
            }
        }

        public string GetHistory()
        {
            if(History.Count > 0)
            {
                if(History.Count == 1)
                {
                    return History[0];
                }
                else
                {
                    return string.Join(";", History);
                }
            }
            else
            {
                return "";
            }
        }
    }
}
