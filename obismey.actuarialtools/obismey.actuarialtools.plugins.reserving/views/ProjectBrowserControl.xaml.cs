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

namespace obismey.actuarialtools.plugins.reserving.views
{
    /// <summary>
    /// Interaction logic for ProjectBrowserControl.xaml
    /// </summary>
    public partial class ProjectBrowserControl : UserControl
    {
        public ProjectBrowserControl()
        {
            InitializeComponent();
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var cc = sender as ContentControl;

            if (cc == null) return;

            var pti = cc.DataContext as obismey.actuarialtools.plugins.reserving.viewmodels.ProjectTreeItem;

            if (pti == null) return;

            if (pti.OnDoubleClick == null) return;

            pti.OnDoubleClick();
        }
    }
}
