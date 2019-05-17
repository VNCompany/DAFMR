using System;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace DAFManager
{
    public class UpdateManager
    {
        public int LocalVersion = 103;

        public const bool LocalEnabled = false;

        public string URL;
        string INFO;
        public string RootPath { get; set; }
        XDocument doc;
        XElement root;
        public string Log = "";

        public UpdateManager(string rootPath)
        {
            URL = "http://vncompany.ru/dafm/updates";
            INFO = URL + "/info.xml";
            RootPath = rootPath;

        }

        public int GetCurrentVersion()
        {
            if (root != null)
            {
                return Convert.ToInt32(root.Element("currentVersion").Value);
            }
            else
            {
                return -1;
            }
        }

        public bool CheckUpdates(out string url, out string version)
        {
            int web_ver = !LocalEnabled ? GetCurrentVersion() : LocalVersion;
            int prog_ver = Convert.ToInt32(Constants.PROG_VERSION);
            if(web_ver > prog_ver)
            {
                if (root != null)
                {
                    url = root.Element("versions").Elements().First(t => t.Attribute("name").Value == web_ver.ToString()).Attribute("file").Value;
                    version = web_ver.ToString();
                    return true;
                }
                else
                {
                    url = null;
                    version = null;
                    return false;
                }
            }
            else
            {
                url = null;
                version = null;
                return false;
            }
        }

        public bool GetElements()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    using(Stream s = wc.OpenRead(INFO))
                    {
                        using(StreamReader sr = new StreamReader(s))
                        {
                            string Value = sr.ReadToEnd();
                            doc = XDocument.Parse(Value);
                            root = doc.Element("uconfig");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log += "\n" + ex.Message;
                return false;
            }
        }
    }
}
