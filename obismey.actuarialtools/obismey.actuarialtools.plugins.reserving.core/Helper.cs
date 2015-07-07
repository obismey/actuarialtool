using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public static class Helper
    {
        static double?[,] ToArray(
            IEnumerable<IRow> rows, 
            Func<IRow,bool> filter, 
            Func<IRow,int> xfunctor, 
            Func<IRow,int> yfunctor,
            Func<IRow, double?> vfunctor)
        {
            return null;
        }

        static double?[,] Accumulate(double?[,] data)
        {
            return null;
        }

        static double?[,] Deccumulate(double?[,] data)
        {
            return null;
        }

        static double?[] AgeToAge(double?[,] data, double?[,] weights)
        {
            return null;
        }
    }
}
