using obismey.actuarialtools.desktop.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.example
{
    public interface IExampleService : IService
    {
        void DoWork();
    }
    internal class ExampleService : IExampleService
    {
        private bool _Disposed;
        private bool _Reseted;

        public void Reset()
        {
            if (this._Reseted || this._Disposed) return;

            this._Reseted = true;
        }

        public void Dispose()
        {
            this._Disposed = true;
        }

        public void DoWork()
        {
        }
    }
}
