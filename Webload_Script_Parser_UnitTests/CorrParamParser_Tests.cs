using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using WLScriptParser.Parsers;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class CorrParamParser_Tests
    {
        //string correlation = "setCorrelationValue(\"corr_New2_r5___VIEWSTATEGENERATOR_3\", GetElementValueByName(\"__VIEWSTATEGENERATOR\"), \"D6B05CC4\");";
       //string correlation2 = "setCorrelationValue(\"corr_WSFederation_wresult_7\", decodeXML(GetElementValueByName(\"wresult\")), \"&lt;trust:RequestSecurityTokenResponseCollection xmlns:trust=\"httpl://docs.oasis-open.org/ws-sx/ws-trust/200512\"&gt;&lt;trust:RequestSecurityTokenResponse&gt;&lt;trust:Lifetime&gt;&lt;wsu:Created xmlns:wsu=\"http://docs.oas...\");";
       string correlation3 = "setCorrelationValue(\"corr_New16_r2_IsHybridOrNativeClient_3\", extractValue(\";IsHybridOrNativeClient=\", \"&amp;\", document.wlSource, 1), \"False\")";



        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsString()
        {
            Assert.IsInstanceOfType(CorrelationParamParser.Parse(correlation3, CorrelationParamParser.Ordinal.Second), typeof(string));
        }

        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsCorrectValue()
        {
            Assert.AreEqual(CorrelationParamParser.Parse(correlation3, CorrelationParamParser.Ordinal.Second), "extractValue(\";IsHybridOrNativeClient=\",\"&amp;\",document.wlSource,1)");
        }
    }
}
