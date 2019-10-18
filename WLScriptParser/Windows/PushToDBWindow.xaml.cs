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
using WLScriptParser.Utilities;

namespace WLScriptParser.Windows
{
    /// <summary>
    /// Interaction logic for PushToDBWindow.xaml
    /// </summary>
    public partial class PushToDBWindow : Window
    {
        private string _fileName;
        public PushToDBWindow()
        {
            _fileName = ScriptRepository.Repository.ScriptLeft.FileName;
            Title = "Push [" + _fileName.Substring(_fileName.LastIndexOf('\\') + 1) + "] to Database";
            InitializeComponent();
            SAC_Test_Names.PropertyChanged += CheckSacEnableStatus;
            SAC_Build_Names.PropertyChanged += CheckSacEnableStatus;
            SAC_Scenario_Names.PropertyChanged += CheckPushStatus;
            Date_Picker.SelectedDateChanged += CheckPushStatus;
            AppLogger.Log.PropertyChanged += UpdateLog;
        }
        #region helpermethods
        private void CheckSacEnableStatus(object sender, PropertyChangedEventArgs args)
        {
            if (sender == SAC_Test_Names)
            {
                SAC_Build_Names.Clear();
            }
            if (sender == SAC_Build_Names)
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
            Button_Push.IsEnabled = (SacIsValid(SAC_Test_Names) && SacIsValid(SAC_Build_Names) && SacIsValid(SAC_Scenario_Names) && Date_Picker.SelectedDate != null) ? true : false;
        }

        private bool SacIsValid(ScriptAttributesControl control)
        {
            return (control != null && control.SelectedValue != null && control.SelectedValue != control.DefaultValue);
        }
        private void UpdateLog(object sender, PropertyChangedEventArgs args)
        {
            Logger_Text_Block.Text += "\n"+ args.PropertyName;
        }
        #endregion
        #region handlers
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Push_Click(object sender, RoutedEventArgs e)
        {
            string confirmation = $"Pushing: {_fileName.Substring(_fileName.LastIndexOf('\\') + 1)} to WLScriptsDB as:" +
                $"\nTest Group: {SAC_Test_Names.SelectedValue}" +
                $"\nBuild Version: {SAC_Build_Names.SelectedValue}" +
                $"\nScript Name: {SAC_Scenario_Names.SelectedValue}" +
                $"\nRecorded on {Date_Picker.SelectedDate.Value.Date}";

            if (MessageBox.Show(confirmation, "Push to Database?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                var scriptPushCoordinator = new ScriptPushCoordinator()
                {
                    TestName = SAC_Test_Names.SelectedValue,
                    BuildVersion = SAC_Build_Names.SelectedValue,
                    ScriptName = SAC_Scenario_Names.SelectedValue,
                    RecordedDate = Date_Picker.SelectedDate.Value
                };

                Logger_Text_Block.Text = "Pushing script......\n";
                try
                {
                    scriptPushCoordinator.Push(ScriptRepository.Repository.ScriptLeft);
                }
                catch (Exception pushException)
                {
                    MessageBox.Show(pushException.ToString());
                }
            }
        }
        #endregion
    }
}
