using obismey.actuarialtools.desktop.core;
using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.example
{
    public class ExamplePlugin : IPlugin
    {

        internal static ExamplePlugin Instance;

        public void Load()
        {
           Instance = this;
        }

        public void Reset(IShell Shell)
        {
            Shell.SubscribeService(typeof(IUIService), UIServiceCallback);
        }

        private void UIServiceCallback(Type serviceType, IService serviceInstance)
        {
            this.UIService = serviceInstance as IUIService;

            UIService.NavigationPanes.Add(new ExamplePane() { _Caption = "Empty Example Pane" });
            UIService.NavigationPanes.Add(
                new ExamplePane() { 
                    _Caption = "File System Browser Pane", 
                    _View = new FolderBrowser() {
                        DataContext = new FolderBrowserItem[] { new FolderBrowserItem(false), new FolderBrowserItem(true) }
                    }
                });

            UIService.ToolCommands.Add(new UICommand() 
                {
                    Icon = "pack://application:,,,/obismey.actuarialtools.desktop;component/resources/icones/squares6.png",
                    Method =() => System.Windows.MessageBox.Show(DateTime.Now.ToString() + " : Module Example")
                });
        }

        public void Unload()
        {
        }

        public event EventHandler<ServiceEventArgs> ServiceCreated;
        internal IUIService UIService;
    }
}
