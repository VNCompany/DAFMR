using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbm_lib;
using dbm_lib.components;

namespace DAFManager
{
    public partial class Priorities_Add : Form
    {
        public bool OK = false;

        Priority[] Priorities;

        public bool CheckExistsKey = true;

        public int PrName { get; private set; }
        public string PrLabel { get; private set; }
        public Color PrColor { get; private set; }

        public void SetValues(object name, object label, object color)
        {
            if (color is Color) panel1.BackColor = (Color)color;
            if (color is string) panel1.BackColor = ColorTranslator.FromHtml(color.ToString());
            n_key.Value = Convert.ToDecimal(name);
            n_label.Text = label.ToString();

            button1.Text = "Изменить";
            Text = "Редактирование приоритета";
        }

        public Priorities_Add(Priority[] pr)
        {
            InitializeComponent();
            Priorities = pr;

            if(Priorities.Length > 0)
            {
                int fst = Priorities.OrderByDescending(t => t.Name).First().Name;
                if (fst < 99)
                {
                    n_key.Value = fst + 1;
                }
            }
        }

        private bool Key_Exists(int name)
        {
            return (from t in Priorities where t.Name == name select t).Count() > 0;
        }

        private bool Key_Exists(string label)
        {
            return (from t in Priorities where t.Label == label select t).Count() > 0;
        }

        private void Panel1_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(n_label.Text)) { MessageBox.Show("Введите название приоритета!"); return; }
            if(Key_Exists(Convert.ToInt32(n_key.Value)) && CheckExistsKey) { MessageBox.Show("Приоритет с таким ключом уже существует."); return; }
            if(Key_Exists(n_label.Text) && CheckExistsKey) { MessageBox.Show("Приоритет с таким названием уже существует."); return; }

            PrName = Convert.ToInt32(n_key.Value);
            PrLabel = n_label.Text;
            PrColor = panel1.BackColor;

            OK = true;
            Close();
        }

        public bool ShowForm()
        {
            ShowDialog();
            return OK;
        }
    }
}
