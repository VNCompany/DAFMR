using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAFManager
{
    [Serializable]
    class SaveWindowProperties
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        public bool HideZeroDebt { get; set; }


        public SaveWindowProperties() { }

        public SaveWindowProperties(int w, int h, int x, int y, bool hzd)
        {
            Width = w;
            Height = h;
            LocationX = x;
            LocationY = y;
            HideZeroDebt = hzd;
        }
    }
}
