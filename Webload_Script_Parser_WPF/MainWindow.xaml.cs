using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using Webload_Script_Parser_WPF.Models;
using Webload_Script_Parser_WPF.Parsers;
using System.Windows.Controls;

namespace Webload_Script_Parser_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TransactionRepository repo;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuFileOpenEventHandler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Title = "Select Webload Project File";
            oFD.Filter = "WLoad Project Files (*.wlp)| *.wlp";
            oFD.Multiselect = false;
            if (oFD.ShowDialog() == true)
            {
                Title = oFD.FileName;
                repo = new TransactionRepository();
                TransactionBlockParser.Parse(oFD.FileName, repo);

                //Build Treeview
                Tree_View.Items.Clear();
                TreeViewItem transTreeViewItem = new TreeViewItem();
                transTreeViewItem.Header = new TextBlock()
                {
                    Text = oFD.FileName,
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

                        if (r.Correlations != null)
                        {
                            foreach (Correlation c in r.Correlations)
                            {
                                TreeViewItem corrTreeViewItem = new TreeViewItem();
                                corrTreeViewItem.Header = new TextBlock()
                                {
                                    Text = $"{c.Rule} {c.OriginalValue}",
                                    Foreground = Brushes.Violet
                                };
                                reqTreeViewItem.Items.Add(corrTreeViewItem);
                            }
                        }
                    }
                }
            }
        }
        private void MenuExportFileEventHandler(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.Title = "Save Text to File";
            sFD.Filter = "Text files (.txt)|*txt|CSV files (.csv)|*.csv";
            sFD.DefaultExt = "txt";
            if (sFD.ShowDialog() == true)
            {
                using (StreamWriter _writer = new StreamWriter(sFD.FileName))
                {
                    //export as text file
                    //if (sFD.FilterIndex == 1)
                    //{
                    //    int textLines = Text_Box.LineCount;
                    //    int currentline = 1;

                    //    while (currentline < textLines)
                    //    {
                    //        _writer.WriteLine(Text_Box.GetLineText(currentline));
                    //        currentline++;
                    //    }
                    //}
                    ////export as csv file
                    //else
                    //{
                        _writer.WriteLine("Transaction,Request,Parameters");
                        foreach (Transaction t in repo.Transactions)
                        {
                            foreach (Request r in t.Requests)
                            {
                                _writer.WriteLine($"{t.Name},{r.Verb},{r.Parameters}");

                                if (r.Correlations.Length > 0)
                                {
                                    foreach (Correlation c in r.Correlations)
                                    {
                                        _writer.WriteLine($"Corr: {c.Rule}, {c.OriginalValue}");
                                    }
                                }
                            }
                        }
                    //}
                }
            }
        }
        private void MenuFileExitEventHander(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}