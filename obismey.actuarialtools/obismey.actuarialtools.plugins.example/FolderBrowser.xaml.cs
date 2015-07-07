using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace obismey.actuarialtools.plugins.example
{
    /// <summary>
    /// Interaction logic for FolderBrowser.xaml
    /// </summary>
    public partial class FolderBrowser : UserControl
    {
        public FolderBrowser()
        {
            InitializeComponent();
        }

        private void FolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var foldertree = sender as TreeView;

            if (foldertree.SelectedItem != null )
            {
                var FolderBrowserItem = foldertree.SelectedItem as FolderBrowserItem;

                ExamplePlugin.Instance.UIService.Navigate(
                    new Uri(@"pack://application:,,,/obismey.actuarialtools.plugins.example;component/FileListPage.xaml"), null);
            }

        }

    }

    public class FolderBrowserItem : NotificationObject
    {
        private System.IO.DirectoryInfo _DirectoryInfo = null;
        private bool _Favoris = false;
        private ObservableCollection<FolderBrowserItem> FavoriteList = new ObservableCollection<FolderBrowserItem>();
        
        private static FolderBrowserItem Favorite = null;

        public FolderBrowserItem(System.IO.DirectoryInfo DirectoryInfo)
        {
            this._DirectoryInfo = DirectoryInfo;
        }

        public FolderBrowserItem(bool Favoris)
        {
            this._Favoris = Favoris;

            if (Favoris)
            {
                Favorite = this;
            }
        }

        public string Name
        {
            get
            {
                return this._DirectoryInfo == null ?
                    (this._Favoris ? "Favoris" : "Computer") :
                    this._DirectoryInfo.Name;
            }
        }

        public UICommand AddToFavorite { get { return new UICommand("", () => Favorite.FavoriteList.Add(this)); } }
        public UICommand RemoveFromFavorite { get { return new UICommand("", () => Favorite.FavoriteList.Remove(this)); } }

        public IEnumerable<FolderBrowserItem> Folders
        {
            get
            {
                try
                {
                    if (this._Favoris) return this.FavoriteList;

                    if (this._DirectoryInfo == null)
                    {
                        return from dinfo in System.IO.Directory.GetLogicalDrives()
                               select new FolderBrowserItem(new System.IO.DirectoryInfo(dinfo));
                    }

                    return from dinfo in this._DirectoryInfo.EnumerateDirectories()
                           select new FolderBrowserItem(dinfo);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
