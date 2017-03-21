using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using databaseconnection.DataAccess;
using databaseconnection.Entities;
using databaseconnection.Framework;
using MetroFramework;


namespace Student_side
{
    
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            attendance_system_data asd = new attendance_system_data();
            metroGrid1.DataSource = asd.returnavailablefacultylist();
            asd.AutoUpdateBackTime();
        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            attendance_system_data asd = new attendance_system_data();
            metroGrid1.DataSource = asd.searchbox(metroTextBox1.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void metroGrid1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void metroGrid1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow selectedrow=null;
            if(metroGrid1.SelectedRows.Count > 0)
            {
                selectedrow=metroGrid1.SelectedRows[0];
            }
            if(selectedrow == null)
                return;

            string x= selectedrow.Cells["first_name"].Value.ToString();
            string y= selectedrow.Cells["middle_name"].Value.ToString();
            string z= selectedrow.Cells["last_name"].Value.ToString();
            Form2 n = new Form2(x,y,z);
            n.ShowDialog();

        }
    }
}
