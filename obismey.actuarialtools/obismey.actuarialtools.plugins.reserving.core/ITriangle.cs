using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface ITriangle
    {
        double Size { get;  }
        
        double DiagonalExtent { get;  }

        double? this[int row, int column] { get; }

        TriangleFormat Format { get; }
    }

    public enum TriangleFormat
    {
        Unknown,
        Top,
        Down,
        Full
    }
}
