using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WLScriptParser.DAL
{
    public static class SqlConnectionManager
    {
        public static SqlConnection GetOpenConnection()
        {
            var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
