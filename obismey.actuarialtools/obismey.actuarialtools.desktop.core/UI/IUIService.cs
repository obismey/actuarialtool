using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.core.ui
{
    public interface IUIService : IService
    {
        System.Collections.ObjectModel.ObservableCollection<UICommand> ToolCommands { get; }

        bool RegisterUICommand(string id, UICommand UICommand);

        System.Collections.ObjectModel.ObservableCollection<INavigationPane> NavigationPanes { get; }

        bool Navigate(object dataOrUri, object navigationState);
    }
}
