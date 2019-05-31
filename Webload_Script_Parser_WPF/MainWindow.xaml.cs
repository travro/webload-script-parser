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
using Microsoft.Win32;
using System.IO;
using Webload_Script_Parser;
using Webload_Script_Parser.Models;

namespace Webload_Script_Parser_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Title = "Select Webload Project File";
            oFD.Filter = "WLoad Project Files (*.wlp)| *.wlp";
            oFD.Multiselect = false;
            if (oFD.ShowDialog() == true)
            {
                File_String_Label.Content = oFD.FileName;

                TransactionRepository repo = new TransactionRepository();
                TransactionBlockParser.Parse(oFD.FileName, repo);

                Text_Box.Text = "\n";

                foreach (Transaction t in repo.Transactions)
                {
                    Text_Box.FontWeight = FontWeights.Bold;
                    Text_Box.AppendText($"{t.Name}\n");
                    Text_Box.FontWeight = FontWeights.Regular;
                    foreach (Request r in t.Requests)
                    {
                        Text_Box.AppendText($"-- {r.Verb.ToString()} {r.Parameters} \n");
                    }
                    Text_Box.AppendText("\n--------------------\n\n");
                }
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.Title = "Save Text to File";
            sFD.Filter = "Text files (.txt)|*txt|All Files (*.*)|*.*";
            sFD.AddExtension = false;
            sFD.DefaultExt = "txt";
            if(sFD.ShowDialog() == true)
            {
                File.WriteAllText(sFD.FileName, Text_Box.Text);
            }
        }
    }
}