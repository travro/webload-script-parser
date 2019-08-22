using System.Collections.Generic;

namespace WLScriptParser.Models
{
    public class TransactionRepository
    {
        public List<Transaction> _transactions;

        public Transaction[] Transactions => _transactions.ToArray();

        public TransactionRepository()
        {
            _transactions= new List<Transaction>();
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
