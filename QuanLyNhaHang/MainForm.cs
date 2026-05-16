using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void labelName_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void watdogdoinStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void foodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuanLyMonAn fMonAn = new frmQuanLyMonAn();
            fMonAn.Show();
        }
        private void OPenChildForm(Form childForm)
        {
           
        }
    }
}
