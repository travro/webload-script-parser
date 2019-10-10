using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WLScriptParser.Models;

namespace WLScriptParser.DAL
{
    public static class SqlAPI
    {
        public static ObservableCollection<string> QueryAttributes(ScriptAttribute attribute)
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
                        case ScriptAttribute.ScenarioNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT script_name FROM Scripts"; ; break;
                        case ScriptAttribute.ScenarioDates: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT recording_date FROM Scripts"; ; break;
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
                cnn.Dispose();
            }
            return list;
        }

    }
}
