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

            bool testNamesIsValid = (SAC_Test_Names != null && SAC_Test_Names.SelectedValue != SAC_Test_Names.DefaultValue);
            bool buildNamesIsValid = (SAC_Build_Names != null && SAC_Build_Names.SelectedValue != SAC_Build_Names.DefaultValue);

            if (testNamesIsValid && buildNamesIsValid)
            {
                SAC_Scenario_Names.Clear();
                AttributesRepository.Repository.BuildScriptCollections(SAC_Test_Names.SelectedValue, SAC_Build_Names.SelectedValue);
                SAC_Scenario_Names.IsEnabled = true;
            }
            else
            {
                SAC_Scenario_Names.IsEnabled = false;
            }
        }

        #endregion
        #region handlers
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Push_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
