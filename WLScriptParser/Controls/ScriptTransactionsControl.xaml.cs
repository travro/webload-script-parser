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
using WLScriptParser.Utilities;

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

            ColorDispenser colorDispenser = new ColorDispenser(55);

            //Build Treeview
            Tree_View.Items.Clear();
            TreeViewItem scriptItem = new TreeViewItem();
            scriptItem.Header = new TextBlock()
            {
                Text = script.FileName.Substring(script.FileName.LastIndexOf('\\') + 1),
                FontWeight = FontWeights.SemiBold,
                Background = Brushes.DarkOrange,
                FontSize = 13.5,
                
            };
            scriptItem.IsExpanded = true;
            Tree_View.Items.Add(scriptItem);

            foreach (Transaction t in script.Transactions)
            {
                TreeViewItem tranTreeViewItem = new TreeViewItem();
                tranTreeViewItem.Header = new TextBlock()
                {
                    Text = t.Name,
                    Foreground = Brushes.Orange,                    
                    FontSize = 12
                };
                tranTreeViewItem.IsExpanded = true;
                scriptItem.Items.Add(tranTreeViewItem);

                foreach (Request r in t.Requests.Where(r => r.Visible == true))
                {
                    TreeViewItem reqTreeViewItem = new TreeViewItem();
                    reqTreeViewItem.Header = new TextBlock()
                    {
                        Text = $"{ r.Verb } { r.Parameters }",
                        Foreground = Brushes.Black,
                        Background = new SolidColorBrush(colorDispenser.GetColorBySeed(r.MatchingId)),
                        Width = 750
                    };
                    reqTreeViewItem.IsExpanded = true;
                    tranTreeViewItem.Items.Add(reqTreeViewItem);

                    foreach (Correlation c in r.Correlations)
                    {
                        TreeViewItem corrTreeViewItem = new TreeViewItem();
                        corrTreeViewItem.Header = new TextBlock()
                        {
                            Text = $"{c.Rule}, {c.ExtractionLogic}, {c.OriginalValue}",
                            Foreground = Brushes.LightPink

                        };
                        reqTreeViewItem.Items.Add(corrTreeViewItem);
                    }
                }
            }
        }
    }
}
