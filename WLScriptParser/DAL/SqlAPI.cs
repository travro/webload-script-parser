using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WLScriptParser.Models;

namespace WLScriptParser.DAL
{
    public static class SqlAPI
    {
        /**
         * Returns: Collection of Test names
         * Params:
         *      attribute: the Script attribute that will determine the column of values to be returned
         *          default: TestNames
         * */
        public static ObservableCollection<string> GetTestCollections(ScriptAttribute attribute)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();


            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;

                    switch (attribute)
                    {
                        case ScriptAttribute.TestNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM Tests"; ; break;
                        case ScriptAttribute.BuildNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT build_version FROM Tests"; ; break;
                        default: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM TESTS"; ; break;
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
                cnn.Close();
            }
            return list;
        }
        /**
         * Populates script collections given as parameters
         * Params:
         *  scriptNames: the observable string collection of script names to be populated
         *  scriptDates: the observable string collection of recorded dates to be populated
         *  testName: the name parameter for retrieving a test id
         *  buildVersion: the build parameter for retrieving a test id
         * 
         * **/
        public static void GetScriptCollections(ObservableCollection<string> scriptNames, ObservableCollection<DateTime> scriptDates, string testName, string buildVersion)
        {
            /**
            DataTable scriptTable = new DataTable();
            scriptTable.Columns.Add(new DataColumn("script_name", typeof(string)));
            scriptTable.Columns.Add(new DataColumn("recording_date", typeof(DateTime)));            

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT script_name, recording_date FROM Scripts WHERE test_id = " +
                    "(SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion)");

                cmd.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                    new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.Date, Value = buildVersion}
                });

                var sqlDataAdapter = new SqlDataAdapter(cmd.CommandText, cnn);

                sqlDataAdapter.Fill(scriptTable);                                               
            }
            */
            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "SELECT script_name, recording_date FROM Scripts WHERE test_id = " +
                        "(SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion)";

                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });



                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scriptNames.Add(reader.GetString(0));
                            scriptDates.Add(reader.GetDateTime(1));
                        }
                    }
                }
                cnn.Close();
            }
        }
        /**
         * Returns: Id column value from a test record
         * Params:
         *  testName: the name parameter for the SQL query
         *  buildVersion: the build paraemter for the SQL query
         * **/
        public static int GetTestId(string testName, string buildVersion)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "SELECT test_id FROM Scripts WHERE test_id = " +
                        "(SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion)";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });
                    id = cmd.ExecuteScalar();
                }
                cnn.Close();
            }
            return (id != null) ? (Int32)id : -1;
        }

        public static int PushNewTest(string testName, string buildVersion)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "INSERT INTO Tests values (@testName, @buildVersion)" +
                        "SELECT CONVERT(int, SCOPE_IDENTITY())";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });

                    id = cmd.ExecuteScalar();
                }
                cnn.Close();
            }
            return (id != null) ? (int)id : -1;
        }

        public static int PushNewScript(string scriptName, DateTime dt, int testId)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "INSERT INTO Scripts values (@scriptName, @recordingDate, @testId)" +
                        "SELECT CONVERT(int, SCOPE_IDENTITY())";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@scriptName", SqlDbType = SqlDbType.NVarChar, Value = scriptName},
                            new SqlParameter(){ ParameterName = "@recordingDate", SqlDbType = SqlDbType.Date, Value = dt},
                            new SqlParameter(){ ParameterName = "@testId", SqlDbType = SqlDbType.Int, Value = testId }
                    });

                    id = cmd.ExecuteScalar();
                }
                cnn.Close();
            }
            return (id != null) ? (int)id : -1;
        }
    }
}
