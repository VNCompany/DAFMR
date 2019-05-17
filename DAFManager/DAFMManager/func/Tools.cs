using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DAFManager
{
    public static class Tools
    {
        public static Color GetForeColorFromThis(Color color)
        {
            string th16 = ColorTranslator.ToHtml(color).ToLower();
            if(th16 == "red" || th16 == "gray" || th16 == "green" || th16 == "blue" ||
               th16 == "darkblue" || th16 == "gold")
            {
                return Color.White;
            }
            else
            {
                return Color.Black;
            }
        }
    }
}
