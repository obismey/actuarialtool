using obismey.actuarialtools.plugins.reserving.viewmodels;
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
using System.Xml.Linq;

namespace obismey.actuarialtools.plugins.reserving.views
{
    /// <summary>
    /// Interaction logic for DataImportPage.xaml
    /// </summary>
    public partial class DataImportPage : Page
    {
        public DataImportPage()
        {
            this.InitializeComponent();

            this.UsageDataGridComboBoxColumn.ItemsSource = DataModel.KnownUsages;
            this.TypeDataGridComboBoxColumn.ItemsSource = DataModel.KnownTypes;
            this.ConvertisseurDataGridComboBoxColumn.ItemsSource = DataModel.KnownConverters;

        }


        private DataModel CurrentModel;
        private System.Data.DataTable CurrentTable;
        private models.DataSourceImpl DataSource;
        private string DataSourceName;


        private void SourceTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SourceTypeRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMappingModelButton_Click(object sender, RoutedEventArgs e)
        {
            var spd = new Microsoft.Win32.SaveFileDialog();
            spd.Filter = "Fichiers de modele de donnes|*.mdl";
            spd.AddExtension = true;
            spd.DefaultExt = "mdl";
            spd.InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules\\ReservingPlugin\\Data");

            if (CurrentModel == null)
                return;

            var spdShowDialog = spd.ShowDialog();

            if (spdShowDialog == null) return;

            if (spdShowDialog.Value)
            {
                Func<DataModelProperty, XElement> fun = (DataModelProperty p) =>
                {
                    var x = new XElement(
                        "Property",
                        new XAttribute("SourceColumn", p.SourceColumn == null ? null : p.SourceColumn.ColumnName),
                        new XAttribute("Name", p.Name),
                        new XAttribute("Type", p.Type),
                        new XAttribute("Formula", p.Formula),
                        new XAttribute("ConverterType", p.ConverterType),
                        new XAttribute("Usage", p.Usage),
                        new XAttribute("Priority", p.Priority));
                    return x;
                };


                var xmodel = new XElement("Model", from p in this.CurrentModel select fun(p));

                xmodel.Save(spd.FileName);
            }
        }

        private void LoadMappingModelButton_Click(object sender, RoutedEventArgs e)
        {
            var opd = new Microsoft.Win32.OpenFileDialog();
            opd.Filter = "Fichiers de modele de donnes|*.mdl";
            opd.InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules\\ReservingPlugin\\Data");

            if (CurrentTable == null)
                return;
            if (CurrentModel == null)
                return;

            var opdShowDialog = opd.ShowDialog();

            if (opdShowDialog == null) return;

            if (opdShowDialog.Value)
            {
                var xmodel = XElement.Load(opd.FileName);

                Func<XElement, DataModelProperty> fun = (XElement x) =>
                {
                    var p = new DataModelProperty();
                    p.Name = x.Attribute("Name").Value;
                    p.Type = x.Attribute("Type").Value;
                    p.Formula = x.Attribute("Formula").Value;
                    p.ConverterType = x.Attribute("ConverterType").Value;
                    p.Usage = x.Attribute("Usage").Value;
                    p.Priority = int.Parse(x.Attribute("Priority").Value);
                    if (CurrentTable != null)
                    {
                        if (CurrentTable.Columns.Contains(x.Attribute("SourceColumn").Value))
                        {
                            p.SourceColumn = CurrentTable.Columns[x.Attribute("SourceColumn").Value];
                        }
                    }
                    return p;
                };


                foreach (var x in xmodel.Elements("Property"))
                {
                    CurrentModel.Add(fun(x));
                }
            }
        }
        private void PreviewListViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {

        }
        private void ValidateChangesMappingModelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImporterButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CSVRadioButton.IsChecked != null)
            {
                if (this.CSVRadioButton.IsChecked.Value)
                {
                    this.LoadCSV();
                }
            }
            if (this.SASRadioButton.IsChecked != null)
            {
                if (this.SASRadioButton.IsChecked.Value)
                {
                    this.LoadSAS();
                }
            }
            if (this.SQLRadioButton.IsChecked != null)
            {
                if (this.SQLRadioButton.IsChecked.Value)
                {
                    this.LoadSQL();
                }
            }
        }

        private void LoadCSV()
        {

        }
        private void LoadSQL()
        {
            var prompt = new MSDASC.DataLinks();

            try
            {
                var constr = prompt.PromptNew();
            }
            catch (Exception)
            {

            }
        }
        private void LoadSAS()
        {
            var opd = new Microsoft.Win32.OpenFileDialog();
            opd.Filter = "Fichiers SAS|*.sas7bdat";

            var opdShowDialog = opd.ShowDialog();
            if (opdShowDialog == null) return;

            if (opdShowDialog.Value)
            {
                var conbuilder = new System.Data.OleDb.OleDbConnectionStringBuilder();
                conbuilder.Provider = "sas.LocalProvider";
                conbuilder.DataSource = System.IO.Path.GetDirectoryName(opd.FileName);

                var cmd = new System.Data.OleDb.OleDbCommand(
                    System.IO.Path.GetFileNameWithoutExtension(opd.FileName),
                    new System.Data.OleDb.OleDbConnection(conbuilder.ConnectionString));

                cmd.CommandType = System.Data.CommandType.TableDirect;
                var dataTable = new System.Data.DataTable();
                var adp = new System.Data.OleDb.OleDbDataAdapter(cmd);
                adp.Fill(dataTable);

                EXCELRadioButton.IsEnabled = false;
                SQLRadioButton.IsEnabled = false;
                CSVRadioButton.IsEnabled = false;

                this.CurrentModel = new DataModel(dataTable);
                this.CurrentTable = dataTable;

                foreach (System.Data.DataColumn c in dataTable.Columns)
                {
                    var dmp = new DataModelProperty();
                    dmp.Name = c.ColumnName;
                    dmp.SourceColumn = c;

                    if (c.DataType == typeof(string))
                    {
                        dmp.Type = "Texte";
                    }
                    if (c.DataType == typeof(double))
                    {
                        dmp.Type = "Nombre";
                    }

                    this.CurrentModel.Add(dmp);
                }

                this.DataModelEditingDataGrid.ItemsSource = this.CurrentModel;

                this.ColonneSourceDataGridComboBoxColumn.ItemsSource = this.CurrentModel.SourceColumns;

                this.FileTextBlock.Text = opd.FileName;

                this.DataSource = this.DataSource == null ?
                    new obismey.actuarialtools.plugins.reserving.models.DataSourceImpl() { Name= DataSourceName } :
                    this.DataSource;

                this.DataSource.Model = this.CurrentModel;

                this.DataSource.Table = this.CurrentTable;


                if (!ReservingPlugin.Instance.CurrentProject.ObservableDataSources.Contains(this.DataSource))
                {
                    ReservingPlugin.Instance.CurrentProject.ObservableDataSources.Add(this.DataSource);
                }

            }
        }

        internal void SetDataSourceName(string name)
        {
            this.DataSourceName = name;

            if (this.DataSource != null)
            {
                this.DataSource.Name = name;
            }
        }
    }
}
