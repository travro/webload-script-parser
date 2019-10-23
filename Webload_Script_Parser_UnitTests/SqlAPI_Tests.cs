using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WLScriptParser.DAL;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class SqlAPI_Tests
    {
        [TestMethod]
        public void SqlAPI_TestCollections_IsNotNull()
        {
            Assert.IsNotNull(SqlCommands.GetTestCollections(WLScriptParser.Models.ScriptAttribute.TestNames));
            Assert.IsNotNull(SqlCommands.GetTestCollections(WLScriptParser.Models.ScriptAttribute.BuildNames));
        }

        [TestMethod]
        public void SqlAPI_TestID_ReturnsCorrectValue()
        {
            Assert.AreEqual(1, SqlCommands.GetTestId("LM", "19.2.0.0-88"));
        }

        //[TestMethod]
        //public void SqlAPI_PushTest_ReturnsCorrectScopeId()
        //{
        //    Assert.AreEqual(11, SqlAPI.PushNewTest("LM", "19.3.0.0-120"));
        //}

        //[TestMethod]
        //public void SqlAPI_PushScript_ReturnsCorrectScopeId()
        //{
        //    Assert.AreEqual(35, SqlAPI.PushNewScript("AdminGenTask", DateTime.Today.Date, 4));
        //}
    }
}
