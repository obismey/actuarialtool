using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.viewmodels
{
    public class DataModel : System.Collections.ObjectModel.ObservableCollection<DataModelProperty>
    {


        private System.Data.DataTable _data;
        public DataModel(System.Data.DataTable data)
        {
            _data = data;
        }

        public IEnumerable<System.Data.DataColumn> SourceColumns
        {
            get { return _data.Columns.Cast<System.Data.DataColumn>().ToList(); }
        }

        public static string[] KnownTypes
        {
            get { return new string[] { "Nombre", "Texte", "Date" }; }
        }

        public static string[] KnownConverters
        {
            get { return new string[] { "(Aucun)", "TextToNumber", "TexteToDate", "SasNumberToDate" }; }
        }

        public static string[] KnownUsages
        {
            get
            {
                return new string[] 
                {
                "(Aucun)",
                "Segmentation", 
                "Line Of Business",
                "Categorie Ministerielle",
                "Garantie", 
                "Survenance", 
                "Declaration", 
                "Deroulement", 
                "Reglement",
                "Prime",
                "Sinistre",
                "Provision",
                "Nombre de Sinistres",
                "Charge de Sinistres"
                };
            }
        }

        internal void NotifySourceColumnsChange()
        {
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("SourceColumns"));
        }

        public DataModel Clone()
        {
            var result = new DataModel(_data);

            foreach (DataModelProperty c in this)
            {
                result.Add(c.Clone());
            }
            return result;
        }
    }

}
