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

namespace our_c_sharp_project
{


    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        public Form1()
        {
            InitializeComponent();
            metroTextBox2.PasswordChar = '.';
        }

        public static void insert(ComboBox x,string query,string table_name)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand(query);
            SqlDataAdapter d = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            d.Fill(dt);
            x.DataSource = dt.Tables[0];
            x.DisplayMember = table_name;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            insert(metroComboBox1,"select id from dbo.faculty_id","id");
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 c = new Form2();
            c.Show();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor=default(Color);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            login s = new login();
            string selected = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
            s.username = selected;
            s.password = metroTextBox2.Text;
            attendance_system_data u = new attendance_system_data();
            if(u.loginvalidationcheck(s)==true)
            {
                this.Hide();
                string x = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem); 
                Form3 n = new Form3(x);
                n.Show();
            }
            else
            {
                MetroMessageBox.Show(this,"Invalid User!!!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
