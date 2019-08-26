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
using WLScriptParser.Models;
using System.Data;

namespace WLScriptParser.Controls
{
    /// <summary>
    /// Interaction logic for ScriptCorrelationsControl.xaml
    /// </summary>
    public partial class ScriptCorrelationsControl : UserControl
    {
        public ScriptCorrelationsControl()
        {
            InitializeComponent();
        }
        public ScriptCorrelationsControl(Script script)
        {
            InitializeComponent();
            var correlations = script.Transactions.SelectMany(t => t.Requests).SelectMany(r => r.Correlations);

            DataTable corrTable = new DataTable("Correlations");
            corrTable.Columns.Add(new DataColumn() { ColumnName = "rule", Caption = "Rule" });
            corrTable.Columns.Add(new DataColumn() { ColumnName = "ext_logic", Caption = "Extraction Logic" });
            corrTable.Columns.Add(new DataColumn() { ColumnName = "original_val", Caption = "Original Value" });

            foreach (Correlation c in correlations)
            {
                var newCorrRow = corrTable.NewRow();
                newCorrRow["rule"] = c.Rule;
                newCorrRow["ext_logic"] = c.ExtractionLogic;
                newCorrRow["original_val"] = c.OriginalValue;
                corrTable.Rows.Add(newCorrRow);
            }
            Data_Grid_Correlations.ItemsSource = corrTable.DefaultView;
        }
    }
}
