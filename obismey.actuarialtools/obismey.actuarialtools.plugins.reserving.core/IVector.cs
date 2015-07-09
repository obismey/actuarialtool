using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface IVector
    {
        double? this[int row] { get; set; }

        int Count { get; }

        IVector Clone();
    }

    public class ArrayVector : IVector
    {

        public double? this[int row]
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

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public IVector Clone()
        {
            throw new NotImplementedException();
        }
    }
}
