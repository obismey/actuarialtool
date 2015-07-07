using obismey.actuarialtools.plugins.reserving.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.models
{
    public class DataSourceImpl : obismey.actuarialtools.plugins.reserving.core.DataSource
    {

        public DataModel Model { get; set; }

        public System.Data.DataTable Table { get; set; }


        public System.Data.DataColumn GetDataColumnByUsage(string usage)
        {
            try
            {
                return (from prop in Model where prop.Usage == usage select prop.SourceColumn).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
