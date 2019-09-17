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
using WLScriptParser.Models.Repositories;
using WLScriptParser.Controls;

namespace WLScriptParser.Pages
{
    /// <summary>
    /// Interaction logic for CorrelationsPage.xaml
    /// </summary>
    public partial class CorrelationsPage : Page
    {
        public CorrelationsPage()
        {
            InitializeComponent();
            Frame_Left.Content = new ScriptCorrelationsControl(ScriptRepository.Repository.ScriptLeft);
            Frame_Right.Content = new ScriptCorrelationsControl(ScriptRepository.Repository.ScriptRight);
        }

        public CorrelationsPage(Script repoLeft, Script repoRight)
        {
            InitializeComponent();
            Frame_Left.Content = new ScriptCorrelationsControl(repoLeft);
            Frame_Right.Content = new ScriptCorrelationsControl(repoRight);
        }

    }
}
