using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface IMatrix
    {
        double? this[int row, int column] { get; set;  }

        int RowCount { get; }

        int ColumnCount { get; }

        IMatrix Clone();
    }

    public class ArrayMatrix : IMatrix
    {

        public double? this[int row, int column]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int RowCount
        {
            get { throw new NotImplementedException(); }
        }

        public int ColumnCount
        {
            get { throw new NotImplementedException(); }
        }

        public IMatrix Clone()
        {
            throw new NotImplementedException();
        }
    }
}
