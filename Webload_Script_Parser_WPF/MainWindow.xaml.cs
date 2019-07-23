using System.Windows;
using Microsoft.Win32;
using System.IO;
using Webload_Script_Parser_WPF.Models;

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
                File_String_Label.Content = oFD.FileName;

                repo = new TransactionRepository();
                TransactionBlockParser.Parse(oFD.FileName, repo);

                Text_Box.Text = "\n";

                foreach (Transaction t in repo.Transactions)
                {
                    Text_Box.AppendText($"{t.Name}\n");

                    foreach (Request r in t.Requests)
                    {
                        Text_Box.AppendText($"-- {r.Verb.ToString()} {r.Parameters} \n");

                        if (r.Correlations != null)
                        {
                            foreach (Correlation c in r.Correlations)
                            {
                                Text_Box.AppendText($"-- --: {c.Name}, {c.OriginalValue}\n");
                            }
                        }
                    }
                    Text_Box.AppendText("\n--------------------\n\n");
                }
                Text_Box.AppendText($"\nLine count:{Text_Box.LineCount}");
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
                    if (sFD.FilterIndex == 1)
                    {
                        int textLines = Text_Box.LineCount;
                        int currentline = 1;

                        while (currentline < textLines)
                        {
                            _writer.WriteLine(Text_Box.GetLineText(currentline));
                            currentline++;
                        }
                    }
                    //export as csv file
                    else
                    {
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
                                        _writer.WriteLine($"Corr: {c.Name}, {c.OriginalValue}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void MenuFileExitEventHander(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}