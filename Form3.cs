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
using SautinSoft;
using System.IO;
using System.Threading;
using System.Timers;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;


namespace our_c_sharp_project
{
    public partial class Form3 : MetroFramework.Forms.MetroForm
    {
        Socket sck;
        EndPoint EpLocal, EpRemote;
        int nayan = 0;
        string g;
        int count;
        string h=null;
        int Senderport = 81;
        int ReceivingPort = 80;
        public Form3(string x)
        {
            InitializeComponent();
            g = x;
        }

        private string getlocalip()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "172.0.0.1";
        }

        private void MessageCallBack(IAsyncResult aresult)
        {
            try
            {
                int size = sck.EndReceiveFrom(aresult, ref EpRemote);
                if (size > 0)
                {
                    byte[] receivedata = new byte[1464];
                    receivedata = (byte[])aresult.AsyncState;

                    ASCIIEncoding endcoding = new ASCIIEncoding();
                    string receivedmessage = endcoding.GetString(receivedata);
                    listBox1.Items.Add("Student:" + receivedmessage);
                }
                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref EpRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception exp)
            {
                MetroMessageBox.Show(this, exp.ToString());
            }
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void t_Elapsed(attendance_system_data u, System.Timers.ElapsedEventArgs e)
        {
            u.getsstatusuponchange(g);
        }
        private void Form3_Load_1(object sender, EventArgs e)
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            metroComboBox1.Enabled = false;

            attendance_system_data u = new attendance_system_data();
            textBox1.Text = attendance_system_data.importname(g);
            object[] i = { "Available", "Busy", "In class", "Taking a break", "Exam deauty", "Having lunch", "In metting", "Gone for the day", "On vacation" };
            metroComboBox1.Items.AddRange(i);
            string h = "\"" + g + "\"";

            DateTime d = DateTime.Now;
            string j = d.ToString("dddd");
            //string j = "Sunday";   //for initial testing
            double hg = Convert.ToDouble(d.ToString("HH.mm"));
            ///double hg = 12.00;    //for initial testing
            double xx = Convert.ToDouble(d.ToString("hh.mm"));

            string formatedtime = xx.ToString("00.00");
            string formateddate = d.ToString("dd.MM.yyyy");

            if (j.StartsWith("Friday") || j.StartsWith("Saturday") || (hg >= 00.01 && hg <= 07.59))
            {
                metroButton2.Enabled = false;
                MetroMessageBox.Show(this,"Your status system will not update dynamically on off days or between (00.01 - 07.59)AM","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

            else
            {
                if (u.checktableisnullornot(h) == true)
                {
                    MetroMessageBox.Show(this, "your initial status is available untill you upload a tsf file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    metroComboBox1.SelectedIndex = 0;
                }
                else if (u.checktableisnullornot(h) == false)
                {
                    login gk = new login();
                    gk.username = g;
                    if (u.cecckifstatuschangebyforcrornot(gk) == false)
                    {
                        u.checkorupdatefacultystatus(g);
                        if (u.getsstatusuponchange(g) == "Available")
                        {
                            metroComboBox1.SelectedIndex = 0;
                        }
                        else if (u.getsstatusuponchange(g) == "In class")
                        {
                            metroComboBox1.SelectedIndex = 2;
                        }
                        else if (u.getsstatusuponchange(g) == "Gone for the day")
                        {
                            metroComboBox1.SelectedIndex = 7;
                        }
                    }
                    else if (u.cecckifstatuschangebyforcrornot(gk) == true) //status changed by foeced
                    {
                        if(u.checkifavailable_fromistime(gk) == false)
                        {
                            string x = u.getavailable_fromtimedata(gk);
                            if(Convert.ToDouble(formatedtime) <= Convert.ToDouble(x))
                            {
                                //combox demonastration.......
                                nayan = 1;     //counter set not to open combobox
                                string xxxx = u.getsstatusuponchange(g);
                                metroComboBox1.SelectedIndex = metroComboBox1.FindStringExact(xxxx);
                                nayan = 0; //counter set to open combobox
                                
                            }
                            else
                            {
                                u.checkorupdatefacultystatus(g);
                                if (u.getsstatusuponchange(g) == "Available")
                                {
                                    metroComboBox1.SelectedIndex = 0;
                                }
                                else if (u.getsstatusuponchange(g) == "In class")
                                {
                                    metroComboBox1.SelectedIndex = 2;
                                }
                                else if (u.getsstatusuponchange(g) == "Gone for the day")
                                {
                                    metroComboBox1.SelectedIndex = 7;
                                }
                            }
                        }
                        else if (u.checkifavailable_fromistime(gk) == true)
                        {
                            string x = u.getavailable_fromdatedata(gk);
                            int y = DateTime.Compare(Convert.ToDateTime(formateddate),Convert.ToDateTime(x));

                            if(y < 0 || y == 0)
                            {
                                //combox demonastration.......
                                nayan = 1;   //counter set not to open combobox
                                string xxxx = u.getsstatusuponchange(g);
                                metroComboBox1.SelectedIndex = metroComboBox1.FindStringExact(xxxx);
                                nayan = 0;  //counter set to open combobox
                            }
                            else
                            {
                                u.checkorupdatefacultystatus(g);
                                if (u.getsstatusuponchange(g) == "Available")
                                {
                                    metroComboBox1.SelectedIndex = 0;
                                }
                                else if (u.getsstatusuponchange(g) == "In class")
                                {
                                    metroComboBox1.SelectedIndex = 2;
                                }
                                else if (u.getsstatusuponchange(g) == "Gone for the day")
                                {
                                    metroComboBox1.SelectedIndex = 7;
                                }
                            }
                        }

                    }
                }
            } 
                       
            if(u.importimage(g)==null)
            {
                pictureBox1.Image = null;
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(u.importimage(g), 0, u.importimage(g).Length);
                Bitmap bitmap = new Bitmap(stream);
                pictureBox1.Image = bitmap;
            }
            
            if (u.checktableexistancy(g) == true)
            {
                if(u.checktableisnullornot(h)==true)
                {
                    metroButton1.Enabled = true;
                }
                else
                {
                    metroButton1.Enabled = false;
                }
            }
            /*else if (u.checktableexistancy(g) == true && u.checktableisnullornot(h)==false)
            {
                metroButton1.Enabled = false;
            }*/
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string x = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);

            faculty_status f = new faculty_status();
            attendance_system_data da = new attendance_system_data();

            if (nayan == 0)
            {
                if (x.Equals("In class") || x.Equals("Taking a break") || x.Equals("Exam deauty") || x.Equals("Having lunch") || x.Equals("In metting"))
                {
                    count = 0;
                    Form4 n = new Form4(g, this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem), count);
                    n.time();
                    n.ShowDialog();
                }
                else if (x.Equals("On vacation"))
                {
                    count = 1;
                    Form4 n = new Form4(g, this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem), count);
                    n.date();
                    n.ShowDialog();
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            attendance_system_data u = new attendance_system_data();
            OpenFileDialog k = new OpenFileDialog();
            k.Filter = "pdf Files(*.pdf)|*.pdf|xls Files(*.xls)|*.xls|xlsx Files(*.xlsx)|*.xlsx|All Files(*.*)|*.*";
            if (k.ShowDialog() == DialogResult.OK)
            {
                string x = k.FileName.ToString();
                if(Path.GetExtension(x) == ".pdf")
                {

                }
                else
                {
                    string h = "\""+g+"\"";
                    if(u.insertexceldata(x,h) == true && u.InsertFileByUsername(x,g))
                    {
                        MetroMessageBox.Show(this,"Successfully Inserted","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            DialogResult dt = MetroMessageBox.Show(this,"Do you want to change you status?","",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);

            if (dt == DialogResult.OK)
            {
                metroComboBox1.Enabled = true;
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            DialogResult d = MetroMessageBox.Show(this,"Do you want to Sign out?","Exit",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(d == DialogResult.Yes)
            {
                this.Hide();
                Form1 n = new Form1();
                n.Show();
            }
        }

        private void metroComboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            metroButton7.Visible = true;
            metroButton6.Visible = true;
            metroPanel1.Visible = true;
            listBox1.Visible = true;
            metroTextBox1.Visible = true;
            metroButton5.Visible = true;
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            //add code for send messmmage....
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = enc.GetBytes(metroTextBox1.Text);

                sck.Send(msg);

                listBox1.Items.Add("You:" + metroTextBox1.Text);
                metroTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.ToString());
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            metroButton7.Visible = false;
            metroButton6.Visible = false;
            metroPanel1.Visible = false;
            listBox1.Visible = false;
            metroTextBox1.Visible = false;
            metroButton5.Visible = false;
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            //write connection code------
            try
            {
                EpLocal = new IPEndPoint(IPAddress.Parse(getlocalip()), Convert.ToInt32(Senderport));
                sck.Bind(EpLocal);

                EpRemote = new IPEndPoint(IPAddress.Parse(getlocalip()), Convert.ToInt32(ReceivingPort));
                sck.Connect(EpRemote);
                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref EpRemote, new AsyncCallback(MessageCallBack), buffer);
                metroButton7.Text = "Connected";
                metroButton7.Enabled = false;
                metroButton5.Enabled = true;
                metroTextBox1.Focus();

            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.ToString());
            }
        }
    }
}
