using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAFManager
{
    public partial class ChangesDialog : Form
    {
        public ChangesDialog(string html)
        {
            InitializeComponent();
            webBrowser1.DocumentText = html;
        }

        private void ChangesDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            webBrowser1.Dispose();
        }
    }
}
