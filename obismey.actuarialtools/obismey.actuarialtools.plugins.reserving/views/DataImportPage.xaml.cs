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
        

        private void SourceTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SourceTypeRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMappingModelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadMappingModelButton_Click(object sender, RoutedEventArgs e)
        {

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
            var prompt = new MSDASC.DataLinksClass();

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

            if (opd.ShowDialog() != null)
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


            }
        }
    }
}
