using obismey.actuarialtools.desktop.core;
using obismey.actuarialtools.desktop.core.ui;
using obismey.actuarialtools.plugins.reserving.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving
{
    public class ReservingPlugin : IPlugin
    {
        public static ReservingPlugin Instance { get; private set; }

        public IUIService UIService { get; private set; }

        public ProjectTreeItem RootProjectTreeItem { get; private set; }

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

            this.SetupDefaultTree();

            UIService.NavigationPanes.Add(new ProjectNavigationPane() );
           
        }

        private void SetupDefaultTree()
        {
            this.RootProjectTreeItem = new ProjectTreeItem() { Caption = "Project" };

            this.datasourceProjectTreeItem = new ProjectTreeItem() { Caption = "Data Sources" };
            this.datasourceProjectTreeItem.ContextMenu.Add(new UICommand("New Data Source", NewDataSource));
           
            var triangleProjectTreeItem = new ProjectTreeItem() { Caption = "Triangles" };
           
            var scriptProjectTreeItem = new ProjectTreeItem() { Caption = "Scripts" };          
            
            var analysisProjectTreeItem = new ProjectTreeItem() { Caption = "Analysis" };

            var batchProjectTreeItem = new ProjectTreeItem() { Caption = "Batchs" };

            this.RootProjectTreeItem.Children.Add(datasourceProjectTreeItem);
            this.RootProjectTreeItem.Children.Add(triangleProjectTreeItem);
            this.RootProjectTreeItem.Children.Add(scriptProjectTreeItem);
            this.RootProjectTreeItem.Children.Add(analysisProjectTreeItem);
            this.RootProjectTreeItem.Children.Add(batchProjectTreeItem);
        }

        private void NewDataSource()
        {
            var name = InputBox("Enter the name of the data source", System.DateTime.Now.ToString().Replace(":", "").Replace("/", ""));

            if (string.IsNullOrEmpty(name)) return;
            
            var newdatasourceProjectTreeItem = new ProjectTreeItem() { Caption = name };
            newdatasourceProjectTreeItem.ContextMenu.Add(new UICommand("Rename Data Source", () => RenameDataSource(newdatasourceProjectTreeItem)));
           
            var page = new obismey.actuarialtools.plugins.reserving.views.DataImportPage();

            var pageuri = new Uri(@"pack://application:,,,/obismey.actuarialtools.plugins.reserving;component/views/DataImportPage.xaml");

            newdatasourceProjectTreeItem.OnDoubleClick = () => this.UIService.Navigate(page, null);

            this.datasourceProjectTreeItem.Children.Add(newdatasourceProjectTreeItem);


        }

        private void RenameDataSource(ProjectTreeItem newdatasourceProjectTreeItem)
        {
            var name = InputBox("Enter the name of the data source", newdatasourceProjectTreeItem.Caption);

            if (string.IsNullOrEmpty(name)) return;

            newdatasourceProjectTreeItem.Caption = name;

        }
        private string InputBox(string message, string defaultvalue = "")
        {
            var txt = new System.Windows.Controls.TextBox();
            txt.Text = defaultvalue;
            txt.Margin = new System.Windows.Thickness(5);
            txt.MinWidth = 200;
            var win = new System.Windows.Window();
            win.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            win.Title = message;
            win.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            win.Content = txt;
            win.ShowDialog();

            return txt.Text;
        }

        public void Unload()
        {
        }

        public event EventHandler<ServiceEventArgs> ServiceCreated;
        private ProjectTreeItem datasourceProjectTreeItem;

    }
}
