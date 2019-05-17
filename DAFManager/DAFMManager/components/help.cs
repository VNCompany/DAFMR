using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAFManager.components
{
    public partial class help : Form
    {
        public help(string ver)
        {
            InitializeComponent();
            version.Text = ver;
            textBox1.Select(0, 0);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}
