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
         * Called during AttributesRepository instantiation
         * */
        public static ObservableCollection<string> GetTestAttribrutes(ScriptAttribute attribute)
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
         * Called by AttributesRepository once TestNames and BuildNames (build version) is selected by user to fill remaining Observaable Collections
         * 
         * **/
        public static void FillScriptCollections(ObservableCollection<string> scriptNames, ObservableCollection<DateTime> scriptDates, string testName, string buildVersion)
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
                    cmd.CommandText = "SELECT script_name, recording_date FROM Scripts WHERE test_id = " +
                        "(SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion)";

                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });

                    cmd.Connection = cnn;

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
    }
}
