using obismey.actuarialtools.desktop.core;
using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace obismey.actuarialtools.desktop
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application, IShell
    {
        private CompositionContainer _Container;

        [ImportMany(typeof(IPlugin))]
        private List<IPlugin> _Plugins;

        private Dictionary<Type, IService> _Services = new Dictionary<Type, IService>();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            obismey.actuarialtools.desktop.viewmodels.MainWindow.Created += UIService_Created;    

            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            var plugindirectories = System.IO.Directory.EnumerateDirectories(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"));

            foreach (var directory in plugindirectories)
            {
                catalog.Catalogs.Add(new DirectoryCatalog(directory));
            }

            _Container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            batch.AddPart(this);

            try
            {
                _Container.Compose(batch);
                foreach (var item in this._Plugins)
                {
                    item.ServiceCreated += Plugin_ServiceCreated;
                }
                foreach (var item in this._Plugins)
                {
                    item.Load();
                }
                foreach (var item in this._Plugins)
                {
                    item.Reset(this);
                }
            }
            catch (Exception ex)
            {
                _Container.Dispose();
                this._Plugins = null;
            }
        }

        void Plugin_ServiceCreated(object sender, ServiceEventArgs e)
        {
            this._Services.Add(e.ServiceType, e.Service);

            if (this._ServiceSubscription.ContainsKey(e.ServiceType))
            {
                if (this._ServiceSubscription[e.ServiceType] != null )
                {
                    foreach (var item in this._ServiceSubscription[e.ServiceType])
                    {
                        item.Invoke(e.ServiceType, e.Service);
                    }
                }
            }
        }

        void UIService_Created(object sender, EventArgs e)
        {
            this._Services.Add(typeof(IUIService), (IService)sender);

            if (this._ServiceSubscription.ContainsKey(typeof(IUIService)))
            {
                if (this._ServiceSubscription[typeof(IUIService)] != null)
                {
                    foreach (var item in this._ServiceSubscription[typeof(IUIService)])
                    {
                        item.Invoke(typeof(IUIService), (IService)sender);
                    }
                }
            }
        }

        private Dictionary<Type, List<SubscribeServiceCallback>> _ServiceSubscription = new Dictionary<Type,List<SubscribeServiceCallback>>();
        void IShell.SubscribeService(Type serviceType, SubscribeServiceCallback Callback)
        {
            List<SubscribeServiceCallback> callbacks = null;

            if (!this._ServiceSubscription.TryGetValue(serviceType, out callbacks))
            {
                callbacks = new List<SubscribeServiceCallback>();
                this._ServiceSubscription.Add(serviceType, callbacks);
            }

            if (callbacks.Contains(Callback)) return;
            
            callbacks.Add(Callback);

            if (this._Services.ContainsKey(serviceType) )
            {
                Callback.Invoke(serviceType, this._Services[serviceType]);
            }
        }

        IEnumerable<IPlugin> IShell.GetPlugins()
        {
            return this._Plugins;
        }
    }


}
