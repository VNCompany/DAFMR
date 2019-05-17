using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace dbm_lib.components
{
    public class Priority
    {
        public int Name { get; set; }
        public string Label { get; set; }
        public Color Color { get; set; }

        public Priority() { }

        public Priority(object name, object label, Color color)
        {
            Name = Convert.ToInt32(name);
            Color = color;
            Label = label.ToString(); ;
        }

        public Priority(object name, object label, string htmlcolor)
        {
            Color col = ColorTranslator.FromHtml(htmlcolor);
            Name = Convert.ToInt32(name);
            Color = col;
            Label = label.ToString();
        }

        public string GetSpecCode()
        {
            return Name.ToString().ToLower() + Label.ToString().ToLower();
        }
    }
}
