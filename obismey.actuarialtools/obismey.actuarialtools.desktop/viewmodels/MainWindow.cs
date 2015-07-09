using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.viewmodels
{
    public class MainWindow : NotificationObject, IUIService
    {
        internal static MainWindow Instance;
        internal static EventHandler Created;

        private object _NavigationSource;
        private UICommand NavPrev;
        private UICommand NavNext;
        public MainWindow()
        {
            if (Instance != null) return;

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
                    Method = () => CoreUiCommandClick("NavPrev"),
                    IsEnabled =false
                });

            this.ToolBarItems.Add(
              new UICommand()
              {
                  Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/right32.png",
                  Method = () => CoreUiCommandClick("NavNext"),
                  IsEnabled=false 
              });

            this.ToolBarItems.Add(
                new UICommand()
                {
                    Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/appbar.undo.png",
                    Method = () => CoreUiCommandClick("Undo")
                });

            this.ToolBarItems.Add(
              new UICommand()
              {
                  Icon = @"pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/appbar.redo.png",
                  Method = () => CoreUiCommandClick("Redo")
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

            this.NavPrev = this.ToolBarItems[1];
            this.NavNext = this.ToolBarItems[2];

            Instance = this;

            if (Created != null)
            {
                Created(this, EventArgs.Empty);
            }
        }

        private void CoreUiCommandClick(string key)
        {
            if (key == "NavPrev")
            {
                var mainwindow = App.Current.MainWindow as obismey.actuarialtools.desktop.views.MainWindow;

                if (mainwindow.MainFrame.CanGoBack )
                {
                    mainwindow.MainFrame.GoBack();

                    this.NavPrev.IsEnabled = mainwindow.MainFrame.CanGoBack;
                    this.NavNext.IsEnabled = mainwindow.MainFrame.CanGoForward;

                }
            }
            if (key == "NavNext")
            {
                var mainwindow = App.Current.MainWindow as obismey.actuarialtools.desktop.views.MainWindow;

                if (mainwindow.MainFrame.CanGoForward)
                {
                    mainwindow.MainFrame.GoForward();

                    this.NavPrev.IsEnabled = mainwindow.MainFrame.CanGoBack;
                    this.NavNext.IsEnabled = mainwindow.MainFrame.CanGoForward;
                }
            }
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

        public System.Collections.ObjectModel.ObservableCollection<INavigationPane> NavigationPanes { get; private set; }

        public System.Collections.ObjectModel.ObservableCollection<UICommand> ToolBarItems { get; private set; }

        System.Collections.ObjectModel.ObservableCollection<UICommand> IUIService.ToolCommands
        {
            get { return this.ToolBarItems; }
        }

        bool IUIService.RegisterUICommand(string id, UICommand UICommand)
        {
            throw new NotImplementedException();
        }

        System.Collections.ObjectModel.ObservableCollection<INavigationPane> IUIService.NavigationPanes
        {
            get { return this.NavigationPanes; }
        }

        bool IUIService.Navigate(object dataOrUri, object navigationState)
        {
            //this.NavigationSource = dataOrUri;

            var mainwindow = App.Current.MainWindow as obismey.actuarialtools.desktop.views.MainWindow;

            if (dataOrUri is Uri)
            {
                mainwindow.MainFrame.Navigate((Uri)dataOrUri);
            }
            else
            {
                mainwindow.MainFrame.Navigate(dataOrUri);
            }
            this.NavPrev.IsEnabled = mainwindow.MainFrame.CanGoBack;
            this.NavNext.IsEnabled = mainwindow.MainFrame.CanGoForward;
            return true;
        }

        void core.IService.Reset()
        {
        }

        void IDisposable.Dispose()
        {
        }
    }
}
