using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbm_lib.components
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Priority Priority { get; set; }
        public string[] Arguments { get; set; }

        public User() { }

        public User(int id, string name, Priority priority, string arguments)
        {
            ID = id;
            Name = name;
            Priority = priority;
            Arguments = arguments.Split(';');
        }

        public User Clone()
        {
            return new User(
                ID,
                Name,
                new Priority(Priority.Name, Priority.Label, Priority.Color),
                Arguments.JoinStr(";")
                );
        }
    }

    public class ArgumentsWorker
    {
        List<AMC> arg_li = new List<AMC>();
        public string[] Args
        {
            get { return (from t in arg_li select t.ToString()).ToArray();  }
            set
            {
                arg_li.Clear();
                foreach(string arg in value) arg_li.Add(new AMC(arg));
            }
        }

        public List<AMC> Get
        {
            get { return arg_li; }
        }

        public AMC this[string key]
        {
            get
            {
                return arg_li.Find(t => t.Key == key);
            }

            set
            {
                int f_index = arg_li.FindIndex(t => t.Key == key);
                if (f_index == -1) throw new NullReferenceException();
                arg_li[f_index] = value;
            }
        }

        public void Add(AMC item)
        {
            if (arg_li.Where(t => t.Key == item.Key).Count() == 0)
                arg_li.Add(item);
            else
                throw new InvalidOperationException("Элемент уже существует");
        }

        public void Add(string argument) => Add(new AMC(argument == null ? "" : argument));

        public ArgumentsWorker(string[] args)
        {
            Args = args;
        }

        public ArgumentsWorker(string args)
        {
            if(!string.IsNullOrWhiteSpace(args) && args.Contains(";"))
                Args = args.Split(';');
        }

        public override string ToString()
        {
            return (from t in arg_li select t.ToString()).JoinStr(";");
        }
    }

    public class AMC
    {
        bool amcval = false;
        bool nullable = false;
        public enum AmcType { OneType = 0, KeyVal = 1, Null = 2 }
        public string Key { get; set; }

        public string Value { get; set; }

        public AmcType AType
        {
            get
            {
                if (nullable)
                    return AmcType.Null;
                else
                {
                    return amcval ? AmcType.KeyVal : AmcType.OneType;
                }
            }
        }

        public AMC(string argument)
        {
            if (!string.IsNullOrWhiteSpace(argument))
            {
                if (argument.Contains("="))
                {
                    string[] splitted = argument.Split('=');
                    Key = splitted[0];
                    Value = splitted[1];
                    amcval = true;
                }
                else
                {
                    Key = argument;
                    Value = "";
                    amcval = false;
                }
            }
            else
            {
                nullable = true;
            }
        }

        public override string ToString()
        {
            if (AType == AmcType.Null) return "";
            if(AType == AmcType.OneType)
            {
                return Key;
            }else if(AType == AmcType.KeyVal)
            {
                return Key + "=" + Value;
            }
            else
            {
                return "";
            }
        }
    }
}
