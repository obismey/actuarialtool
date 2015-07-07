using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.viewmodels
{
    public class ProjectTreeItem   : NotificationObject
    {
        private Action _OnDoubleClick; 
        private string _Caption = "";
        private string _Icon = "";
        private System.Collections.ObjectModel.ObservableCollection<ProjectTreeItem> _Children =
            new System.Collections.ObjectModel.ObservableCollection<ProjectTreeItem>();
        private System.Collections.ObjectModel.ObservableCollection<UICommand> _ContextMenu =
            new System.Collections.ObjectModel.ObservableCollection<UICommand>();

        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                OnPropertyChanged("Caption");
            }
        }

        public virtual string Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
                OnPropertyChanged("Icon");
            }
        }

        public virtual Action OnDoubleClick
        {
            get
            {
                return _OnDoubleClick;
            }
            set
            {
                _OnDoubleClick = value;
                OnPropertyChanged("OnDoubleClick");
            }
        }
        public virtual System.Collections.ObjectModel.ObservableCollection<ProjectTreeItem> Children { get { return _Children; } }
        public virtual System.Collections.ObjectModel.ObservableCollection<UICommand> ContextMenu { get { return _ContextMenu; } }


    }
}
