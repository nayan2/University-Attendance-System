using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using System.Data.SqlClient;
using databaseconnection.DataAccess;
using databaseconnection.Entities;
using databaseconnection.Framework;

namespace Student_side
{
    public partial class Form3 : MetroFramework.Forms.MetroForm
    {
        string x, y, z;
        public Form3(string FirstName,string MiddleName,string LastName)
        {
            InitializeComponent();
            x = FirstName;
            y = MiddleName;
            z = LastName;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            attendance_system_data c = new attendance_system_data();
            metroGrid1.DataSource = c.GeTFullTsfFile(c.GetTableNameFromGridView(x, y, z)); //set table c.GeTFullTsfFile retuen a table..
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 n = new Form2(x,y,z);
            n.ShowDialog();
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //write cosde for search.....
            //String searchValue = metroTextBox1.Text;
            ///int rowIndex = -1;
            try
            {
                attendance_system_data d = new attendance_system_data();
                metroGrid1.DataSource = d.TsfSearchbox(metroTextBox1.Text, d.GetTableNameFromGridView(x, y, z));
                /*foreach (DataGridViewRow row in metroGrid1.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals(searchValue))
                    {
                        rowIndex = row.Index;
                        break;
                    }
                }
                metroGrid1.Rows[rowIndex].Selected = true;*/
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.ToString());
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
