using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public class DataSource : ProjectItem
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string Type { get; set; }

        public XElement ExtraParameters { get; set; }
    }
}
