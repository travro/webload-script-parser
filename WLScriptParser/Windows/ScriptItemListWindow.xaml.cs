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
using WLScriptParser.Models.Repositories;
using WLScriptParser.Controls;
using System.ComponentModel;

namespace WLScriptParser.Windows
{
    /// <summary>
    /// Interaction logic for ScriptItemListWindow.xaml
    /// </summary>
    public partial class ScriptItemListWindow : Window, INotifyPropertyChanged
    {
        public string[] SelectableList { get; }
        public ScriptItemListWindow()
        {
            InitializeComponent();
        }
        public ScriptItemListWindow(ScriptAttribute attribute)
        {
            InitializeComponent();
            DataContext = this;

            switch (attribute)
            {
                case ScriptAttribute.TestNames: SelectableList = AttributesRepository.Repository.TestNames; break;
                case ScriptAttribute.BuildNames: SelectableList = AttributesRepository.Repository.TestBuilds; break;
                case ScriptAttribute.ScenarioNames: SelectableList = AttributesRepository.Repository.ScriptNames; break;
                //case ScriptAttribute.ScenarioDates: SelectableList = AttributesRepository.Repository.ScriptDates; break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void List_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selection = (sender as ListView).SelectedValue.ToString();
            OnPropertyChanged(selection);            
            Close();
        }

        public void OnPropertyChanged(string selection)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selection));
            }
        }
    }
}
