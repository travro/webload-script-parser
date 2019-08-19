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
using Webload_Script_Parser_WPF.Models;
using Webload_Script_Parser_WPF.Windows;

namespace Webload_Script_Parser_WPF.Controls
{
    /// <summary>
    /// Interaction logic for ScriptItemControl.xaml
    /// </summary>
    public partial class ScriptItemControl : UserControl
    {

        public string ListType
        {
            get { return (string)GetValue(ListTypeProperty); }
            set { SetValue(ListTypeProperty, value); }
        }
        public static readonly DependencyProperty ListTypeProperty = DependencyProperty.Register("ListType", typeof(string), typeof(ScriptItemControl));


        public ScriptItemControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #region handlers
        private void Select_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var listWindow = new ScriptItemListWindow();
                listWindow.ShowDialog();
            }
            catch (Exception scriptItemException)
            {

                MessageBox.Show(scriptItemException.ToString());
            }
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion handlers        
    }
}
