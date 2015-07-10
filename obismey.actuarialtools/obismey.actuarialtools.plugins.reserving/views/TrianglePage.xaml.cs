using obismey.actuarialtools.plugins.reserving.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace obismey.actuarialtools.plugins.reserving.views
{
    /// <summary>
    /// Logique d'interaction pour TrianglePage.xaml
    /// </summary>
    public partial class TrianglePage : Page
    {

        private unvell.ReoGrid.ReoGridControl _SheetControl;


        public TrianglePage()
        {
            InitializeComponent();

            this._SheetControl = new unvell.ReoGrid.ReoGridControl();
            this._SheetControl.ControlStyle[unvell.ReoGrid.ReoGridControlColors.GridLine] = Colors.LightGray;
            this._SheetControl.ControlStyle[unvell.ReoGrid.ReoGridControlColors.ColHeadSplitter] = Colors.LightGray;
            this._SheetControl.ControlStyle[unvell.ReoGrid.ReoGridControlColors.RowHeadSplitter] = Colors.LightGray;

            var defaultstyle = new unvell.ReoGrid.WorksheetRangeStyle(this._SheetControl.CurrentWorksheet.GetRangeStyle(unvell.ReoGrid.ReoGridRange.EntireRange));
            defaultstyle.FontName = "Calibri";
            defaultstyle.FontSize = 12;
            
            this._SheetControl.CurrentWorksheet.SetRangeStyle(unvell.ReoGrid.ReoGridRange.EntireRange, defaultstyle);
       
            Grid.SetRow(_SheetControl, 1);

            RootGrid.Children.Add(this._SheetControl);

            this.DataSourceComboBox.ItemsSource = ReservingPlugin.Instance.CurrentProject.ObservableDataSources;


        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var datasource = this.DataSourceComboBox.SelectedItem as DataSourceImpl;

            if (datasource == null) return;

            var survenance = datasource.GetDataColumnByUsage("Survenance");
            var deroulement = datasource.GetDataColumnByUsage("Deroulement");
            var sinistre = datasource.GetDataColumnByUsage("Sinistre");

            if (survenance == null || deroulement == null || sinistre == null) return;

            var origin = 19  ;
            var size = 20;

            try
            {
                origin = int.Parse(OriginTextBox.Text);
                size = int.Parse(SizeTextBox.Text);
            }
            catch (Exception)
            {               
            }

            var qdata = from r in datasource.Table.Rows.Cast<System.Data.DataRow>()
                        where !r.IsNull(survenance) && !r.IsNull(deroulement)
                        select new { 
                            Surv = (double)r[survenance]-1.0, 
                            Deroul = (double)r[deroulement]-1.0,
                            Reglement = r.IsNull(sinistre) ? 0.0 : (double)r[sinistre]
                        };

            qdata = from r in qdata
                    where r.Surv <= origin && r.Surv >= (origin+1 - size) && r.Deroul <= r.Surv
                    select r;

            var finaldata =( from r in qdata
                            group r by new { Surv = (int)r.Surv, Deroul=(int)r.Deroul } into grouping
                            select new { 
                                Key = grouping.Key, 
                                Reglement = grouping.Sum((elt)=> elt.Reglement)
                            }).ToList();


            this._SheetControl.CurrentWorksheet.ClearRangeContent(
                unvell.ReoGrid.ReoGridRange.EntireRange,
                unvell.ReoGrid.CellElementFlag.All);

            this._SheetControl.CurrentWorksheet.RowCount = size + 3;
            this._SheetControl.CurrentWorksheet.ColumnCount = size + 3;
 

            if (finaldata.Count == 0) return;

            var arraydata = new double[size, size];

            foreach (var elt in finaldata)
            {
                arraydata[elt.Key.Surv, elt.Key.Deroul] = elt.Reglement;
            }
            for (int i = 1; i < size; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    arraydata[i, j] = arraydata[i, j] + arraydata[i, j-1];
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    this._SheetControl.CurrentWorksheet[size - i , j + 1] = arraydata[i, j];
                }
            }

            //foreach (var elt in finaldata)
            //{
            //    this._SheetControl.CurrentWorksheet[origin - elt.Key.Surv + 1, elt.Key.Deroul + 1] = elt.Reglement;
            //}

            this._SheetControl.CurrentWorksheet.SetRangeDataFormat(
                unvell.ReoGrid.ReoGridRange.EntireRange,
                unvell.ReoGrid.DataFormat.CellDataFormatFlag.Number,
                new unvell.ReoGrid.DataFormat.NumberDataFormatter.NumberFormatArgs() {  UseSeparator= true, DecimalPlaces=0});
        }

        private void DataSourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
        }
    }
}
