using System;
using System.IO;
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
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Webload_Script_Parser_WPF.Windows
{
    /// <summary>
    /// Interaction logic for ResolveDatabase.xaml
    /// </summary>
    public partial class ResolveDBWindow : Window
    {
        string _dbName = "WLScriptsDB";
        public ResolveDBWindow()
        {
            InitializeComponent();
            CheckDatabaseAync();
        }

        private async void CheckDatabaseAync()
        {
            bool dbExists = await Task.Run(() => CheckDatabase());

            if (dbExists)
            {
                Text_Block.Text = $"Database {_dbName} found";
                Next_Button.IsEnabled = true;
            }
            else
            {
                Text_Block.Text = $"Database {_dbName} not found, please restore {_dbName} with the appropriate backup file";
                Next_Button.IsEnabled = false;
            }
        }
        private bool CheckDatabase()
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["master"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand("select count(*) from master.dbo.sysdatabases where name=@database", cnn))
                {
                    cmd.Parameters.Add("@database", SqlDbType.NVarChar).Value = _dbName;
                    return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                }
            }
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            PushToDBWindow pushWindow = new PushToDBWindow();
            this.Close();
            pushWindow.ShowDialog();

        }
    }
}
