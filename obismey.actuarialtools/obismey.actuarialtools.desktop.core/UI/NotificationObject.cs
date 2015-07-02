using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.core.ui
{
    public class NotificationObject: INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }   
}
