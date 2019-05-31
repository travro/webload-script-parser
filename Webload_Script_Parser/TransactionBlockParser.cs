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
        private static string ParseTransactionName(XElement element)
        {
            string itemName = element.Element("Properties").Element("PropertyPage").Element("ItemName").Value;
            string beginTrans = "BeginTransaction::";
            if (itemName.Contains(beginTrans))
            {
                return itemName.Substring(itemName.IndexOf(beginTrans) + beginTrans.Length);
            }
            else return "Transaction";
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
        private static string ParseRequestParams(XElement element)
        {
            string val = element.Attribute("Text").Value;
            string sumTotalSite = "sumtotaldevelopment.net/";
            string domain = (val.Contains(sumTotalSite)) ? sumTotalSite : "https://";
            int domainIndex = val.IndexOf(domain) + domain.Length;
            int paramIndex = val.IndexOf(' ', domainIndex);

            return val.Substring(domainIndex, paramIndex - domainIndex);
        }

        //Given a filepath and repo, will populate repo with Transaction objects
        public static void Parse(string path, TransactionRepository repo)
        {
            FileStream _fStream = new FileStream(path, FileMode.Open);
            XmlReader _xReader = XmlReader.Create(_fStream);
            XDocument _XDoc = XDocument.Load(_xReader);

            //Get the JavaScriptObject XElements that are BeginTransaction blocks
            var jScriptElements = from transTable in _XDoc.Descendants("JavaScriptObject")
                                  where transTable
                                  .Element("Properties")
                                  .Element("PropertyPage")
                                  .Element("ItemName")
                                  .Value.Contains("BeginTransaction::")
                                  select transTable;

            //Add each XElement to the repo as a Transaction object
            foreach (var jSE in jScriptElements)
            {
                Transaction trans = new Transaction(ParseTransactionName(jSE));

                //Get the PropertyPage descendant elements that are of type HTTPHeader, which contain the request URLs
                var reqElements = jSE.Descendants("JavaScriptObject")
                    .Descendants()
                    .Where(desc => desc.Name == "PropertyPage" && desc.Attribute("Name").Value == "HTTPHeaders")
                    .Elements();

                foreach (var req in reqElements)
                {
                    trans.AddRequest(new Request(ParseRequestVerb(req), ParseRequestParams(req)));
                }
                repo.AddTransaction(trans);
            }
            _xReader.Dispose();
            _fStream.Dispose();
        }
    }
}
