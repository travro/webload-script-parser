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
        private ObservableCollection<DateTime> _scriptDates;
        public string[] TestNames => _testNames.ToArray();
        public string[] TestBuilds => _testBuilds.ToArray();
        public string[] ScriptNames => _scriptNames.ToArray()?? new string[] { };
        public DateTime[] ScriptDates => _scriptDates.ToArray()?? new DateTime[] { };
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
            _testNames = SqlAPI.GetTestAttribrutes(ScriptAttribute.TestNames);
            _testBuilds = SqlAPI.GetTestAttribrutes(ScriptAttribute.BuildNames);
        }

        public void BuildScriptCollections(string testName, string buildVersion)
        {
            if (_scriptNames == null) _scriptNames = new ObservableCollection<string>(); else _scriptNames.Clear();
            if (_scriptDates == null) _scriptDates = new ObservableCollection<DateTime>(); else _scriptDates.Clear();

            SqlAPI.FillScriptCollections(_scriptNames, _scriptDates, testName, buildVersion);
        }
    }
}

