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
using System.IO;
using System.Text.RegularExpressions;

namespace our_c_sharp_project
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        string filepath = null;
        public Form2()
        {
            InitializeComponent();
            metroTextBox1.MaxLength = 14;
        }

        public bool IsEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            { return false; }
            try
            {
                Regex _regex = new Regex("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])" +
                        "+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)" +
                        "((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|" +
                        "[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\u" +
                        "FDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|" +
                        "(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|" +
                        "[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900" +
                        "-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFF" +
                        "EF])))\\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                return _regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Form1.insert(metroComboBox1, "select * from dbo.faculty_id where dbo.faculty_id.id not in (select dbo.login.username from dbo.login);", "id");
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 d = new Form1();
            d.Show();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog g = new OpenFileDialog();
            g.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg";
            if(g.ShowDialog() == DialogResult.OK)
            {
                string x = g.FileName.ToString();
                pictureBox1.ImageLocation = x;
                filepath = x;
               
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedItem.ToString() == "" || metroTextBox8.Text == "" || metroTextBox9.Text == "" || metroTextBox5.Text == "" || metroTextBox3.Text == "" || metroDateTime1.Text == "" || metroTextBox7.Text == "" || metroTextBox1.Text == "")
            {
                    MetroMessageBox.Show(this, "Username,password,fist name,present address,permanent address,dob,email and contact number can't be unfill", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);

             }
            else
            {
                try
                {

                    p_contact_number o = new p_contact_number();
                    o.username = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
                    o.contact_number = metroTextBox1.Text;
                    attendance_system_data u = new attendance_system_data();

                    p_email r = new p_email();
                    r.username = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
                    r.email = metroTextBox7.Text;

                    personal_info i = new personal_info();
                    i.first_name = metroTextBox9.Text;
                    i.middle_name = metroTextBox2.Text;
                    i.last_name = metroTextBox4.Text;
                    i.present_address = metroTextBox5.Text;
                    i.permanent_address = metroTextBox3.Text;
                    i.username = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
                    i.password = metroTextBox8.Text;
                    i.dob = this.metroDateTime1.Text;

                    FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(stream);

                    byte[] photo = reader.ReadBytes((int)stream.Length);

                    reader.Close();
                    stream.Close();

                    i.image = photo;


                    if (u.insertpersonalinfodata(i) == true)
                    {
                        u.createtable(this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem));
                        u.insertemaildata(r);
                        u.insertcontactdata(o);
                        u.updatelogintable(this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem), metroTextBox8.Text);
                        u.insertintotable(this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem),"Available");

                        DialogResult d = MetroMessageBox.Show(this, "Registation successfully completed", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (d == DialogResult.OK)
                        {
                            this.Hide();
                            Form1 f = new Form1();
                            f.Show();
                        }
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Something went wrong!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (System.ArgumentNullException)
                {
                    MetroMessageBox.Show(this, "Image can't be unfilled", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                catch (Exception en)
                {
                    MetroMessageBox.Show(this, en.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }

        }

        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if(!char.IsDigit(ch) && ch!=8 && ch!=46)
            {
                e.Handled = true;
            } 
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox7_Leave(object sender, EventArgs e)
        {
            if(IsEmail(metroTextBox7.Text) == false)
            {
                MetroMessageBox.Show(this,"Invalid Email Address","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Error);
                metroTextBox7.Clear();
            }
        }

        private void metroTextBox7_Click(object sender, EventArgs e)
        {

        }


    }
}
