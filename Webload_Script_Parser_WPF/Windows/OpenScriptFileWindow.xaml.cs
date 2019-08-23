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
using System.Windows.Shapes;
using WLScriptParser.Models;
using WLScriptParser.Parsers;
using System.IO;
using Microsoft.Win32;

namespace WLScriptParser.Windows
{
    /// <summary>
    /// Interaction logic for OpenScriptFileWindow.xaml
    /// </summary>
    public partial class OpenScriptFileWindow : Window
    {
        TransactionRepository _repoLeft;
        string _scriptNameLeft;
        TransactionRepository _repoRight;
        string _scriptNameRight;
        public OpenScriptFileWindow()
        {
            _scriptNameLeft = "Left Script";
            _scriptNameRight = "Right Script";
            InitializeComponent();
        }

        private void Button_Left_Click(object sender, RoutedEventArgs e)
        {
            _repoLeft = FillRepo(Text_Block_Left, _repoLeft, out _scriptNameLeft);
            CheckRepositoryInstances();
        }

        private void Button_Right_Click(object sender, RoutedEventArgs e)
        {
            _repoRight = FillRepo(Text_Block_right, _repoRight, out _scriptNameRight);
            CheckRepositoryInstances();
        }

        private TransactionRepository FillRepo(TextBlock textBlock, TransactionRepository repository, out string scriptName)
        {
            repository = new TransactionRepository();

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select Webload Project File (*.wlp)",
                Filter = "WLoad Project Files (*.wlp)| *.wlp",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                scriptName = openFileDialog.Title = openFileDialog.FileName;

                try
                {
                    TransactionBlockParser.Parse(openFileDialog.FileName, repository);
                    textBlock.Text = openFileDialog.FileName;
                }
                catch (System.Exception openFileException)
                {
                    MessageBox.Show(openFileException.ToString());
                }
            }
            else
            {
                scriptName = "name not assigned";
            }
            return repository;
        }

        public event EventHandler<ReposFilledEventArgs> ClosedWithResults;
       
        private void CheckRepositoryInstances()
        {
            OK_Button.IsEnabled = (_repoLeft != null && _repoRight != null)? true: false;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ClosedWithResults != null)
            {
                try
                {
                    ClosedWithResults(this, new ReposFilledEventArgs()
                    {
                        RepoLeft = _repoLeft,
                        ScriptNameLeft = _scriptNameLeft,
                        RepoRight = _repoRight,
                        ScriptNameRight = _scriptNameRight
                    });
                }
                catch (Exception closeWithResultException)
                {

                    MessageBox.Show(closeWithResultException.ToString());
                }
            }
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class ReposFilledEventArgs: EventArgs
    {
        public TransactionRepository RepoLeft { get; set; }
        public string ScriptNameLeft { get; set; }
        public TransactionRepository RepoRight { get; set; }
        public string ScriptNameRight { get; set; }
    }
}
