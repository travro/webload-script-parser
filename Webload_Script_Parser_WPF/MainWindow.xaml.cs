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
        TransactionRepository _repo;
        TreeViewPage _treeViewPage;
        DataTablePage _dataTablePage;
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
                _repo = new TransactionRepository();
                TransactionBlockParser.Parse(oFD.FileName, _repo);
                _treeViewPage = new TreeViewPage(oFD.FileName, _repo);
                _dataTablePage = new DataTablePage(_repo);


                Main_Frame.Content = _treeViewPage;
                Tree_View_Button.IsEnabled = true;
                Data_Table_Button.IsEnabled = true;
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
                    foreach (Transaction t in _repo.Transactions)
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

        private void Tree_View_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = _treeViewPage;
        }

        private void Data_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = Main_Frame.Content = _dataTablePage;
        }
    }
}