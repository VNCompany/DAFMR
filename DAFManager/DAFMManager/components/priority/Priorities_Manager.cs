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
    public partial class Priorities_Manager : Form
    {
        List<Priority> pri = new List<Priority>();
        DataBaseManager dbm;

        public bool UpdatePriorities = false;

        public Priorities_Manager(Priority[] pr, DataBaseManager db)
        {
            InitializeComponent();
            pri = pr.ToList();
            dbm = db;
            Print();
        }

        public void Print()
        {
            listView1.Items.Clear();

            foreach (Priority p in pri)
            {
                listView1.Items.Add(
                    new ListViewItem(
                        new string[]
                        {
                            p.Name.ToString(),
                            p.Label,
                            ColorTranslator.ToHtml(p.Color)
                        }
                        )
                    );
            }
        }

        private void Priorities_Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            pri = null;
            dbm = null;
        }

        private void СоздатьНовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Priorities_Add pa = new Priorities_Add(pri.ToArray());
            if (pa.ShowForm())
            {
                Priority pr = new Priority(pa.PrName, pa.PrLabel, pa.PrColor);
                pri.Add(pr);
                Print();
                dbm.AddPriority(pr);
                UpdatePriorities = true;
                sync_manager.Synchronization.Changes += 1;
            }
            pa.Dispose();
            pa = null;
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                foreach(ListViewItem item in listView1.SelectedItems)
                {
                    int id = Convert.ToInt32(item.SubItems[0].Text);
                    dbm.DeletePriority(id);
                    pri.RemoveAll(t => t.Name == id);
                    Print();
                    UpdatePriorities = true;
                    sync_manager.Synchronization.Changes += 1;
                }
            }
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                УдалитьToolStripMenuItem_Click(sender, new EventArgs());
            }
        }

        private void ИзменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 1)
            {
                ListViewItem item = listView1.SelectedItems[0];
                UpdateConstructor ic = new UpdateConstructor();
                ic.Wheres = "`name`='" + item.SubItems[0].Text + "'";
                Priorities_Add pa = new Priorities_Add(pri.ToArray());

                pa.SetValues(item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text);
                pa.CheckExistsKey = false;

                if (pa.ShowForm())
                {
                    ic.Values.Add(new DBKV("name", pa.PrName.ToString()));
                    ic.Values.Add(new DBKV("label", pa.PrLabel));
                    ic.Values.Add(new DBKV("color", ColorTranslator.ToHtml(pa.PrColor)));
                    item.SubItems[0].Text = pa.PrName.ToString();
                    item.SubItems[1].Text = pa.PrLabel;
                    item.SubItems[2].Text = ColorTranslator.ToHtml(pa.PrColor);
                    UpdatePriorities = true;
                }
                dbm.EditPriority(ic);
                sync_manager.Synchronization.Changes += 1;
                pa.Dispose();
                pa = null;
            }
        }
    }
}
