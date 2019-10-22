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
