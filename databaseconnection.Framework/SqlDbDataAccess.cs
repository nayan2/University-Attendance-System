using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace databaseconnection.Framework
{
    public class SqlDbDataAccess
    {
        const string ConnectionString = "Data Source=FUJITSU-PC\\SQLEXPRESS;Initial Catalog=attendance_system;Integrated Security=True";
        public SqlCommand GetCommand(String query)
        {
            var connection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = connection;
            return cmd;
        }
    }
}
