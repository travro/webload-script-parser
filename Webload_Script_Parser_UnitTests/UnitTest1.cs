using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Webload_Script_Parser_WPF.Models;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        string path = "C:\\Users\\tsmelvin\\Documents\\WebLOAD\\Sessions\\Scripts_XML_Formatted\\LM_PRM_191_AdminViewRoster_Feb06.wlp";
        XElement nodeScript;

        [TestMethod]
        public void XElement_AssignedThroughFileStream_IsNotNull()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsNotNull(nodeScript);
        }
        [TestMethod]
        public void XElement_AssignedThroughFileStream_NameIsNodeScript()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.AreEqual(nodeScript.Name, "NodeScript");
        }
        [TestMethod]
        public void XElement_AssignedThroughFileStream_IsTypeOfString()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsInstanceOfType(nodeScript.Value, typeof(string));
        }
        [TestMethod]
        public void XElement_AssignedThroughFileStream_ContainsSetCorrelation()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsTrue(nodeScript.Value.Contains("setCorrelation"));
        }
        [TestMethod]
        public void CorrelationFactory_GivenElement_IsNotNull()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsNotNull(CorrelationFactory.GetCorrelations(nodeScript));
        }
        [TestMethod]
        public void CorrelationFactory_GivenElement_ReturnsCorrelationArrayType()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsInstanceOfType(CorrelationFactory.GetCorrelations(nodeScript), typeof(Correlation[]));
        }

        [TestMethod]
        public void CorrelationFactory_GivenElement_ReturnsCorrelationArray_NonEmpty()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsTrue(CorrelationFactory.GetCorrelations(nodeScript).Length > 0);
        }

        [TestMethod]
        public void CorrelationFactory_GivenElement_FirstElementContainsCorr()
        {
            nodeScript = AssignNodeScript(nodeScript, path);
            Assert.IsTrue(CorrelationFactory.GetCorrelations(nodeScript)[0].Name.Contains("corr"));
        }




        //
        //Assert.IsTrue(CorrelationFactory.GetCorrelations(nodeScript).Count > 0);

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
