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
using WLScriptParser.Utilities;
using System.Data;
namespace WLScriptParser.Pages
{
    /// <summary>
    /// Interaction logic for DataTablePage.xaml
    /// </summary>
    public partial class DataTablePage : Page
    {


        public DataTablePage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public DataTablePage(Script s1, Script s2)
        {
            InitializeComponent();
            DataContext = this;

            var dataSet = new TransactionDataSet(s1, s2);

            if (dataSet.DataSet.Tables != null)
            {
                foreach (DataTable table in dataSet.DataSet.Tables)
                {

                    Stack_Panel.Children.Add(new TextBlock()
                    {
                        Text = table.TableName
                    });
                    Stack_Panel.Children.Add(new DataGrid()
                    { 
                        ItemsSource = table.DefaultView                    
                    });

                    
                }
            }
            else
            {
                MessageBox.Show("Dataset Tables are null");
            }

        }
    }
}
