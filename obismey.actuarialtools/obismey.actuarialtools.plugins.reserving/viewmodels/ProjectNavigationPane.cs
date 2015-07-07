using obismey.actuarialtools.desktop.core.ui;
using obismey.actuarialtools.plugins.reserving.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.viewmodels
{
    public class ProjectNavigationPane : NotificationObject , INavigationPane
    {
        private ProjectBrowserControl _View;
       

        public string Icon
        {
            get { return ""; }
        }

        public string Caption
        {
            get { return "Reserve Evaluation Plugin"; }
        }

        public object View
        {
            get 
            {
                if (this._View == null)
                {
                    this._View = new ProjectBrowserControl();
                    this._View.DataContext = ReservingPlugin.Instance;
                }

                return this._View; 
            }
        }
    }
}
