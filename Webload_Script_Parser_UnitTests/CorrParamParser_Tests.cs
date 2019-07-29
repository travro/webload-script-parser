using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Webload_Script_Parser_WPF.Parsers;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class CorrParamParser_Tests
    {
        string correlation = "setCorrelationValue(\"corr_New2_r5___VIEWSTATEGENERATOR_3\", GetElementValueByName(\"__VIEWSTATEGENERATOR\"), \"D6B05CC4\");";
       // string correlation2 = "setCorrelationValue(\"corr_WSFederation_wresult_7\", decodeXML(GetElementValueByName(\"wresult\")), \"&lt;trust:RequestSecurityTokenResponseCollection xmlns:trust=\"httpl://docs.oasis-open.org/ws-sx/ws-trust/200512\"&gt;&lt;trust:RequestSecurityTokenResponse&gt;&lt;trust:Lifetime&gt;&lt;wsu:Created xmlns:wsu=\"http://docs.oas...\");";

        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsString()
        {
            Assert.IsInstanceOfType(CorrelationParamParser.Parse(correlation, CorrelationParamParser.Ordinal.Third), typeof(string));
        }

        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsCorrectValue()
        {
            Assert.AreEqual(CorrelationParamParser.Parse(correlation, CorrelationParamParser.Ordinal.Third), "\"D6B05CC4\"");
        }
    }
}
