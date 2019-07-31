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

namespace Webload_Script_Parser_WPF.Views
{
    /// <summary>
    /// Interaction logic for TreeViewPage.xaml
    /// </summary>
    public partial class TreeViewPage : Page
    {
        string _fileName;
        public TreeViewPage()
        {
            InitializeComponent();
        }
        public TreeViewPage(string filename, TransactionRepository repo)
        {
            InitializeComponent();
            _fileName = filename;

            //Build Treeview
            Tree_View.Items.Clear();
            TreeViewItem transTreeViewItem = new TreeViewItem();
            transTreeViewItem.Header = new TextBlock()
            {
                Text = _fileName,
                FontWeight = FontWeights.SemiBold
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
