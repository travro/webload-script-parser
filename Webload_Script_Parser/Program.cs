using System;
using System.Collections;
using System.Xml.Linq;
using System.Linq;

namespace Webload_Script_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "TM_PRM_Journal_Reply_24May19" + ".wlp";
            string path = $@"C:\Users\tsmelvin\Documents\WebLOAD\Sessions\{name}";

            Console.WriteLine($"{name}\n\n---------------------\n");
            XDocument project = XDocument.Load(path);
            //TransactionRepository repo = new TransactionRepository();

           

            var transactionBlocks = from transTable in project.Descendants("JavaScriptObject")
                                    where transTable
                                    .Element("Properties")
                                    .Element("PropertyPage")
                                    .Element("ItemName")
                                    .Value.Contains("Begin")
                                    select transTable;
                       

            foreach (var t in transactionBlocks)
            {
                Console.WriteLine(t.Element("Properties").Element("PropertyPage").Element("ItemName").Value);

                foreach (var h in t.Descendants("HeaderItem").Where(headerItem => headerItem.Parent.Name == "PropertyPage"))
                {
                    Console.WriteLine("-- " + (h.Attribute("Text").Value));
                }

            }  
            
        }
    }
}
