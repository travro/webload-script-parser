using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using WLScriptParser.Models;
using System;

namespace WLScriptParser.Parsers
{
    public static class TransactionBlockParser
    {
        //Given a filepath and repo, will populate repo with Transaction objects
        public static void Parse(string path, TransactionRepository repo)
        {
            try
            {
                using (FileStream _fStream = new FileStream(path, FileMode.Open))
                {
                    using (XmlReader _xReader = XmlReader.Create(_fStream))
                    {
                        XDocument _XDoc = XDocument.Load(_xReader);

                        //Filter out Webload Sleeptime, JSObjects, and End Trasactions
                        var jsParentBlockElements = _XDoc
                            .Descendants("Agenda")
                            .Elements("JavaScriptObject")
                            .Where(element => element.Element("Properties")
                            .Element("PropertyPage")
                            .Element("ItemName")
                            .Value
                            .Contains("BeginTransaction::"));

                        //Add each XElement to the repo as a Transaction object
                        foreach (var jsPBE in jsParentBlockElements)
                        {
                            repo.AddTransaction(new Transaction(jsPBE));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
