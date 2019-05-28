using System;
using System.Collections.Generic;
using System.Text;

namespace Webload_Script_Parser
{
    public class Transaction
    {
        public List<Request> Requests { get;}
        public string Name { get; set; }

        public Transaction(string name)
        {
            Name = name;
            Requests = new List<Request>();
        }

        public void AddRequest(Request r)
        {
            Requests.Add(r);            
        }

        public bool Equals(Transaction t)
        {
            return this.Name == t.Name;
        }
    }

    
}
