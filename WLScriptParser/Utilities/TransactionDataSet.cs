using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Models;
using System.Data;

namespace WLScriptParser.Utilities
{
    public class TransactionDataSet
    {
        Script _s1, _s2;
        string _s1Name, _s2Name;
        public DataSet DataSet { get; }

        public TransactionDataSet(Script s1, Script s2)
        {
            DataSet = new DataSet();
            _s1 = s1;
            _s1Name = s1.FileName.Substring(s1.FileName.LastIndexOf('\\') + 1);
            _s2 = s2;
            _s2Name = s2.FileName.Substring(s2.FileName.LastIndexOf('\\') + 1);


           // CreateFileNameTable();
            //CreateTransactionTables();
        }
        public bool CreateFileNameTable()
        {
            if (ScriptTransactionsComparer.CompareAll(_s1, _s2))
            {
                var fileNameTable = new DataTable()
                {
                    TableName = "FileNameTable"
                };
                fileNameTable.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn(_s1Name),
                    new DataColumn(_s2Name)
                });
                
                DataSet.Tables.Add(fileNameTable);
                return true;
            }
            else return false;
        }
        public bool CreateTransactionTables()
        {
            if (ScriptTransactionsComparer.CompareCount(_s1, _s2))
            {
                for(int i = 0; i < _s1.Transactions.Count; i++)
                {
                    DataTable table = new DataTable(_s1.Transactions[i].Name);

                    table.Columns.AddRange(new DataColumn[]
                    {
                    new DataColumn(){ ColumnName = _s1Name, DataType = typeof(Request)},                        
                    new DataColumn(){ ColumnName = _s2Name, DataType = typeof(Request)}
                    });

                    Request[,] requestsArr = RequestTableBuilder.GetRequestTable(_s1.Transactions[i].Requests.Where(r => r.Visible == true), _s2.Transactions[i].Requests.Where(r => r.Visible == true));

                    for (int j = 0; j < requestsArr.GetLength(0); j++)
                    {
                        DataRow row = table.NewRow();
                        row[0] = requestsArr[j, 0];
                        row[1] = requestsArr[j, 1];
                        table.Rows.Add(row);
                    }
                    DataSet.Tables.Add(table);                    
                }
                return true;
            }
            else return false;
        }
    }
}
