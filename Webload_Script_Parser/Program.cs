using System;
using System.Xml.Linq;
using System.Linq;

namespace Webload_Script_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "TM_PRM_Provide_Feedback_24May19" + ".wlp";
            string path = $@"C:\Users\tsmelvin\Documents\WebLOAD\Sessions\{name}";

            Console.WriteLine($"{name}\n\n---------------------\n");
            XDocument project = XDocument.Load(path);            

            var transNames = from transactions in project.Descendants("ItemName")
                             where transactions.Value.Contains("BeginTransaction")
                             select transactions;
                             

            foreach(XElement t in transNames)
            {
                Console.WriteLine(t.Value);
            }            
            

            //TransactionRepository repo = new TransactionRepository();


        }


    }
}
