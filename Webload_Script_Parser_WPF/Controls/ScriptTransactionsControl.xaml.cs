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

namespace WLScriptParser.Controls
{
    /// <summary>
    /// Interaction logic for ScriptTransactionsControl.xaml
    /// </summary>
    public partial class ScriptTransactionsControl : UserControl
    {
        string _fileName;

        public ScriptTransactionsControl()
        {
            InitializeComponent();
        }

        public ScriptTransactionsControl(TransactionRepository repo, string filename)
        {
            InitializeComponent();
            _fileName = filename;

            //Build Treeview
            Tree_View.Items.Clear();
            TreeViewItem transTreeViewItem = new TreeViewItem();
            transTreeViewItem.Header = new TextBlock()
            {
                Text = _fileName.Substring(_fileName.LastIndexOf('\\') + 1),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };
            transTreeViewItem.IsExpanded = true;
            Tree_View.Items.Add(transTreeViewItem);

            foreach (Transaction t in repo.Transactions)
            {
                TreeViewItem tranTreeViewItem = new TreeViewItem();
                tranTreeViewItem.Header = new TextBlock()
                {
                    Text = t.Name,
                    Foreground = Brushes.Blue,
                    FontSize = 14.5
                };
                tranTreeViewItem.IsExpanded = true;
                transTreeViewItem.Items.Add(tranTreeViewItem);

                foreach (Request r in t.Requests)
                {
                    TreeViewItem reqTreeViewItem = new TreeViewItem();
                    reqTreeViewItem.Header = new TextBlock()
                    {
                        Text = $"{ r.Verb } { r.Parameters }",
                        Foreground = (r.Visible) ? Brushes.DarkRed : Brushes.Gray
                    };
                    reqTreeViewItem.IsExpanded = true;
                    tranTreeViewItem.Items.Add(reqTreeViewItem);

                    foreach (Correlation c in r.Correlations)
                    {
                        TreeViewItem corrTreeViewItem = new TreeViewItem();
                        corrTreeViewItem.Header = new TextBlock()
                        {
                            Text = $"{c.Rule}, {c.ExtractionLogic}, {c.OriginalValue}",
                            Foreground = Brushes.DarkViolet
                        };
                        reqTreeViewItem.Items.Add(corrTreeViewItem);
                    }
                }
            }
        }
    }
}
