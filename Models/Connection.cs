using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechBank.Models
{
    class Connection
    {
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-BO24OOP; Initial Catalog=FintechBank; Integrated Security=True");
        
        public void openConnection()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            
        }

        public SqlConnection getConnection()
        {
            return connection;
        }
            
            
    }
}
