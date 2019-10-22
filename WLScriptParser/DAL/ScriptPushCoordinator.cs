using System;
using WLScriptParser.Models;
using WLScriptParser.Utilities;

namespace WLScriptParser.DAL
{
    public class ScriptPushCoordinator
    {
        public int TestId { get; private set; }
        public string TestName { get; set; }
        public string BuildVersion { get; set; }
        public string ScriptName { get; set; }
        public DateTime RecordedDate { get; set; }

        public ScriptPushCoordinator() { }

        public ScriptPushCoordinator(string testName, string buildVers, string scriptName, DateTime dt)
        {
            TestName = testName; BuildVersion = buildVers; ScriptName = scriptName; RecordedDate = dt;
        }

        public void Push(Script script)
        {
            using (var sqlCnn = SqlConnectionManager.GetOpenConnection())
            {
                //Get Test Id
                TestId = SqlAPI.GetTestId(TestName, BuildVersion, sqlCnn);
                if (TestId == -1)
                {
                    TestId = SqlAPI.PushNewTest(TestName, BuildVersion, sqlCnn);
                    AppLogger.Logger.LogMessage($"----Pushing new test {TestName} to database----");
                }
                else
                {
                    AppLogger.Logger.LogMessage($"----Test name {TestName} found in database----");
                }


                //Begin pushing script
                using (var sqlTrn = sqlCnn.BeginTransaction())
                {
                    try
                    {
                        AppLogger.Logger.LogMessage($"----Pushing {ScriptName} to database---");
                        SqlAPI.PushNewScript(script, ScriptName, RecordedDate, TestId, sqlCnn, sqlTrn);    
                        sqlTrn.Commit();
                        AppLogger.Logger.LogMessage($"----Successfully pushed {ScriptName} to database---");
                    }
                    catch (Exception)
                    {
                        AppLogger.Logger.LogMessage($"----Error loging script | Transaction rolled back -----");
                        sqlTrn.Rollback();
                        throw;
                    }
                    finally
                    {
                        sqlCnn.Close();
                    }
                }
            }
        }
    }
}
