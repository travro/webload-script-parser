using System.Collections.Generic;

namespace WLScriptParser.Models
{
    public class Script
    {
        public List<Transaction> Transactions { get; }
        //public Transaction[] Transactions => _transactions.ToArray();
        public string FileName { get; set; }

        public Script()
        {
            Transactions= new List<Transaction>();
        }
        public Script(string fileName)
        {
            FileName = fileName;
            Transactions = new List<Transaction>();
        }
        //public void AddTransaction(Transaction t)
        //{
        //    Transactions.Add(t);
        //}
        public bool Contains(Transaction t)
        {
            return Transactions.Exists(element => element.Name == t.Name);
        }
        public bool Contains(string s)
        {
            return Transactions.Exists(element => element.Name == s);
        }
    }
}
