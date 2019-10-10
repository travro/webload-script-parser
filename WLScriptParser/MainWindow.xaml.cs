using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using WLScriptParser.Models;
using WLScriptParser.Models.Repositories;
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
        FullScriptPage _fullScriptPage;
        RequestComparerPage _requestComparerPage;
        CorrelationsPage _correlationsPage;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void MenuFileOpenEventHandler(object sender, RoutedEventArgs e)
        {
            OpenScriptFileWindow openScriptFileWindow = new OpenScriptFileWindow();
            openScriptFileWindow.Owner = this;
            openScriptFileWindow.ClosedWithResults += FillMainReposEventHandler;
            openScriptFileWindow.ShowDialog();
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
                    //_writer.WriteLine("Transaction,Request,Parameters");
                    //foreach (Transaction t in _repoLeft.Transactions)
                    //{
                    //    foreach (Request r in t.Requests)
                    //    {
                    //        _writer.WriteLine($"{t.Name},{r.Verb},{r.Parameters}");
                    //    }
                    //}
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
        private void RequestComparer_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = _requestComparerPage;
        }
        private void FullScript_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Content = _fullScriptPage;
        }
        private void FillMainReposEventHandler(object sender, FilesSelected args)
        {
            try
            {
                ScriptRepository.Create(args.ScriptNameLeft, args.ScriptNameRight);
            }
            catch (System.Exception mainRepoFillException)
            {
                MessageBox.Show(mainRepoFillException.ToString());
            }
            ShowContent();
        }
        private void ShowContent()
        {
            //_transactionsPage = new TransactionsPage();
            //Main_Frame.Content = _transactionsPage;
            ////Transactions_Button.IsEnabled = true;
            ///
            //_correlationsPage = new CorrelationsPage();            
            // //Correlations_Button.IsEnabled =  true;
            //Main_Frame.Content = _correlationsPage;
            try
            {
                _requestComparerPage = new RequestComparerPage(ScriptRepository.Repository.ScriptLeft, ScriptRepository.Repository.ScriptRight);
                _fullScriptPage = new FullScriptPage(ScriptRepository.Repository.ScriptLeft, ScriptRepository.Repository.ScriptRight);

                Header_SaveAs.IsEnabled = true;
                Header_PushDB.IsEnabled = true;
                Request_Comparer_Button.IsEnabled = true;
                FullScript_Button.IsEnabled = true;

                Main_Frame.Content = _requestComparerPage;
            }
            catch (System.Exception mainwindowException)
            {
                MessageBox.Show(mainwindowException.ToString());
            }
        }
    }
}