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

namespace WLScriptParser.Controls
{
    /// <summary>
    /// Interaction logic for DualScrollViewerControl.xaml
    /// </summary>
    public partial class DualScrollViewerControl : UserControl
    {
        public DualScrollViewerControl(Script scriptLeft, Script scriptRight)
        {
            InitializeComponent();
            DataContext = this;
            SV_Left.Content = new FullScriptConrol(scriptLeft);
            SV_Right.Content = new FullScriptConrol(scriptRight);
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == SV_Left)
            {
                SV_Right.ScrollToVerticalOffset(e.VerticalOffset);
                SV_Right.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else
            {
                SV_Left.ScrollToVerticalOffset(e.VerticalOffset);
                SV_Left.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
