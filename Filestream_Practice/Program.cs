using System;
using System.IO;
using System.Text;


namespace FileStream_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "TM_PRM_Provide_Feedback_24May19" + ".wlp";
            string path = $@"C:\Users\tsmelvin\Documents\WebLOAD\Sessions\{name}";

            Console.WriteLine($"{name}\n\n---------------------\n");

            StreamReader strmRdr = new StreamReader(path, Encoding.UTF8, true);
            TransactionRepository repo = new TransactionRepository();
            TransactionParser.Parse(strmRdr, repo);

            foreach (Transaction t in repo.Transactions)
            {
                string output = $"Transaction: {t.Name} has the following requests:";
               

                if (t.Requests != null)
                {
                    Console.WriteLine(output);
                    foreach (Request r in t.Requests)
                    {
                        Console.WriteLine($" - {r.Verb.ToString()}: {r.Parameters}");
                    }
                }
                else
                {
                    Console.WriteLine(output + " none");
                }
                Console.WriteLine();                
            }
        }
    }
}
