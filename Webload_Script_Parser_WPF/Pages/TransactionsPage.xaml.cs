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
using WLScriptParser.Models;
using WLScriptParser.Controls;

namespace WLScriptParser.Pages
{
    /// <summary>
    /// Interaction logic for TransactionsPage.xaml
    /// </summary>
    public partial class TransactionsPage : Page
    {
        public TransactionsPage()
        {
            InitializeComponent();
        }
        public TransactionsPage(
            TransactionRepository repoLeft, 
            TransactionRepository repoRight,
            string fileNameLeft,
            string fileNameRight)
        {
            InitializeComponent();
            Frame_Left.Content = new ScriptTransactionsControl(repoLeft, fileNameLeft);
            Frame_Right.Content = new ScriptTransactionsControl(repoRight, fileNameRight);
        }
    }
}
