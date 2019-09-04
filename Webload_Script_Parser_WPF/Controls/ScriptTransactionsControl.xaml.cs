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
using WLScriptParser.Models.Repositories;

namespace WLScriptParser.Controls
{
    /// <summary>
    /// Interaction logic for ScriptTransactionsControl.xaml
    /// </summary>
    public partial class ScriptTransactionsControl : UserControl
    {
        public ScriptTransactionsControl()
        {
            InitializeComponent();
        }

        public ScriptTransactionsControl(Script script)
        {
            InitializeComponent();

            //Build Treeview
            Tree_View.Items.Clear();
            TreeViewItem scriptItem = new TreeViewItem();
            scriptItem.Header = new TextBlock()
            {
                Text = script.FileName.Substring(script.FileName.LastIndexOf('\\') + 1),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };
            scriptItem.IsExpanded = true;
            Tree_View.Items.Add(scriptItem);

            foreach (Transaction t in script.Transactions)
            {
                TreeViewItem tranTreeViewItem = new TreeViewItem();
                tranTreeViewItem.Header = new TextBlock()
                {
                    Text = t.Name,
                    Foreground = Brushes.Blue,                    
                    FontSize = 14.5
                };
                tranTreeViewItem.IsExpanded = true;
                scriptItem.Items.Add(tranTreeViewItem);

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
