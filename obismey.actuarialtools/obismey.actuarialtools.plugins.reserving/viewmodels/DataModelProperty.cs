using obismey.actuarialtools.desktop.core.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.viewmodels
{
    public class DataModelProperty : NotificationObject
    {

        private static int inc = 0;
        private string _ConverterType;
        private string _Usage;
        private int _Priority;
        private string _Formula="";
        private string _Type;
        private string _Name;

        private System.Data.DataColumn _SourceColumn;

        public DataModelProperty()
        {
            inc += 1;
            _Name = "Colonne " + inc;
            _Type = "Texte";
            _ConverterType = "(Aucun)";
            _Usage = "(Aucun)";
        }


        public System.Data.DataColumn SourceColumn
        {
            get { return _SourceColumn; }
            set
            {
                _SourceColumn = value;
                OnPropertyChanged("SourceColumn");
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }

        public string Formula
        {
            get { return _Formula; }
            set
            {
                _Formula = value;
                OnPropertyChanged("Formula");
            }
        }

        public string ConverterType
        {
            get { return _ConverterType; }
            set
            {
                _ConverterType = value;
                OnPropertyChanged("Converter");
            }
        }

        public string Usage
        {
            get { return _Usage; }
            set
            {
                _Usage = value;
                OnPropertyChanged("Usage");
            }
        }

        public int Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public DataModelProperty Clone()
        {
            DataModelProperty result = new DataModelProperty();

            result._ConverterType = _ConverterType;
            result._Usage = _Usage;
            result._Priority = _Priority;
            result._Formula = _Formula;
            result._Type = _Type;
            result._Name = _Name;
            result._SourceColumn = _SourceColumn;

            return result;
        }
    }
}
