using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace sm_lib
{
    public class SWorker : IDisposable
    {
        public FileInfo path;
        SettingsCollection collection = new SettingsCollection();
        bool act = false;
        public string Path { get; set; }
        public bool Active
        {
            get
            {
                if (path == null || act == false) return false;
                return true;
            }
        }

        public void Open()
        {
            if (!String.IsNullOrEmpty(Path))
            {
                path = new FileInfo(Path);
                if (path.Directory.Exists)
                {
                    act = true;
                    if (path.Exists)
                    {
                        using (StreamReader read = new StreamReader(path.FullName))
                        {
                            string line;
                            while ((line = read.ReadLine()) != null)
                            {
                                if (line.StartsWith("#")) continue;
                                Regex rx = new Regex(@"([^А-Яа-я\s#=]+?)=([^#=]+)");
                                Match mh = rx.Match(line);
                                if (mh.Success)
                                {
                                    collection.Add(mh.Groups[1].Value, mh.Groups[2].Value);
                                }
                            }
                            read.Close();
                        }
                    }
                }
                else
                {
                    throw new Exception($"Директория \"{path.Directory}\" не существует");
                }
            }
            else
            {
                throw new Exception("Пустой путь");
            }
        }

        public SettingsCollection Items
        {
            get
            {
                if (!Active) throw new Exception("Файл настроек не открыт");
                return collection;
            }
        }

        public void Save(string file)
        {
            if (!Active) throw new Exception("Файл настроек не открыт");
            using (StreamWriter sw = new StreamWriter(path.FullName))
            {
                foreach(SettingElement elem in collection)
                {
                    sw.WriteLine(elem.Name + "=" + elem.Value);
                }
                sw.Close();
            }
        }

        public void Save()
        {
            Save(path.FullName);
        }

        public void Dispose()
        {
            collection = null;
            path = null;
            act = false;
            Path = null;
        }
    }

    public class SettingsCollection : IEnumerable<SettingElement>
    {
        List<SettingElement> list = new List<SettingElement>();
        public IEnumerator<SettingElement> GetEnumerator()
        {
            return new SettingsCollectionIEnumerator(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public SettingElement this[int key]
        {
            get { return list[key]; }
            set { list[key] = value; }
        }

        public SettingElement this[string name]
        {
            get
            {
                SettingElement val = list.FirstOrDefault(t => t.Name == name);
                return val;
            }

            set
            {
                int val = list.FindIndex(t => t.Name == name);
                if (val == -1) throw new NullReferenceException();
                list[val] = value;
            }
        }

        public void Add(SettingElement element)
        {
            if (list.Where(t => t.Name == element.Name).Count() > 0)
                throw new Exception("Ключ уже существует.");
            list.Add(element);
        }

        public void Add(object name, object value)
        {
            if(list.Where(t => t.Name == name.ToString()).Count() > 0)
                throw new Exception("Ключ уже существует.");
            list.Add(new SettingElement(name, value));
        }

        public void Delete(string key)
        {
            int index = list.FindIndex(t => t.Name == key);
            if(index != -1)
            {
                list.RemoveAt(index);
            }
        }
    }

    public class SettingElement : ICloneable
    {
        string nm;
        public string Name
        {
            get { return nm; }
            set
            {
                nm = value.Replace(" ", "_");
            }
        }
        public string Value { get; set; }

        public SettingElement() { }
        public SettingElement(object name, object value)
        {
            Name = name.ToString();
            Value = value.ToString();
        }

        public object Clone()
        {
            return new SettingElement(Name, Value);
        }
    }

    public class SettingsCollectionIEnumerator : IEnumerator<SettingElement>
    {
        SettingElement[] elements;
        int pos = -1;

        public SettingsCollectionIEnumerator(IEnumerable<SettingElement> array)
        {
            elements = array.ToArray();
        }

        public SettingElement Current
        {
            get
            {
                if (pos == -1 || pos >= elements.Length)
                    throw new InvalidOperationException();
                return elements[pos];
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if(pos < elements.Length - 1)
            {
                pos++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            pos = -1;
        }

        public void Dispose()
        {
            pos = 0;
            elements = null;
        }
    }
}
