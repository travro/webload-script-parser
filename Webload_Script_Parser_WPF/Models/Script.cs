using System.Collections.Generic;

namespace WLScriptParser.Models
{
    public class Script
    {
        private List<Transaction> _transactions;

        public Transaction[] Transactions => _transactions.ToArray();
        public string FileName { get; set; }

        public Script()
        {
            _transactions= new List<Transaction>();
        }
        public Script(string fileName)
        {
            FileName = fileName;
            _transactions = new List<Transaction>();
        }
        public void AddTransaction(Transaction t)
        {
            _transactions.Add(t);
        }

        public bool Contains(Transaction t)
        {
            return _transactions.Exists(element => element.Name == t.Name);
        }
        public bool Contains(string s)
        {
            return _transactions.Exists(element => element.Name == s);
        }
    }
}
