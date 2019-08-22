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
using WLScriptParser.Windows;
using System.ComponentModel;

namespace WLScriptParser.Controls
{
    /// <summary>
    /// Interaction logic for ScriptItemControl.xaml
    /// </summary>
    public partial class ScriptItemControl : UserControl
    {        

        public string SelectedValue { get; private set; }
        public ScriptAttribute Attribute
        {
            get { return (ScriptAttribute)GetValue(ListTypeProperty); }
            set { SetValue(ListTypeProperty, value); }
        }
        public static readonly DependencyProperty ListTypeProperty = DependencyProperty.Register("ListType", typeof(ScriptAttribute), typeof(ScriptItemControl));


        public ScriptItemControl()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedValue = "No item selected";
        }
        #region handlers
        private void Select_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var listWindow = new ScriptItemListWindow(Attribute);
                listWindow.PropertyChanged += UpdateSelectedValue;
                listWindow.ShowDialog();
            }
            catch (Exception scriptItemException)
            {
                MessageBox.Show(scriptItemException.ToString());
            }
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Text_Block.Text = "test";
        }
        private void UpdateSelectedValue(object sender, PropertyChangedEventArgs args)
        {
            Text_Block.Text = SelectedValue = args.PropertyName;
        }
        #endregion handlers
        //public enum ScriptAttribute
        //{
        //    TestNames = 0,
        //    BuildNames = 1,
        //    ScenarioNames = 2,
        //    ScenarioDates = 3
        //}
    }
}
