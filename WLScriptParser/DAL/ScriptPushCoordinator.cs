using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Models;

namespace WLScriptParser.DAL
{
    public class ScriptPushCoordinator
    {
        public int TestId { get; private set; }
        public string TestName { get; set; }
        public string BuildVersion { get; set; }
        public int ScriptId { get; private set; }
        public string ScriptName { get; set; }
        public DateTime RecordedDate { get; set; }

        public ScriptPushCoordinator() { }

        public ScriptPushCoordinator(string testName, string buildVers, string scriptName, DateTime dt)
        {
            TestName = testName; BuildVersion = buildVers; ScriptName = scriptName; RecordedDate = dt;
        }

        public void Push(Script script)
        {
            int TestId = SqlAPI.GetTestId(TestName, BuildVersion);

            if (TestId == -1) TestId = SqlAPI.PushNewTest(TestName, BuildVersion);
            try
            {
                SqlAPI.PushNewScript(script, ScriptName, RecordedDate, TestId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
