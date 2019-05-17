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
    public partial class ResDebt : Form
    {
        int PrevSum = 0;
        bool OK = false;
        public ResDebt(int prevSum)
        {
            InitializeComponent();
            PrevSum = prevSum;
        }

        public bool HistoryEnabled
        {
            get { return setHistory.Checked; }
        }

        public int AmountSum
        {
            get
            {
                return Convert.ToInt32(amountSum.Value);
            }
        }

        public int OldSum
        {
            get
            {
                return PrevSum;
            }
        }

        public DateTime CreateDate
        {
            get
            {
                return createDate.Value;
            }
        }

        public string Description
        {
            get
            {
                return desc.Text.Replace(";", ".").Replace("^","_");
            }
        }

        private void SetHistory_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = setHistory.Checked;
        }

        public bool ShowForm()
        {
            ShowDialog();
            return OK;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(PrevSum == 0)
            {
                MessageBox.Show("Должник уже выплатил весь долг.");
                return;
            }else if(PrevSum < AmountSum)
            {
                MessageBox.Show("Сумма выплаты не должна превышать существующую.");
                return;
            }
            else
            {
                OK = true;
                Close();
            }
        }
    }
}
