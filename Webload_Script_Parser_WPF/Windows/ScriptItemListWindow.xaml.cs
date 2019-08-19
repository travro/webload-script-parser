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
using Webload_Script_Parser_WPF.Models;

namespace Webload_Script_Parser_WPF.Windows
{
    /// <summary>
    /// Interaction logic for ScriptItemListWindow.xaml
    /// </summary>
    public partial class ScriptItemListWindow : Window
    {
        public ScriptItemListWindow()
        {
            InitializeComponent();
            
            foreach(string testName in AttributesRepository.Repository.TestNames)
            {
                List_View.Items.Add(testName);
            }
        }
    }
}
