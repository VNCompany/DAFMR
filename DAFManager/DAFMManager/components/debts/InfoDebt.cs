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
    public partial class InfoDebt : Form
    {
        public InfoDebt(Debt debt)
        {
            InitializeComponent();

            amount.Text = debt.Amount.ToString() + GetCurrency();
            username.Text = debt.Debtor.Name;
            date.Text = debt.CreateDate.ToLongDateString();
            desc.Text = !string.IsNullOrWhiteSpace(debt.Desc) ? debt.Desc : "без описания";
            foreach(string val in debt.History)
            {
                string[] vals = val.Split('^');
                history.Items.Add(new ListViewItem(new string[] { vals[1], vals[0] + GetCurrency(), vals[2] }));
            }
        }

        public string GetCurrency()
        {
            if (Program.Settings.Items["Currency"] != null) return " " + Program.Settings.Items["Currency"].Value;

            return " р.";
        }
    }
}
