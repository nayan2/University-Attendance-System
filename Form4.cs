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

namespace our_c_sharp_project
{
    public partial class Form4 : MetroFramework.Forms.MetroForm
    {
        string username1;
        string status1;
        int count1;
        public Form4(string username,string status,int count)
        {
            InitializeComponent();
            username1 = username;
            status1 = status;
            count1 = count;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
            metroComboBox3.SelectedIndex = 0;
        }

        public void time()
        {
            metroComboBox1.Visible = true;
            metroComboBox2.Visible = true;
            metroComboBox3.Visible = true;
            metroDateTime1.Visible = false;
        }

        public void date()
        {
                this.Text = "Available from";
                metroComboBox1.Visible = false;
                metroComboBox2.Visible = false;
                metroComboBox3.Visible = false;
                metroDateTime1.Visible = true;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ////forchchangestatus h = new forchchangestatus();
            faculty_status f = new faculty_status();
            attendance_system_data da = new attendance_system_data();

            if(count1 == 0)
            {
                string y = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem) + "." + this.metroComboBox2.GetItemText(this.metroComboBox2.SelectedItem) + " " + this.metroComboBox3.GetItemText(this.metroComboBox3.SelectedItem);
                ///double s = Convert.ToDouble(this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem) + "." + this.metroComboBox2.GetItemText(this.metroComboBox2.SelectedItem));
                f.username = username1;
                f.status = status1;
                f.available_from = y;
                //h.time = s;  //prevent to call checkorupdatefacultystatus method
            }
            else if(count1 == 1)
            {
                string x = metroDateTime1.Text;
                f.username = username1;
                f.status = status1;
                f.available_from = x;
                ///h.date = x;   //prevent to call checkorupdatefacultystatus method
            }

            da.forcelychangestatus(f);
            ///h.fstatu = true;  //prevent to call checkorupdatefacultystatus method
            
            if (da.forcelychangestatus(f) == true)
            {
                this.Hide();
                MetroMessageBox.Show(this, "Successfully updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MetroMessageBox.Show(this, "Something went wrong", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }
    }
}
