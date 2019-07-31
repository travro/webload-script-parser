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

            DataTable requestTable = new DataTable("Requests");
            requestTable.Columns.Add(new DataColumn("verb"));
            requestTable.Columns.Add(new DataColumn("parameters"));

            DataTable corrTable = new DataTable("Correlations");
            corrTable.Columns.Add(new DataColumn("rule"));
            corrTable.Columns.Add(new DataColumn("ext_logic"));
            corrTable.Columns.Add(new DataColumn("original_val"));

            foreach (Transaction t in repo.Transactions)
            {
                var newTransRow = transTable.NewRow();
                newTransRow["name"] = t.Name;
                transTable.Rows.Add(newTransRow);

                foreach (Request r in t.Requests)
                {
                    var newRequestRow = requestTable.NewRow();
                    newRequestRow["verb"] = r.Verb;
                    newRequestRow["parameters"] = r.Parameters;
                    requestTable.Rows.Add(newRequestRow);

                    foreach (Correlation c in r.Correlations)
                    {
                        var newCorrRow = corrTable.NewRow();
                        newCorrRow["rule"] = c.Rule;
                        newCorrRow["ext_logic"] = c.ExtractionLogic;
                        newCorrRow["original_val"] = c.OriginalValue;
                        corrTable.Rows.Add(newCorrRow);
                    }
                }
            }
            ds.Tables.AddRange(new DataTable[] { transTable, requestTable, corrTable });

            Data_Grid_Transactions.ItemsSource = transTable.DefaultView;
            Data_Grid_Requests.ItemsSource = requestTable.DefaultView;
            Data_Grid_Correlations.ItemsSource = corrTable.DefaultView;

        }
    }
}
