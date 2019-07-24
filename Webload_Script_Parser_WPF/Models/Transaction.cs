using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace Webload_Script_Parser_WPF.Models
{
    public class Transaction
    {
        private List<Request> _requests;
        public Request[] Requests => _requests.ToArray();
        public string Name { get; set; }
        public Transaction(string name)
        {
            Name = name;
            _requests = new List<Request>();
        }
        public Transaction(XElement element)
        {
            Name = ParseTransactionName(element);
            _requests = new List<Request>();

            var jsChildBlockElements = element.Elements("JavaScriptObject");

            foreach (var jsCBE in jsChildBlockElements)
            {
                XElement nodeScriptElement = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "JavaScript")
                    .First()
                    .Element("NodeScript")?? new XElement("NodeScript");

                IEnumerable<XElement> httpHeaderElements = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "HTTPHeaders")
                    .Elements()?? new List<XElement>();

                //Transaction will navigate the childblock elements and attach the nodescript of any correlation onto the first httpHeaderElement using the overloaded
                //Request constructor
                foreach (var httpHeaderElement in httpHeaderElements)
                {
                    if (httpHeaderElement == httpHeaderElements.First() && nodeScriptElement.Value.Contains("setCorr"))
                    {
                        _requests.Add(new Request(httpHeaderElement, nodeScriptElement));
                    }
                    _requests.Add(new Request(httpHeaderElement));
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
