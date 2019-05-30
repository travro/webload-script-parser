using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using Webload_Script_Parser.Models;

namespace Webload_Script_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "LM_PRM_192_EcomReg_Apr22";
            string path = $@"C:\Users\tsmelvin\Documents\WebLOAD\Sessions\{name}" + ".wlp";

            Console.WriteLine($"{name}\n\n---------------------\n");

            TransactionRepository repo = new TransactionRepository();
            TransactionBlockParser.Parse(path, repo);

            foreach (Transaction t in repo.Transactions)
            {
                Console.WriteLine(t.Name + "\n");
                foreach (Request r in t.Requests)
                {
                    Console.WriteLine("-- " + r.Verb.ToString() + "  " + r.Parameters);
                }
                Console.WriteLine("\n--------------------\n");
            }
        }
    }
}
