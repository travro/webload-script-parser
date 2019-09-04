using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WLScriptParser.Models;
using WLScriptParser.Parsers;
using WLScriptParser.Utilities;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class ScriptTransactionsComparer_Test
    {
        string filepath1 = ConfigurationData.FilePath1;
        string fileName1 = ConfigurationData.FileName1;
        string filepath2 = ConfigurationData.FilePath2;
        string fileName2 = ConfigurationData.FileName2;
        string extension = ConfigurationData.Extension;

        [TestMethod]
        public void CompareCounrt_GivenTwoSimilarScripts_ReturnsTrue()
        {
            

            Script script1 = ScriptTransactionParser.Parse(filepath1 + fileName1 + extension);
            Script script2 = ScriptTransactionParser.Parse(filepath2 + fileName2 + extension);

            Assert.IsTrue(ScriptTransactionsComparer.CompareCount(script1, script2));

        }
        [TestMethod]
        public void CompareEach_GivenTwoSimilarScripts_ReturnsTrue()
        {
            Script script1 = ScriptTransactionParser.Parse(filepath1 + fileName1 + extension);
            Script script2 = ScriptTransactionParser.Parse(filepath2 + fileName2 + extension);

            Assert.IsTrue(ScriptTransactionsComparer.CompareEach(script1, script2));

        }
    }
}
