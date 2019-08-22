using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WLScriptParser.Models;

namespace WLScriptParser.DAL
{
    public sealed class SqlAPI
    {
        private static SqlAPI api = null;
        private SqlConnection cnn = null;
        private SqlCommand cmd = null;

        private ObservableCollection<string> _testNames;
        private ObservableCollection<string> _testBuilds;
        private ObservableCollection<string> _scenarioNames;
        private ObservableCollection<DateTime> _scenarioDates;

        public static SqlAPI API
        {
            get
            {
                if(api == null)
                {
                    api = new SqlAPI();
                }
                return api;
            }
        }

        private SqlAPI()
        {

        }

        private ObservableCollection<string> QueryAttributes(ScriptAttribute attribute)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            //"USE WLScriptsDB SELECT DISTINCT [@item] FROM dbo.[TESTS] ORDER BY [@item]"
            string column = "";
            string table = "";

            switch (attribute)
            {
                case ScriptAttribute.TestNames: column = "NAME"; table = "TESTS"; break;
                case ScriptAttribute.BuildNames: column = "BUILD"; table = "TESTS"; break;
                case ScriptAttribute.ScenarioNames: column = "NAME"; table = "SCENARIOS"; break;
                case ScriptAttribute.ScenarioDates: column = "DATE"; table = "SCENARIOS"; break;
                default: column = "NAME"; table = "TESTS"; break;
            }

            using (cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT [@column] FROM dbo.[@table] ORDER BY [@column]";
                    cmd.Parameters.AddWithValue("@column", column);


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

    }
}
