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

            var transDataSet = new TransactionDataSet(s1, s2);
            if (!transDataSet.CreateTransactionTables()) MessageBox.Show("The transaction count or names of these two scripts do not match");



            if (transDataSet.DataSet.Tables != null)
            {
                ColorDispenser colorDispenser = new ColorDispenser(55);

                foreach (DataTable table in transDataSet.DataSet.Tables)
                {
                    

                    #region stackpanelOption
                    
                    Stack_Panel.Children.Add(new TextBlock()
                    {
                        Text = "\n" + table.TableName,
                        FontSize = 16
                        //HorizontalAlignment = HorizontalAlignment.Center
                    });
                    DockPanel dockPanelNames = new DockPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        LastChildFill = true
                    };

                    var fontSize = 14;
                    var foreGround = Brushes.White;
                    var backGround = Brushes.DarkGray;
                    var nameBlockLeft = new TextBlock()
                    {
                        Text = table.Columns[0].ColumnName,
                        FontSize = fontSize,
                        Foreground = foreGround,
                        Background = backGround,
                        Width = 800,
                    };

                    var nameBlockRight = new TextBlock()
                    {
                        Text = table.Columns[1].ColumnName,
                        FontSize = fontSize,
                        Foreground = foreGround,
                        Background = backGround
                    };

                    DockPanel.SetDock(nameBlockLeft, Dock.Left);
                    DockPanel.SetDock(nameBlockRight, Dock.Right);

                    dockPanelNames.Children.Add(nameBlockLeft);
                    dockPanelNames.Children.Add(nameBlockRight);

                    Stack_Panel.Children.Add(dockPanelNames);

                    foreach (DataRow row in table.Rows)
                    {
                        var leftRequest = row[0] as Request;
                        var rightRequest = row[1] as Request;
                        
                        DockPanel dockPanelValues = new DockPanel();
                        var valueBlockLeft = new TextBlock()
                        {
                            Text = leftRequest.GetRequestString(),
                            FontSize = fontSize,
                            Width = 800,
                            Background = new SolidColorBrush(colorDispenser.GetColorBySeed(leftRequest.MatchingId))
                        };

                        var valueBlockRight = new TextBlock()
                        {
                            Text = rightRequest.GetRequestString(),
                            FontSize = fontSize,
                            Background = new SolidColorBrush(colorDispenser.GetColorBySeed(rightRequest.MatchingId))
                            
                        };

                        DockPanel.SetDock(valueBlockLeft, Dock.Left);
                        DockPanel.SetDock(valueBlockRight, Dock.Right);

                        dockPanelValues.Children.Add(valueBlockLeft);
                        dockPanelValues.Children.Add(valueBlockRight);

                        Stack_Panel.Children.Add(dockPanelValues);
                        colorDispenser.Reset();
                    }
                    #endregion
                }
            }
            else
            {
                MessageBox.Show("Dataset Tables are null");
            }
        }
    }
}
