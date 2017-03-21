using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using databaseconnection.Entities;
using databaseconnection.Framework;
using System.Data;
using System.Windows;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using excelsheetconnection.Entities;

namespace databaseconnection.DataAccess
{
    public class attendance_system_data
    {
        public bool loginvalidationcheck(login obj)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select username,password from dbo.login where username = @username and password = @password;");
            
            SqlParameter p = new SqlParameter("@username", SqlDbType.VarChar,10);
            p.Value = obj.username;

            SqlParameter p1 = new SqlParameter("@password",SqlDbType.VarChar,100);
            p1.Value = obj.password;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();
            SqlDataReader myreader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myreader.Read();
            
            cmd.Connection.Close();

            

        }        
        public bool insertpersonalinfodata(personal_info g)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("insert into dbo.personal_info (first_name,middle_name,last_name,present_address,permanent_address,username,password,dob,image)"
                + "values (@first_name, @middle_name, @last_name, @present_address, @permanent_address, @username, @password, @dob, @image)");


            SqlParameter p = new SqlParameter("@first_name",SqlDbType.VarChar,50);
            p.Value = g.first_name;

            SqlParameter p1 = new SqlParameter("@middle_name",DBNull.Value);
            p1.Value = g.middle_name;

            SqlParameter p2 = new SqlParameter("@last_name",DBNull.Value);
            p2.Value = g.last_name;


            SqlParameter p3 = new SqlParameter("@present_address", SqlDbType.VarChar, 200);
            p3.Value = g.present_address;

            SqlParameter p4 = new SqlParameter("@permanent_address", SqlDbType.VarChar, 200);
            p4.Value = g.permanent_address;

            SqlParameter p5 = new SqlParameter("@username", SqlDbType.VarChar, 10);
            p5.Value = g.username;

            SqlParameter p6 = new SqlParameter("@password", SqlDbType.VarChar, 50);
            p6.Value = g.password;

            SqlParameter p7 = new SqlParameter("@dob", SqlDbType.Date);
            p7.Value = g.dob;

            SqlParameter p8 = new SqlParameter("@image", DBNull.Value);
            p8.Value = g.image;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.Parameters.Add(p7);
            cmd.Parameters.Add(p8);
                

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

            return true; 

        }
        public bool insertcontactdata(p_contact_number c)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("insert into dbo.p_contact_number (contact_number,username)"
    + "values (@contact_number, @username)");

            SqlParameter p = new SqlParameter("@contact_number", SqlDbType.VarChar, 14);
            p.Value = c.contact_number;

            SqlParameter p1 = new SqlParameter("@username", SqlDbType.VarChar, 10);
            p1.Value = c.username;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

            return true;

        }
        public bool insertemaildata(p_email e)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("insert into dbo.p_email (email,username)"
    + "values (@email, @username)");

            SqlParameter p = new SqlParameter("@email", SqlDbType.VarChar, 14);
            p.Value = e.email;

            SqlParameter p1 = new SqlParameter("@username", SqlDbType.VarChar, 10);
            p1.Value = e.username;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

            return true;
        }
        public void updatelogintable(string username,string password)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("insert into dbo.login (username,password)"
                +"values (@username, @password)");

            SqlParameter p = new SqlParameter("@username",SqlDbType.VarChar,10);
            p.Value = username;

            SqlParameter p1 = new SqlParameter("@password",SqlDbType.VarChar,100);
            p1.Value = password;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

        }
        public static string importname(string username)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select personal_info.first_name,personal_info.middle_name,personal_info.last_name from dbo.personal_info where personal_info.username=@username");

            SqlParameter p = new SqlParameter("@username",SqlDbType.VarChar,10);
            p.Value = username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader dr=cmd.ExecuteReader();

            dr.Read();

            return "            "+ dr.GetString(0) + " " + dr.GetString(1) + " " + dr.GetString(2);
        }
        public void createtable(string name)
        {
            string r = "\"" + name + "\"";
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("create table " + r + " (Time varchar(11),Sunday varchar(8000),Monday varchar(8000),Tuesday varchar(8000),Wednesday varchar(8000),Thursday varchar(8000))");

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();
        }
        public byte[] importimage(string username)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select personal_info.image from personal_info where personal_info.username=@username");

            SqlParameter p = new SqlParameter("@username",DBNull.Value);
            p.Value = username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            
            byte[] image = (byte[])cmd.ExecuteScalar();
            cmd.Connection.Close();

            return image;
            
        }
        public bool insertexceldata(string filepath,string Tablename)
        {
            String strConnection = "Data Source=FUJITSU-PC\\SQLEXPRESS;Initial Catalog=attendance_system;Integrated Security=True";

            OleDbDataAccess da = new OleDbDataAccess();
            OleDbCommand cmd = da.getcommand("Select * FROM [Sheet1$]",filepath);

            cmd.Connection.Open();

            OleDbDataReader dReader;

            dReader = cmd.ExecuteReader();

            SqlBulkCopy sqlBulk = new SqlBulkCopy(strConnection);

            sqlBulk.DestinationTableName = Tablename;

            sqlBulk.WriteToServer(dReader);

            cmd.Connection.Close();

            return true;
        }
        public bool checktableexistancy(string tablename)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("checkTableExist");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@theDate", SqlDbType.VarChar).Value = tablename;
            cmd.Connection.Open();
            int result = (Int32)cmd.ExecuteScalar();
            cmd.Connection.Close();

            if (result == 1)
                return true; //table exists
            else
                return false; //table not exist
        }
        public bool checktableisnullornot(String tablename)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select count(*) from "+tablename);

            cmd.Connection.Open();

            int result = int.Parse(cmd.ExecuteScalar().ToString());

            cmd.Connection.Close();

            if (result == 0)
                return true; //is empty
            else
                return false;//is not empty
        }
        public int checkcolumnexistancy(string username)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select count (*) from dbo.faculty_status where dbo.faculty_status.username='" + username + "'");

            cmd.Connection.Open();

            int i = (int)cmd.ExecuteScalar(); //if return 1 means column exists
            cmd.Connection.Close();
            return i;
        }
        public void updatetable(string username,string status)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("update faculty_status set status='" + status + "' where username='" + username + "'");

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public void insertintotable(string username,string status)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("insert into dbo.faculty_status(username,status,available_from) values('" + username + "','" + status + "','null')");

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public void checkorupdatefacultystatus(string username)
        {
            DateTime d = DateTime.Now;
            string j = d.ToString("dddd");
            ///string j = "Sunday";        //for initial testing
            string x = d.ToString("HH.mm");
            double g = Convert.ToDouble(x);
            //double g = 12.00;   //for initial testing
            double f = 00.00;

            if (g >= 08.00 && g <= 08.29)
            {
                f = 08.00;
            }
            else if (g >= 08.30 && g <= 08.59)
            {
                f = 08.30;
            }
            else if (g >= 09.00 && g <= 09.29)
            {
                f = 09.00;
            }
            else if (g >= 09.30 && g <= 09.59)
            {
                f = 09.30;
            }
            else if (g >= 10.00 && g <= 10.29)
            {
                f = 10.00;
            }
            else if (g >= 10.30 && g <= 10.59)
            {
                f = 10.30;
            }
            else if (g >= 11.00 && g <= 11.29)
            {
                f = 11.00;
            }
            else if (g >= 11.30 && g <= 11.59)
            {
                f = 11.30;
            }
            else if (g >= 12.00 && g <= 12.29)
            {
                f = 12.00;
            }
            else if (g >= 12.30 && g <= 12.59)
            {
                f = 12.30;
            }
            else if (g >= 13.00 && g <= 13.29)
            {
                f = 13.00;
            }
            else if (g >= 13.30 && g <= 13.59)
            {
                f = 13.30;
            }
            else if (g >= 14.00 && g <= 14.29)
            {
                f = 14.00;
            }
            else if (g >= 14.30 && g <= 14.59)
            {
                f = 14.30;
            }
            else if (g >= 15.00 && g <= 15.29)
            {
                f = 15.00;
            }
            else if (g >= 15.30 && g <= 15.59)
            {
                f = 15.30;
            }
            else if (g >= 16.00 && g <= 16.29)
            {
                f = 16.00;
            }
            else if (g >= 16.30 && g <= 16.59)
            {
                f = 16.30;
            }
            else if (g >= 17.00 && g <= 17.29)
            {
                f = 17.00;
            }
            else if (g >= 17.30 && g <= 17.59)
            {
                f = 17.30;
            }
            else if (g >= 18.00 && g <= 18.29)
            {
                f = 18.00;
            }
            else if (g >= 18.30 && g <= 18.59)
            {
                f = 18.30;
            }
            else if (g >= 19.00 && g <= 19.29)
            {
                f = 19.00;
            }
            else if (g >= 19.30 && g <= 19.59)
            {
                f = 19.30;
            }
            else if (g >= 20.00 && g <= 20.29)
            {
                f = 20.00;
            }
            else if (g >= 20.30 && g <= 20.59)
            {
                f = 20.30;
            }
            else if (g >= 21.00 && g <= 21.29)
            {
                f = 21.00;
            }
            else if (g >= 21.30 && g <= 21.59)
            {
                f = 21.30;
            }
            else if (g >= 22.00 && g <= 22.29)
            {
                f = 22.00;
            }
            else if (g >= 22.30 && g <= 22.59)
            {
                f = 22.30;
            }
            else if (g >= 23.00 && g <= 23.29)
            {
                f = 23.00;
            }
            else if (g >= 23.30 && g <= 23.59)
            {
                f = 23.30;
            }

            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd=da.GetCommand("select dbo.[" + username + "]." + j + " from dbo.[" + username + "] where dbo.[" + username + "].Time like '%" + f.ToString("00.00") + "%';");

            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            string i = null;

            try
            {
                i = dr.GetString(0);
            }

            catch(System.Data.SqlTypes.SqlNullValueException)
            {
                i = "null";
            }
            catch (System.InvalidOperationException)
            {
                i = "null";
            }
            cmd.Connection.Close();

            string status=null;

            if(i.StartsWith("null"))
            {
                status="Gone for the day";
            }

            else if (i.StartsWith("Consulting"))
            {
                status="Available";
            }

            else
            {
                status="In class";
            }

            if (checkcolumnexistancy(username) == 1)
            {
                updatetable(username,status);
            }
            else if (checkcolumnexistancy(username) == 0)
            {
                insertintotable(username, status);
            }
        }
        public string getsstatusuponchange(string username)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select dbo.faculty_status.status from dbo.faculty_status where dbo.faculty_status.username=@username;");

            SqlParameter p = new SqlParameter("@username",SqlDbType.VarChar,10);
            p.Value = username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            string i = null;

            try
            {
                i = dr.GetString(0);
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                i = "null";
            }
            return i;
        }
        public bool forcelychangestatus(faculty_status f)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("update dbo.faculty_status set status=@status,available_from=@available_from where dbo.faculty_status.username=@username;");

            SqlParameter p = new SqlParameter("@status",SqlDbType.VarChar,50);
            p.Value = f.status;

            SqlParameter p1 = new SqlParameter("@available_from",SqlDbType.VarChar,10);
            p1.Value = f.available_from;

            SqlParameter p2 = new SqlParameter("@username",SqlDbType.VarChar,10);
            p2.Value = f.username;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }
        public bool cecckifstatuschangebyforcrornot(login s)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select count(*) from dbo.faculty_status where dbo.faculty_status.username=@username and dbo.faculty_status.available_from like'%null%';");

            SqlParameter p = new SqlParameter("@username",SqlDbType.VarChar,10);
            p.Value = s.username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            int value = int.Parse(cmd.ExecuteScalar().ToString());

            cmd.Connection.Close();

            if (value == 0)
            {
                return true; //table status change by force
            }
            else
                return false;  //table status is not change by force
        }
        public bool checkifavailable_fromistime(login s)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select count(*) from dbo.faculty_status where dbo.faculty_status.username=@username and dbo.faculty_status.available_from like '[0-9][0-9][.][0-9][0-9][ ][A-Z][A-Z]'");

            SqlParameter p = new SqlParameter("@username", SqlDbType.VarChar, 10);
            p.Value = s.username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            int value = int.Parse(cmd.ExecuteScalar().ToString());

            cmd.Connection.Close();

            if (value == 0)
            {
                return true; 
            }
            else
                return false;
        }
        public string getavailable_fromtimedata(login s)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select substring(dbo.faculty_status.available_from,1,5) from dbo.faculty_status where dbo.faculty_status.username=@username");

            SqlParameter p = new SqlParameter("@username",SqlDbType.VarChar,10);
            p.Value = s.username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            string x = d.GetString(0);

            cmd.Connection.Close();

            return x;
        }
        public string getavailable_fromdatedata(login s)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.faculty_status.available_from from dbo.faculty_status where dbo.faculty_status.username=@username");

            SqlParameter p = new SqlParameter("@username", SqlDbType.VarChar, 10);
            p.Value = s.username;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            string x = d.GetString(0);

            cmd.Connection.Close();

            return x;
        }
        public DataTable returnavailablefacultylist() 
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.personal_info.first_name,dbo.personal_info.middle_name,dbo.personal_info.last_name,dbo.faculty_status.status,dbo.faculty_status.available_from from dbo.personal_info inner join dbo.faculty_status on dbo.personal_info.username=dbo.faculty_status.username;");

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close(); 
            }
            return tbl;
        }
        public DataTable searchbox(string x)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.personal_info.first_name,dbo.personal_info.middle_name,dbo.personal_info.last_name,dbo.faculty_status.status,dbo.faculty_status.available_from from dbo.personal_info inner join dbo.faculty_status on dbo.personal_info.username=dbo.faculty_status.username where dbo.personal_info.first_name like '%" + x + "%' or dbo.personal_info.middle_name like '%" + x + "%' or dbo.personal_info.last_name like '%" + x + "%';");

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }
        public DataTable TsfSearchbox(string keyword,string username)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select * from dbo." + username + " where dbo." + username + ".Sunday like '%" + keyword + "%' or dbo." + username + ".Monday like '%" + keyword + "%' or dbo." + username + ".Thursday like '%" + keyword + "%' or dbo." + username + ".Tuesday like '%" + keyword + "%' or dbo." + username + ".Wednesday like '%" + keyword + "%' or dbo." + username + ".Thursday like '%" + keyword + "%';");

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }
        public DataTable gettableforexcelsheet(string FirstName, string MiddleName, string LastName)
        {
            string y;
            DataTable tbl = new DataTable();
            SqlDbDataAccess dr = new SqlDbDataAccess();
            using (SqlCommand cmd = dr.GetCommand("select dbo.personal_info.username from dbo.personal_info where dbo.personal_info.first_name like '%"+FirstName+"%' and dbo.personal_info.middle_name like '%"+MiddleName+"%' and dbo.personal_info.last_name like '%"+LastName+"%';"))
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();
                y = d.GetString(0);
                cmd.Connection.Close();
                d.Close();
            }
            using (SqlCommand cmd = dr.GetCommand("select * from dbo.["+y+"]"))
            {
                using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    dt.Fill(tbl);
                    cmd.Connection.Close();
                }
            }
            return tbl;
        }
        public DataTable GeTFullTsfFile(string username)
        {
            DataTable tbl = new DataTable();
            SqlDbDataAccess dr = new SqlDbDataAccess();
            using (SqlCommand cmd =dr.GetCommand("select * from dbo."+username+""))
            {
                using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    dt.Fill(tbl);
                    cmd.Connection.Close();
                }
            }
            return tbl;
        }
        public string GetTableNameFromGridView(string FirstName, string MiddleName, string LastName)
        {
            string y;
            SqlDbDataAccess dr = new SqlDbDataAccess();
            using (SqlCommand cmd = dr.GetCommand("select dbo.personal_info.username from dbo.personal_info where dbo.personal_info.first_name like '%" + FirstName + "%' and dbo.personal_info.middle_name like '%" + MiddleName + "%' and dbo.personal_info.last_name like '%" + LastName + "%';"))
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();
                y = d.GetString(0);
                cmd.Connection.Close();
                d.Close();
            }
            return "["+y+"]";
        }
        public string GetEmailByUsername(string FirstName, string MiddleName, string LastName)
        {
            string y, z;
            SqlDbDataAccess dr = new SqlDbDataAccess();
            using (SqlCommand cmd = dr.GetCommand("select dbo.personal_info.username from dbo.personal_info where dbo.personal_info.first_name like '%" + FirstName + "%' and dbo.personal_info.middle_name like '%" + MiddleName + "%' and dbo.personal_info.last_name like '%" + LastName + "%';"))
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();
                y = d.GetString(0);
                cmd.Connection.Close();
                d.Close();
            }
            using (SqlCommand cmd = dr.GetCommand("select dbo.p_email.email from dbo.p_email where dbo.p_email.username='"+y+"';"))
            {
                using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    SqlDataReader d = cmd.ExecuteReader();
                    d.Read();
                    z = d.GetString(0);
                    cmd.Connection.Close();
                }
            }
            return z;
        }
        public void AutoUpdateBackTime() //update back time in student side for everyuser
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            DateTime d = DateTime.Now;
            string xd = d.ToString("hh.mm");
            double time = Convert.ToDouble(xd);
            string date = d.ToString("dddd");

            using (SqlCommand cmd = dr.GetCommand("select dbo.faculty_status.username from dbo.faculty_status"))
            {
                
                cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string x = reader["username"].ToString();
                        using (SqlCommand cmmd = dr.GetCommand("if (select dbo.faculty_status.status from dbo.faculty_status where dbo.faculty_status.username='" + x + "') like 'In class' select 1 else select 0;"))
                        {  
                            cmmd.Connection.Open();

                            int result = int.Parse(cmmd.ExecuteScalar().ToString());

                            cmmd.Connection.Close();

                            if (result == 1)  //means faculty status is In class
                            {
                                using (SqlCommand newcmd = dr.GetCommand("select dbo.[" + x + "]." + date + " from dbo.[" + x + "] where dbo.[" + x + "]." + date + " like '%Consult%';"))
                                {     
                                    newcmd.Connection.Open();
                                    using (var reader1 = newcmd.ExecuteReader())
                                    {
                                        while (reader1.Read())
                                        {
                                            string Consult = reader1[date].ToString();
                                            int h = Consult.IndexOf('(');
                                            string ff = Consult.Substring(h + 1, 6);
                                            string fff = ff.Replace(':', '.');
                                            double r = Convert.ToDouble(fff);
                                            if (r > time)
                                            {
                                                string gh = r.ToString("00.00");
                                                using (SqlCommand nayan = dr.GetCommand("update dbo.faculty_status set available_from='" + gh + "' where dbo.faculty_status.username='" + x + "'"))
                                                { 
                                                    nayan.Connection.Open();
                                                    nayan.ExecuteNonQuery();
                                                    nayan.Connection.Close();
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    newcmd.Connection.Close();
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                cmd.Connection.Close();
            }
        }
        public bool InsertFileByUsername(string filename,string username)
        {
            FileInfo fi = new FileInfo(filename);
            byte[] document = File.ReadAllBytes(filename);

            string name = fi.Name;
            string ext = fi.Extension;

                SqlDbDataAccess dr = new SqlDbDataAccess();
                SqlCommand cmd = dr.GetCommand("savedocument");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@username", SqlDbType.VarChar, 10).Value = username;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = name;
                cmd.Parameters.Add("@content", SqlDbType.VarBinary, 5000).Value = document;
                cmd.Parameters.Add("@extn", SqlDbType.VarChar, 5).Value = ext;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return true;
        }


    }

}
