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
using System.ComponentModel;
using WLScriptParser.Models.Repositories;
using WLScriptParser.Controls;
using WLScriptParser.DAL;

namespace WLScriptParser.Windows
{
    /// <summary>
    /// Interaction logic for PushToDBWindow.xaml
    /// </summary>
    public partial class PushToDBWindow : Window
    {
        public PushToDBWindow()
        {
            InitializeComponent();
            SAC_Test_Names.PropertyChanged += CheckSacEnableStatus;
            SAC_Build_Names.PropertyChanged += CheckSacEnableStatus;
            SAC_Scenario_Names.PropertyChanged += CheckPushStatus;
            Date_Picker.SelectedDateChanged += CheckPushStatus;
        }
        #region helpermethods
        private void CheckSacEnableStatus(object sender, PropertyChangedEventArgs args)
        {
            if(sender == SAC_Test_Names)
            {
                SAC_Build_Names.Clear();
            }
            if(sender == SAC_Build_Names)
            {
                SAC_Scenario_Names.Clear();
            }

            if (SacIsValid(SAC_Test_Names) && SacIsValid(SAC_Build_Names))
            {
                SAC_Scenario_Names.Clear();
                //calls to SQL DB
                AttributesRepository.Repository.BuildScriptCollections(SAC_Test_Names.SelectedValue, SAC_Build_Names.SelectedValue);
                SAC_Scenario_Names.IsEnabled = true;
            }
            else
            {
                SAC_Scenario_Names.IsEnabled = false;
                Date_Picker.SelectedDate = null;
            }
        }

        private void CheckPushStatus(object sender, EventArgs args)
        {
            Button_Push.IsEnabled = (SacIsValid(SAC_Test_Names) && SacIsValid(SAC_Build_Names)&& SacIsValid(SAC_Scenario_Names) && Date_Picker.SelectedDate != null) ? true : false;              
        }

        private bool SacIsValid(ScriptAttributesControl control)
        {
            return (control != null && control.SelectedValue != null && control.SelectedValue != control.DefaultValue);
        }

        #endregion
        #region handlers
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Push_Click(object sender, RoutedEventArgs e)
        {
            var scriptPushCoordinator = new ScriptPushCoordinator
                (SAC_Test_Names.SelectedValue, 
                SAC_Build_Names.SelectedValue, 
                SAC_Scenario_Names.SelectedValue,
                Date_Picker.SelectedDate.Value);

            scriptPushCoordinator.Push();
        }
        #endregion
    }
}
