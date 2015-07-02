using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace obismey.actuarialtools.desktop.core.ui
{
    public class UICommand : NotificationObject, ICommand
    {
        private string _Caption = "";
        private string _Id = "";
        private Action _OnClick;
        private bool _IsEnabled = true;
        private string _Category = "";
        private string _Icon = "";
        private System.Collections.ObjectModel.ObservableCollection<UICommand> _Children = 
            new System.Collections.ObjectModel.ObservableCollection<UICommand>();

        /// <summary>
        /// 
        /// </summary>
        public UICommand()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caption"></param>
        public UICommand(string Caption)
        {
            this._Caption = Caption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caption"></param>
        /// <param name="OnClick"></param>
        public UICommand(string Caption, Action OnClick)
        {
            this._Caption = Caption;
            this._OnClick = OnClick;
        }

        public string Id { get { return this._Id; }}

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public virtual Action OnClick
        {
            get
            {
                return _OnClick;
            }
            set
            {
                _OnClick = value;
                OnPropertyChanged("OnClick");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                if (value == this._IsEnabled) return;
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
                if (this.CanExecuteChangedEventHandler != null)
                {
                    this.CanExecuteChangedEventHandler(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
                OnPropertyChanged("Category");
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public virtual System.Collections.ObjectModel.ObservableCollection<UICommand> Children { get { return _Children; } }

        bool System.Windows.Input.ICommand.CanExecute(object parameter)
        {
            return this._IsEnabled;
        }

        private EventHandler CanExecuteChangedEventHandler;
        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged
        {
            add { CanExecuteChangedEventHandler += value; }
            remove { CanExecuteChangedEventHandler -= value; }
        }

        void System.Windows.Input.ICommand.Execute(object parameter)
        {
            if (this.OnClick == null) return;
            this.OnClick();
        }
    }
}
