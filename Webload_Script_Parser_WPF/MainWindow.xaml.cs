using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using WLScriptParser.Models;
using WLScriptParser.Parsers;
using WLScriptParser.Pages;
using WLScriptParser.Windows;
using System.Windows.Controls;

namespace WLScriptParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TransactionRepository _repo;
        TreeViewPage _treeViewPage;
        CorrelationTablePage _corrTablePage;
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
                try
                {
                    TransactionBlockParser.Parse(oFD.FileName, _repo);
                }
                catch (System.Exception openFileException)
                {
                    MessageBox.Show(openFileException.ToString());
                }
                _treeViewPage = new TreeViewPage(oFD.FileName, _repo);
                _corrTablePage = new CorrelationTablePage(_repo);

                Main_Frame.Content = _treeViewPage;
                Tree_View_Button.IsEnabled = true;
                Corr_Table_Button.IsEnabled = true;
                Header_SaveAs.IsEnabled = true;
            }
        }
        private void MenuExportFileEventHandler(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.Title = "Save Text to File";
            //sFD.Filter = "Text files (.txt)|*txt|CSV files (.csv)|*.csv";
            sFD.Filter = "CSV files (.csv)|*.csv";
            sFD.DefaultExt = "csv";
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

        private void MenuPushDBEventHandler(object sender, RoutedEventArgs e)
        {
            ResolveDBWindow resolveDatabase = new ResolveDBWindow();
            resolveDatabase.ShowDialog();
        }
        private void Tree_View_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = _treeViewPage;
        }
        private void Corr_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = _corrTablePage;
        }
    }
}