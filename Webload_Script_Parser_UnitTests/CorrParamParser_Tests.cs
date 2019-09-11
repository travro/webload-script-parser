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
       //string correlation3 = "setCorrelationValue(\"corr_New16_r2_IsHybridOrNativeClient_3\", extractValue(\";IsHybridOrNativeClient=\", \"&amp;\", document.wlSource, 1), \"False\")";
        string corr4 = "setCorrelationValue(\"corr_New20_r16_PackageItemId_81\", extractValue( \"rmChangeNextPackage('\", \"'\", document.wlSource, 2), \"E92190CB-3703-4576-A652-CA3F59AE0827\");";


        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsString()
        {
            Assert.IsInstanceOfType(CorrelationParamParser.Parse(corr4, CorrelationParamParser.Ordinal.First), typeof(string));
        }

        [TestMethod]
        public void CorrelationParamParser_GivenLine_ReturnsCorrectValue()
        {
            Assert.AreEqual(CorrelationParamParser.Parse(corr4, CorrelationParamParser.Ordinal.Third), "---");
        }

        private XElement AssignNodeScript(XElement element, string filepath)
        {
            using (FileStream _fStream = new FileStream(filepath, FileMode.Open))
            {
                using (XmlReader _xReader = XmlReader.Create(_fStream))
                {
                    XDocument _xDoc = XDocument.Load(_xReader);

                    var jsParentBlockElements = _xDoc
                        .Descendants("Agenda")
                        .Elements("JavaScriptObject")
                        .Where(el => el.Element("Properties")
                        .Element("PropertyPage")
                        .Element("ItemName")
                        .Value
                        .Contains("BeginTransaction::"));

                    var jsChildBlockElements = jsParentBlockElements.Elements("JavaScriptObject");

                    element = jsChildBlockElements.Descendants("PropertyPage")
                       .Where(desc => desc.Attribute("Name").Value == "JavaScript")
                       .First()
                       .Element("NodeScript");

                }
            }
            return element;
        }
    }
}
