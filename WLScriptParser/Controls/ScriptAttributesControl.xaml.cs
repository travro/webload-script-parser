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
    public partial class ScriptAttributesControl : UserControl, INotifyPropertyChanged
    {        

        public ScriptAttribute Attribute
        {
            get { return (ScriptAttribute)GetValue(ListTypeProperty); }
            set { SetValue(ListTypeProperty, value); }
        }
        public static readonly DependencyProperty ListTypeProperty = DependencyProperty.Register("ListType", typeof(ScriptAttribute), typeof(ScriptAttributesControl));

        public string SelectedValue
        {
            get { return (string)GetValue(SelectionProperty); }
            private set { SetValue(SelectionProperty, value); }
        }
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register("SelectionType", typeof(string), typeof(ScriptAttributesControl));

        public event PropertyChangedEventHandler PropertyChanged;

        public string DefaultValue { get; }

        public ScriptAttributesControl()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedValue = DefaultValue = "No Selection Made";
        }
        
        public void Clear()
        {
            Text_Block.Text = SelectedValue = DefaultValue;
            NotifyPropertyChanged();
        }
        #region helpermethods
        private void NotifyPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(SelectedValue));
            }
        }
        #endregion
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
            var scritItemAddWindow = new ScriptItemAddWindow();
            scritItemAddWindow.Show();
        }
        private void UpdateSelectedValue(object sender, PropertyChangedEventArgs args)
        {
            Text_Block.Text = SelectedValue = args.PropertyName;
            NotifyPropertyChanged();
        }
        #endregion handlers
    }
}
