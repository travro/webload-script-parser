using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileStream_Practice;

namespace FileStream_Test
{
    [TestClass]
    public class UnitTest1
    {
        string url = "wlHttp.Get(\"https://\"+pURL_Webserver.getValue()+\"/Core/Session/api/sessionapi/GetExtendSessionPopupTimer\")";
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
            Request.RequestVerb verb1 = Request.RequestVerb.GET;

            Request.RequestVerb verb2 = TransactionParser.ParseRequestVerb(url);

            Assert.AreEqual(verb1, verb2);
        }

        [TestMethod]
        public void ParseRequestParametersTest()
        {
            string parameters1 = "/Core/Session/api/sessionapi/GetExtendSessionPopupTimer";

            string parameters2 = TransactionParser.ParseRequestParameters(url);

            Assert.AreEqual(parameters1, parameters2);
        }
    }
}
