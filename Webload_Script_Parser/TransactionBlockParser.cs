using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using Webload_Script_Parser.Models;

namespace Webload_Script_Parser
{
    public class TransactionBlockParser
    {
        //TO_ERASE?
        private static void ParseTransactionName(XElement element)
        {

        }
        private static Request.RequestVerb ParseRequestVerb(XElement element)
        {
            string val = element.Attribute("Text").Value;
            if (val.StartsWith(" DELETE")) return Request.RequestVerb.DELETE;
            if (val.StartsWith(" POST")) return Request.RequestVerb.POST;
            if (val.StartsWith(" PUT")) return Request.RequestVerb.PUT;
            if (val.StartsWith(" GET")) return Request.RequestVerb.GET;
            return Request.RequestVerb.GET;
        }
        //TODO
        private static string ParseRequestParams(XElement element)
        {
            string val = element.Attribute("Text").Value;
            string domain = "sumtotaldevelopment.net/";
            int uRlIndex = val.IndexOf(domain) + domain.Length;
            int limitIndex = val.IndexOfAny(new[] { '?',' '}, uRlIndex);

            return val.Substring(uRlIndex, limitIndex - uRlIndex);
        }

        public static void Parse(string path, TransactionRepository repo)
        {
            FileStream _fStream = new FileStream(path, FileMode.Open);
            XmlReader _xReader = XmlReader.Create(_fStream);
            XDocument _XDoc = XDocument.Load(_xReader);

            //Get all BeginTransaction JS objects as XElements
            var jScriptElements = from transTable in _XDoc.Descendants("JavaScriptObject")
                                  where transTable
                                  .Element("Properties")
                                  .Element("PropertyPage")
                                  .Element("ItemName")
                                  .Value.Contains("Begin")
                                  select transTable;

            foreach (var jSE in jScriptElements)
            {
                Transaction trans = new Transaction(jSE.Element("Properties").Element("PropertyPage").Element("ItemName").Value);

                var reqElements = jSE.Descendants("JavaScriptObject")
                    .Descendants()
                    .Where(desc => desc.Name == "PropertyPage" && desc.Attribute("Name").Value == "HTTPHeaders")
                    .Elements();

                foreach (var req in reqElements)
                {
                    //Console.WriteLine(req.Attribute("Text").Value);
                    trans.AddRequest(new Request(ParseRequestVerb(req), ParseRequestParams(req)));
                }
                repo.AddTransaction(trans);
            }
            _xReader.Dispose();
            _fStream.Dispose();
        }
    }
}
