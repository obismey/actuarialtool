using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.viewmodels
{
    public class MainWindow : NotificationObject
    {
        private object _NavigationSource;
        public MainWindow()
        {
            this.NavigationPanes = new System.Collections.ObjectModel.ObservableCollection<INavigationPane>();

           

            this.ToolBarItems = new System.Collections.ObjectModel.ObservableCollection<UICommand>();

            this.ToolBarItems.Add(
                new UICommand()
                {
                    Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/Home.png",
                    Method = () => CoreUiCommandClick("Home")
                });

            this.ToolBarItems.Add(
                new UICommand()
                {
                    Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/left34.png",
                    Method = () => CoreUiCommandClick("NavPrev")
                });

            this.ToolBarItems.Add(
              new UICommand()
              {
                  Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/right32.png",
                  Method = () => CoreUiCommandClick("NavPrev")
              }); 
            
            this.ToolBarItems.Add(
                new UICommand()
                {
                    Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/appbar.tools.png",
                    Method = () => CoreUiCommandClick("Options")
                });

            this.ToolBarItems.Add(
              new UICommand()
              {
                  Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/qmark.png",
                  Method = () => CoreUiCommandClick("Help")
              });
        }

        private void CoreUiCommandClick(string key)
        {
            this.NavigationSource = new Uri("http://intranatixis.intranatixis.com/intranet/jcms/c_5021/fr/accueil");
        }

        /// <summary>
        /// 
        /// </summary>
        public object NavigationSource
        {
            get
            {
                return _NavigationSource;
            }
            set
            {
                _NavigationSource = value;
                OnPropertyChanged("NavigationSource");
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<INavigationPane> NavigationPanes { get; private set;  }

        public System.Collections.ObjectModel.ObservableCollection<UICommand> ToolBarItems { get; private set; }
    }
}
