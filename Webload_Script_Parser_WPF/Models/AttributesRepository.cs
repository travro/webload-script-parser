using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WLScriptParser.DAL;

namespace WLScriptParser.Models
{
    public sealed class AttributesRepository
    {
        private static AttributesRepository repo = null;
        private List<string> _testNames;
        private List<string> _testBuilds;
        public string[] TestNames => _testNames.ToArray();
        public string[] TestBuilds => _testBuilds.ToArray();
        //private  List<object> _scenarioNames;
        //private  List<object> _scenarioDates;
        public static AttributesRepository Repository
        {
            get
            {
                if (repo == null)
                {
                    //lazy loading here
                    repo = new AttributesRepository();
                }
                return repo;
            }
        }
        private AttributesRepository()
        {
            
            _testNames = new List<string>();
            _testBuilds = new List<string>();

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand("USE WLScriptsDB SELECT DISTINCT [NAME] FROM dbo.[TESTS] ORDER BY [NAME]", cnn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _testNames.Add(reader.GetString(0));
                        }
                    }
                }
                cnn.Close();
                cnn.Dispose();
            }

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand("USE WLScriptsDB SELECT DISTINCT [BUILD] FROM dbo.[TESTS] ORDER BY [BUILD]", cnn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _testBuilds.Add(reader.GetString(0));
                        }
                    }
                }
                cnn.Close();
                cnn.Dispose();
            }
        }
    }
}

