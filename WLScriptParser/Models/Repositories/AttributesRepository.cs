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

            // _testNames = new List<string>();            
            // _testBuilds = new List<string>();
            _testNames = SqlAPI.QueryAttributes(ScriptAttribute.TestNames);
            _testBuilds = SqlAPI.QueryAttributes(ScriptAttribute.BuildNames);

        }
    }
}

