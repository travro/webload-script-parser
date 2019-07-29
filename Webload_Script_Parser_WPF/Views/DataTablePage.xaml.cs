using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Webload_Script_Parser_WPF.Models;
using System.Data;

namespace Webload_Script_Parser_WPF.Views
{
    /// <summary>
    /// Interaction logic for DataTablePage.xaml
    /// </summary>
    public partial class DataTablePage : Page
    {
        public DataTablePage()
        {
            InitializeComponent();
        }
        public DataTablePage(TransactionRepository repo)
        {
            InitializeComponent();

            DataSet ds = new DataSet("Transaction Set");

            DataTable transTable = new DataTable("Transactions");
            transTable.Columns.Add(new DataColumn("name"));
            transTable.Columns.Add(new DataColumn("scen_id"));

            DataTable requestTable = new DataTable("Requests");
            requestTable.Columns.Add(new DataColumn("verb"));
            requestTable.Columns.Add(new DataColumn("parameters"));
            requestTable.Columns.Add(new DataColumn("trans_id"));

            DataTable corrTable = new DataTable("Correlations");
            corrTable.Columns.Add(new DataColumn("name"));
            corrTable.Columns.Add(new DataColumn("original_val"));
            corrTable.Columns.Add(new DataColumn("req_id"));

            foreach(Transaction t in repo.Transactions)
            {
                var newRow = transTable.NewRow();
                newRow["name"] = t.Name;
                newRow["scen_id"] = 1;

            }

            ds.Tables.AddRange(new DataTable[] { transTable, requestTable, corrTable });

            Data_Grid.DataContext = transTable;                

        }
    }
}
