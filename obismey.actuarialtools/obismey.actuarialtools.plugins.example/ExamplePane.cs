using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.example
{
    public class ExamplePane : INavigationPane
    {
        internal string _Icon;
        internal string _Caption;
        internal object _View;
        
        public string Icon
        {
            get { return this._Icon; }
        }

        public string Caption
        {
            get { return this._Caption; }
        }

        public object View
        {
            get { return this._View; }
        }
    }
}
