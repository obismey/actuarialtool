using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.core
{
    public interface IShell
    {
        void SubscribeService(System.Type serviceType, SubscribeServiceCallback Callback);

        IEnumerable<IPlugin> GetPlugins();
    }

    public delegate void SubscribeServiceCallback(System.Type serviceType, IService serviceInstance);
}
