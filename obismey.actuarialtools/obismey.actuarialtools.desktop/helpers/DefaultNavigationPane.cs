using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.helpers
{
    public class DefaultNavigationPane : INavigationPane
    {
        public string Icon
        {
            get { return ""; }
        }

        public string Caption
        {
            get { return "Default"; }
        }

        public object View
        {
            get { return new System.Windows.Controls.Button(); }
        }
    }
}
