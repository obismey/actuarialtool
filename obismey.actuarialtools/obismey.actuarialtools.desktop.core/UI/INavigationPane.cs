using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.core.ui
{
    public interface INavigationPane
    {
        string Icon { get; }
        string Caption { get; }
        object View { get; }
    }
}
