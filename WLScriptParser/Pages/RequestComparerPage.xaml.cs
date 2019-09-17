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
    public partial class RequestComparerPage : Page
    {


        public RequestComparerPage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public RequestComparerPage(Script s1, Script s2)
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
                        FontSize = 13.5
                        //HorizontalAlignment = HorizontalAlignment.Center
                    });
                    DockPanel dockPanelNames = new DockPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        LastChildFill = true
                    };

                    var fontSize = 12;
                    var foreGround = Brushes.White;
                    var backGround = Brushes.DarkGray;
                    var blockMargin = new Thickness(4.5, 0, 0, 0);

                    var nameBlockLeft = new TextBlock()
                    {
                        Text = table.Columns[0].ColumnName,
                        FontSize = fontSize,
                        Foreground = foreGround,
                        Background = backGround,
                        Width = 750,
                    };

                    var nameBlockRight = new TextBlock()
                    {
                        Text = table.Columns[1].ColumnName,
                        FontSize = fontSize,
                        Foreground = foreGround,
                        Background = backGround,
                        Margin = blockMargin                        
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
                            Width = 750,
                            Background = new SolidColorBrush(colorDispenser.GetColorBySeed(leftRequest.MatchingId))
                        };

                        var valueBlockRight = new TextBlock()
                        {
                            Text = rightRequest.GetRequestString(),
                            FontSize = fontSize,
                            Background = new SolidColorBrush(colorDispenser.GetColorBySeed(rightRequest.MatchingId)),
                            Margin = blockMargin                            
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
