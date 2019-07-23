using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webload_Script_Parser_WPF.Models;

namespace WebLoad_Script_Parser_Tests
{
    [TestClass]
    public class UnitTest1
    {
        string path = "C:\\Users\tsmelvin\\Documents\\WebLOAD\\Sessions\\LM_PRM_192_AdminGenTasks_03Jun.wlp";
        

        [TestMethod]
        public void GetCorrelations_GivenElement_ProducesCorrelationList()
        {

            XElement nodeScript;

            using (FileStream _fStream = new FileStream(path, FileMode.Open))
            {
                using (XmlReader _xReader = XmlReader.Create(_fStream))
                {
                    XDocument _xDoc = XDocument.Load(_xReader);

                    nodeScript = _xDoc.Descendants("NodeScript").First();
                }
            }

            Assert.IsNotNull(nodeScript);
            Assert.IsInstanceOfType(CorrelationFactory.GetCorrelations(nodeScript), typeof(List<Correlation>));
            Assert.IsTrue(CorrelationFactory.GetCorrelations(nodeScript).Count > 0);
        }
    }
}
