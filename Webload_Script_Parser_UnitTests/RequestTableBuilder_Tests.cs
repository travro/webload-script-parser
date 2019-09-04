using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WLScriptParser.Utilities;
using WLScriptParser.Models;
using WLScriptParser.Models.Repositories;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class RequestTableBuilder_Tests
    {
        //arrange
        List<Request> r1;
        List<Request> r2;
        Request[,] arr;

        //act
        public RequestTableBuilder_Tests()
        {
            string leftScript = ConfigurationData.FilePath1 + ConfigurationData.FileName1 + ConfigurationData.Extension;
            string rightScript = ConfigurationData.FilePath2 + ConfigurationData.FileName2 + ConfigurationData.Extension;
            ScriptRepository.Create(leftScript, rightScript);
            //r1 = ScriptRepository.Repository.ScriptLeft.Transactions.SelectMany(t => t.Requests);
            r1 = ScriptRepository.Repository.ScriptLeft.Transactions[1].Requests;
            r2 = ScriptRepository.Repository.ScriptRight.Transactions[1].Requests;
            arr = RequestTableBuilder.GetRequestTable(r1, r2);
        }

        //asserts
        [TestMethod]
        public void RequestTableBuilder_SampleRepo_NotNull()
        {
            Assert.IsNotNull(ScriptRepository.Repository);
        }
        [TestMethod]
        public void RequestTableBuilder_SampleRequests1_NotNull()
        {
            Assert.IsNotNull(r1);
        }
        [TestMethod]
        public void RequestTableBuilder_SampleRequests2_NotNull()
        {
            Assert.IsNotNull(r2);
        }
        [TestMethod]
        public void RequestTableBuilder_SampleRequestsBoth_NotEmpty()
        {
            Assert.IsTrue(r1.Count() > 0);
            Assert.IsTrue(r2.Count() > 0);
        }
        [TestMethod]
        public void RequestTableBuilder_GetRequestTable_NotNull()
        {
            Assert.IsNotNull(RequestTableBuilder.GetRequestTable(r1, r2));
        }
        [TestMethod]
        public void RequestTableBuilder_GetRequestTable_NotEmpty()
        {
            Assert.IsTrue(arr.Length > 0);

            for(int i = 0; i < arr.GetLength(0); i++)
            {
                Assert.IsTrue(arr[i, 0].Parameters != "ERROR");
                Assert.IsTrue(arr[i, 1].Parameters != "ERROR");
            }
        }
        [TestMethod]
        public void RequestTableBuilder_ElementsOfTableFirstRow_EqualsFirstRequestsOfEachList()
        {

            Assert.AreEqual(arr[0, 0].GetRequestString(), r1[0].GetRequestString());
            //Assert.AreEqual(arr[0, 1].GetRequestString(), r2[0].GetRequestString());
        }
    }
}
