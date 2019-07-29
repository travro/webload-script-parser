using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using Webload_Script_Parser_WPF.Models;
using Webload_Script_Parser_WPF.Parsers;
using Webload_Script_Parser_WPF.Views;
using System.Windows.Controls;

namespace Webload_Script_Parser_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TransactionRepository repo;
        TreeViewPage treeViewPage;
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

                treeViewPage = new TreeViewPage(oFD.FileName, repo);
                Main_Frame.Content = treeViewPage;
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