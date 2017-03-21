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
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;
using ExcelLibrary.BinaryDrawingFormat;
using ExcelLibrary.BinaryFileFormat;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Net;
using System.Web;
using System.Net.Mail;
using System.Net.Sockets;

namespace Student_side
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        Socket sck;
        EndPoint EpLocal, EpRemote;
        string x, y, z;
        string h;
        int Senderport=80;
        int ReceivingPort=81;
        public Form2(string FirstName,string MiddleName,string LastName)
        {
            InitializeComponent();
            x = FirstName; //get first name from form1
            y = MiddleName; //get middle name from form1
            z = LastName; //get last name from form1
            sck = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress,true);
            h = getlocalip();
        }
        private string getlocalip()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
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
                int size = sck.EndReceiveFrom(aresult,ref EpRemote);
                if(size > 0)
                {
                    byte[] receivedata=new byte[1464];
                    receivedata = (byte[])aresult.AsyncState;

                    ASCIIEncoding endcoding = new ASCIIEncoding();
                    string receivedmessage = endcoding.GetString(receivedata);
                    listBox1.Items.Add("Faculty:"+receivedmessage);
                }
                byte[] buffer=new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref EpRemote,new AsyncCallback(MessageCallBack),buffer);
            }
            catch(Exception exp)
            {
                MetroMessageBox.Show(this, exp.ToString());
            }
        }
        public bool IsValidEmail(string email)
        {
         try 
         {
              var addr = new System.Net.Mail.MailAddress(email);
              return true;
         }
         catch
         {
             return false;
         }
         }       

        public static bool CheckForInternetConnection()   //for checking internet connection
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            metroButton1.Text = "Show "+x+" "+y+" "+z+" tsf File"; //set button text
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            attendance_system_data c = new attendance_system_data();
            if(c.checktableisnullornot(c.GetTableNameFromGridView(x, y, z)) == true)
            {
                MetroMessageBox.Show(this,"Faculty isn't provided any tsf file","Failed",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                ///GetTableNameFromGridView
                /*DataSet ds = new DataSet("New_dataset");
                ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                ds.Tables.Add(c.gettableforexcelsheet(x, y, z));
                ExcelLibrary.DataSetHelper.CreateWorkbook("tsf.xlsx", ds);
                DialogResult d = MetroMessageBox.Show(this, "Successfully Downloaded.Do you wants to open this file?", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (d == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start("E:\\c# project\\our_c_sharp_project\\Student_side\bin\\Debug\\tsf.xlsx");
                }*/
                this.Hide();
                Form3 n = new Form3(x,y,z);
                n.ShowDialog();
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if(CheckForInternetConnection()==false)
            {
                MetroMessageBox.Show(this,"Internet Connection is not Available","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                metroButton7.Visible = false;
                metroPanel1.Visible = true;
                metroButton6.Visible = true;
                //GetEmailByUsername
                attendance_system_data c = new attendance_system_data();
                To.Text = c.GetEmailByUsername(x, y, z);
            }
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage(From.Text,To.Text,Subject.Text,Body.Text);
            SmtpClient clint = new SmtpClient();
                              //for determile email smtp...
            string x=Username.Text;
            int startIndex = x.IndexOf('@');
            int endIndex = x.LastIndexOf('.');
            int length = endIndex - startIndex;
            string xx = x.Substring(startIndex + 1, length - 1);

            if(xx == "gmail" || xx == "Gmail")
            {
                clint.Host="smtp.gmail.com";
                clint.Port = 587;
                clint.EnableSsl = true;
            }
            if (xx == "Hotmail" || xx == "hotmail" || xx == "live" || xx == "Live")
            {
                clint.Host = "smtp.live.com";
                clint.Port = 587;
                clint.EnableSsl = true;
            }
            if (xx == "yahoo" || xx == "Yahoo")
            {
                clint.Host = "smtp.mail.yahoo.com";
                clint.Port = 465;
                clint.EnableSsl = true;
            }
            clint.Credentials = new System.Net.NetworkCredential(Username.Text, Password.Text);
            clint.DeliveryMethod = SmtpDeliveryMethod.Network;
            clint.UseDefaultCredentials = false;
            clint.Send(mail);
            MetroMessageBox.Show(this,"Email Successfully Send","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void From_Leave(object sender, EventArgs e)
        {
            if (IsValidEmail(To.Text) == false)
            {
                MetroMessageBox.Show(this, "Invalid Email", "Invalid", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            Username.Text=From.Text;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            metroButton7.Visible = true;
            metroPanel1.Visible = false;
            metroButton6.Visible = false;
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = enc.GetBytes(metroTextBox1.Text);

                sck.Send(msg);

                listBox1.Items.Add("You:" + metroTextBox1.Text);
                metroTextBox1.Clear();
            }
            catch(Exception ex)
            {
                MetroMessageBox.Show(this,ex.ToString());
            }
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            ///write connection code......
            try
            {
                EpLocal = new IPEndPoint(IPAddress.Parse(h), Convert.ToInt32(Senderport));
                sck.Bind(EpLocal);

                EpRemote = new IPEndPoint(IPAddress.Parse(h), Convert.ToInt32(ReceivingPort));
                sck.Connect(EpRemote);
                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref EpRemote, new AsyncCallback(MessageCallBack), buffer);
                
                metroButton7.Text = "Connected";
                metroButton7.Enabled = false;
                metroButton4.Enabled = true;
                metroTextBox1.Focus();

            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.ToString());
            }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
