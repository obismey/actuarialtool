using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public class Project
    {
        public virtual IEnumerable<DataSource> DataSources { get; private set; }
    }
}
