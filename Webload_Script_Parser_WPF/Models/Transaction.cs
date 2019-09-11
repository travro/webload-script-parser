using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace WLScriptParser.Models
{
    public class Transaction
    {
        public List<Request> Requests { get; }
        //public Request[] Requests => _requests.ToArray();
        public string Name { get; set; }
        public Transaction(string name)
        {
            Name = name;
            Requests = new List<Request>();
        }
        public Transaction(XElement element)
        {
            Name = ParseTransactionName(element);
            Requests = new List<Request>();

            var jsChildBlockElements = element.Elements("JavaScriptObject");

            foreach (var jsCBE in jsChildBlockElements)
            {
                XElement nodeScriptElement = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "JavaScript")
                    .First()
                    .Element("PersistentCorrelation")?? new XElement("PersistentCorrelation");

                IEnumerable<XElement> httpHeaderElements = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "HTTPHeaders")
                    .Elements()?? new List<XElement>();

                //Transaction will navigate the childblock elements and attach the nodescript of any correlation onto the first httpHeaderElement using the overloaded
                //Request constructor
                //The first request is the visible request shown in the actual .wlp file
                foreach (var httpHeaderElement in httpHeaderElements)
                {
                    if (httpHeaderElement == httpHeaderElements.First() && nodeScriptElement.Value.Contains("setCorr"))
                    {
                        Requests.Add(new Request(httpHeaderElement, nodeScriptElement, true));
                    }
                    else if(httpHeaderElement == httpHeaderElements.First())
                    {
                        Requests.Add(new Request(httpHeaderElement, true));
                    }
                    else
                    {
                        Requests.Add(new Request(httpHeaderElement, false));
                    }                    
                }
            }
        }
        public bool Equals(Transaction t)
        {
            return this.Name == t.Name;
        }
        private string ParseTransactionName(XElement element)
        {
            string itemName = element
                .Element("Properties")
                .Element("PropertyPage")
                .Element("ItemName").Value;
            string beginTrans = "BeginTransaction::";

            if (itemName.Contains(beginTrans))
            {
                return itemName.Substring(itemName.IndexOf(beginTrans) + beginTrans.Length);
            }
            else return "Transaction";
        }
    }
}
