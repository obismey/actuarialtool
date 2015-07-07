using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface IMatrix
    {
        double this[int row, int column] { get; set;  }

        int RowCount { get; }

        int ColumnCount { get; }

        IMatrix Clone();
    }
}
