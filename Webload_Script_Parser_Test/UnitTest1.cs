using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileStream_Practice;

namespace FileStream_Test
{
    [TestClass]
    public class UnitTest1
    {
        string url1 = "wlHttp.Get(\"https://\"+pURL_Webserver.getValue()+\"/Core/Session/api/sessionapi/GetExtendSessionPopupTimer\")";
        string url2 = "wlHttp.Get(\"https://pr10lm.sumtotaldevelopment.net/core/Core/Session/api/sessionapi/GetExtendSessionPopupTimer\")";
        string urlTemp = "wlHttp.Post(wlTemporary)";

        [TestMethod]
        public void ParseTransactionNameTest()
        {
            string str1 = "05_Goals_02_LoginPost";
            string str2 = TransactionParser.ParseTransactionName("BeginTransaction(\"05_Goals_02_LoginPost\")");

            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void ParseRequestVerbTest()
        {
            Request.RequestVerb verbTest = Request.RequestVerb.GET;
            Request.RequestVerb verb1 = TransactionParser.ParseRequestVerb(url1);
            Request.RequestVerb verb2 = TransactionParser.ParseRequestVerb(url2);

            Assert.AreEqual(verbTest, verb1);
            Assert.AreEqual(verbTest, verb2);
        }

        [TestMethod]
        public void ParseRequestParametersTest()
        {
            string parametersTest = "/Core/Session/api/sessionapi/GetExtendSessionPopupTimer";
            string parameters1 = TransactionParser.ParseRequestParameters(url1);
            string parameters2 = TransactionParser.ParseRequestParameters(url2);

            Assert.AreEqual(parametersTest, parameters1);
            Assert.AreEqual(parametersTest, parameters2);
        }
    }
}
