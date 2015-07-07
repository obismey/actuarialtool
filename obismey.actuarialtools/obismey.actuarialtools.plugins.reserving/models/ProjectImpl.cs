using obismey.actuarialtools.plugins.reserving.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.models
{
    public class ProjectImpl : Project
    {
        public System.Collections.ObjectModel.ObservableCollection<DataSourceImpl> ObservableDataSources { get; private set; }

        public ProjectImpl()
        {
            this.ObservableDataSources = new System.Collections.ObjectModel.ObservableCollection<DataSourceImpl>();
        }

        public override IEnumerable<DataSource> DataSources
        {
            get
            {
                return this.ObservableDataSources.Cast<DataSource>();
            }
        }
    }
}
