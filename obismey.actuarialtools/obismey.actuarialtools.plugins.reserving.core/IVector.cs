using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface IVector
    {
        double this[int row] { get; set; }

        int Count { get; }

        IVector Clone();
    }
}
