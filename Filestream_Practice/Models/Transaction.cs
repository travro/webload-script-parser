using System;
using System.Collections.Generic;
using System.Text;

namespace FileStream_Practice
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
