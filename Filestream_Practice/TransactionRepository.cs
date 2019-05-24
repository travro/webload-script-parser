using System.Collections.Generic;

namespace FileStream_Practice
{
    public class TransactionRepository
    {
        public List<Transaction> Transactions { get; }

        public TransactionRepository()
        {
            Transactions = new List<Transaction>();
        }
        public void AddTransaction(Transaction t)
        {
            Transactions.Add(t);
        }

        public bool Contains(Transaction t)
        {
            return this.Transactions.Exists(element => element.Name == t.Name);
        }
        public bool Contains(string s)
        {
            return this.Transactions.Exists(element => element.Name == s);
        }

    }
}
