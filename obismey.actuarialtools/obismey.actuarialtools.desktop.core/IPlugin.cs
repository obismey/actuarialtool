using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.desktop.core
{
    [System.ComponentModel.Composition.InheritedExport()]
    public interface IPlugin
    {
        void Load();

        void Reset(IShell Shell);

        void Unload();

        event EventHandler<ServiceEventArgs> ServiceCreated;
    }

    public class ServiceEventArgs : EventArgs
    {
        public ServiceEventArgs(IService Service, Type ServiceType)
        {
            this.Service = Service;
            this.ServiceType = ServiceType;
        }

        public IService Service { get; private set; }

        public Type ServiceType { get; private set; }
    }
}
