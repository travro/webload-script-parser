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
        TransactionRepository _repoLeft;
        string _scriptNameLeft;
        TransactionRepository _repoRight;
        string _scriptNameRight;
        TreeViewPage _treeViewPageLeft;
        TreeViewPage _treeViewPageRight;
        CorrelationTablePage _corrTablePageLeft;
        CorrelationTablePage _corrTablePageRight;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void MenuFileOpenEventHandler(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog oFD = new OpenFileDialog();
            //oFD.Title = "Select Webload Project File";
            //oFD.Filter = "WLoad Project Files (*.wlp)| *.wlp";
            //oFD.Multiselect = false;
            //if (oFD.ShowDialog() == true)
            //{
            //    Title = oFD.FileName;
            //    _repo = new TransactionRepository();
            //    try
            //    {
            //        TransactionBlockParser.Parse(oFD.FileName, _repo);
            //    }
            //    catch (System.Exception openFileException)
            //    {
            //        MessageBox.Show(openFileException.ToString());
            //    }
            //    _treeViewPage = new TreeViewPage(oFD.FileName, _repoLeft);
            //    _treeViewPage2 = new TreeViewPage(oFD.FileName, _repoRight);
            //    _corrTablePage = new CorrelationTablePage(_repoLeft);
            //    _corrTablePage2 = new CorrelationTablePage(_repoRight);

            //    Main_Frame_Left.Content = _treeViewPage;
            //    Main_Frame_Right.Content = _treeViewPage2;
            //    Tree_View_Button.IsEnabled = true;
            //    Corr_Table_Button.IsEnabled = true;
            //    //Header_SaveAs.IsEnabled = true;
            //}
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
                    _writer.WriteLine("Transaction,Request,Parameters");
                    foreach (Transaction t in _repoLeft.Transactions)
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
            Main_Frame_Left.Content = _treeViewPageLeft;
            Main_Frame_Right.Content = _treeViewPageRight;
        }
        private void Corr_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame_Left.Content = _corrTablePageLeft;
            Main_Frame_Right.Content = _corrTablePageRight;
        }
        private void FillMainReposEventHandler(object sender, ReposFilledEventArgs args)
        {
            try
            {
                _repoLeft = args.RepoLeft;
                _scriptNameLeft = args.ScriptNameLeft;
                _repoRight = args.RepoRight;
                _scriptNameRight = args.ScriptNameRight;
            }
            catch (System.Exception mainRepoFillException)
            {
                MessageBox.Show(mainRepoFillException.ToString());
            }
            ShowContent();
        }
        private void ShowContent()
        {
            _treeViewPageLeft = new TreeViewPage(_repoLeft, _scriptNameLeft);
            _treeViewPageRight = new TreeViewPage(_repoRight, _scriptNameRight);
            _corrTablePageLeft = new CorrelationTablePage(_repoLeft);
            _corrTablePageRight = new CorrelationTablePage(_repoRight);

            Main_Frame_Left.Content = _treeViewPageLeft;
            Main_Frame_Right.Content = _treeViewPageRight;
            Tree_View_Button.IsEnabled = true;
            Corr_Table_Button.IsEnabled = true;
        }
    }
}