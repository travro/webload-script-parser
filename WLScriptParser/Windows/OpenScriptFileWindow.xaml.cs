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
        //Script _scriptLeft;
        string _scriptNameLeft;
        //Script _scriptRight;
        string _scriptNameRight;
        public OpenScriptFileWindow()
        {
            _scriptNameLeft = "";
            _scriptNameRight = "";
            InitializeComponent();
        }

        private void Button_Left_Click(object sender, RoutedEventArgs e)
        {
            //_scriptLeft = FillRepo(Text_Block_Left, _scriptLeft, out _scriptNameLeft);
            SelectScript(Text_Block_Left, out _scriptNameLeft);
            OK_Button.IsEnabled = (BothScriptsChosen()) ? true : false;
        }

        private void Button_Right_Click(object sender, RoutedEventArgs e)
        {
            //_scriptRight = FillRepo(Text_Block_right, _scriptRight, out _scriptNameRight);
            SelectScript(Text_Block_Right, out _scriptNameRight);
            OK_Button.IsEnabled = (BothScriptsChosen()) ? true : false;
        }

        private void SelectScript(TextBlock textBlock, out string scriptName)
        {
            scriptName = "";

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select Webload Project File (*.wlp)",
                Filter = "WLoad Project Files (*.wlp)| *.wlp",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    //ScriptTransactionParser.Parse(openFileDialog.FileName, script);
                    textBlock.Text = scriptName = openFileDialog.FileName;
                }
                catch (System.Exception openFileException)
                {
                    MessageBox.Show(openFileException.ToString());
                }
            }
        }

        public event EventHandler<FilesSelected> ClosedWithResults;
       
        private bool BothScriptsChosen()
        {
            return (_scriptNameLeft != null && _scriptNameLeft != "" && _scriptNameRight != null && _scriptNameRight != "");
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ClosedWithResults != null)
            {
                try
                {
                    ClosedWithResults(this, new FilesSelected()
                    {
                        ScriptNameLeft = _scriptNameLeft,
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

    public class FilesSelected: EventArgs
    {
        public string ScriptNameLeft { get; set; }
        public string ScriptNameRight { get; set; }
    }
}
