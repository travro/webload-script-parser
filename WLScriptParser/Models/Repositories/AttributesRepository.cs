using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WLScriptParser.DAL;

namespace WLScriptParser.Models.Repositories
{
    public sealed class AttributesRepository
    {
        private static AttributesRepository repo = null;
        private ObservableCollection<string> _testNames;
        private ObservableCollection<string> _testBuilds;
        private ObservableCollection<string> _scriptNames;
        public string[] TestNames => _testNames.ToArray();
        public string[] TestBuilds => _testBuilds.ToArray();
        public string[] ScriptNames => _scriptNames.ToArray();
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
                    repo.Update();
                }
                return repo;
            }
        }
        private AttributesRepository() { }

        public void BuildScriptCollection(string testName)
        {
            using (var sqlConnection = SqlConnectionManager.GetOpenConnection())
            {
                _scriptNames = SqlCommands.GetScriptCollection(testName, sqlConnection);
            }
        }

        public void Update()
        {
            using (var sqlConnection = SqlConnectionManager.GetOpenConnection())
            {
                _testNames = SqlCommands.GetTestCollections(ScriptAttribute.TestNames, sqlConnection);
                _testBuilds = SqlCommands.GetTestCollections(ScriptAttribute.BuildNames, sqlConnection);
            }
        }
    }
}

