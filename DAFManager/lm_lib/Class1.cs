using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace lm_lib
{
    public class Log
    {
        string log_file;
        public enum LogType { ERR, INFO, WARN, SUC }

        public Log(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("Строка path была null. LogManager");

            if (!Directory.Exists(Path.GetDirectoryName(path)) && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));

            log_file = path;
        }

        public void WriteLine(object text, LogType type=LogType.ERR)
        {
            string txt = text.ToString().Replace("\n", " ").Replace("\t", " ").Replace("\r"," ");
            string result = string.Format("[{0}][{1}]: {2}", DateTime.Now.ToString(), type.ToString(), txt);

            string old_val = "";

            if (File.Exists(log_file))
            {
                using (StreamReader read = new StreamReader(log_file))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        line = line.Replace("\n", " ");
                        old_val += line + "\n";
                    }
                }
            }

            using(StreamWriter write = new StreamWriter(log_file))
            {
                write.Write(old_val + result);
            }
        }

        public void WriteLineF(string text, LogType type, params object[] args)
        {
            WriteLine(string.Format(text, args), type);
        }

        public void WriteList(IEnumerable<WriteLineElement> writeLines)
        {
            string old_val = "";

            if (File.Exists(log_file))
            {
                using (StreamReader read = new StreamReader(log_file))
                {
                    string line;
                    while ((line = read.ReadLine()) != null)
                    {
                        line = line.Replace("\n", " ");
                        old_val += line + Environment.NewLine;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach(WriteLineElement elem in writeLines)
            {
                string txt = elem.Text.ToString().Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
                string result = string.Format("[{0}][{1}]: {2}", DateTime.Now.ToString(), elem.Type.ToString(), txt);

                sb.AppendLine(result);
            }

            using (StreamWriter write = new StreamWriter(log_file))
            {
                write.Write(old_val + sb.ToString());
            }
        }

        public class WriteLineElement
        {
            public string Text { get; set; }
            public LogType Type { get; set; }
            public WriteLineElement() { }
            public WriteLineElement(string text, LogType type)
            {
                Text = text;
                Type = type;
            }
        }
    }
}
