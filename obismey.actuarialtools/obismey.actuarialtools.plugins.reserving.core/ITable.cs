using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{

    public interface IReadOnlyTable  
    {
        int RowCount { get; }
        int ColumnCount { get; }

        IEnumerable<IColumn> ColumnList { get; }
        IEnumerable<IColumn> KeyColumnList { get; }

        object this[int row, string column] { get; }

        int this[params object[] keys] { get; }
    }

    public interface ITable : IQueryable<IRow>
    {
        IRow Add(string setkey, string key);

        IColumn AddColumn(string Name, TypeCode Type, bool Indexed);

        bool Delete(IRow row);
        bool Delete(string setkey, string key);

        IRow this[string setkey, string key] { get; }

        IEnumerable<string> SetList { get; }

        IEnumerable<IColumn> ColumnList { get; }

        bool ReadOnly { get; }
        bool Sealed { get; }

        void Persist();
    }

    public interface IRow
    {
        ITable Table { get; }

        string Key { get; }

        string SetKey { get; }

        object this[string column] { get; set; }
    }

    public interface IColumn
    {
        ITable Table { get; }

        string Name { get; }

        TypeCode Type { get; }

        bool Indexed { get; }
    }

    public class PagedTable
    {

    }

    public class PagedTableRow : IRow
    {
        private string _Key;
        private string _SetKey;
        private ITable _Table;

        public ITable Table
        {
            get { return (ITable)this._Table; }
        }

        public string Key
        {
            get { return this._Key; }
        }

        public string SetKey
        {
            get { return this._SetKey; }
        }

        public object this[string column]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    internal struct RowKey
    {

    }

    public class PagedTableColumn : IColumn
    {
        private PagedTable _Table;
        private string _Name;
        private TypeCode _Type;
        private bool _Indexed;



        internal PagedTableColumn(PagedTable Table, string Name, TypeCode Type, bool Indexed)
        {

        }

        public ITable Table
        {
            get { return (ITable)this._Table; }
        }

        public string Name
        {
            get { return this._Name; }
        }

        public TypeCode Type
        {
            get { return this._Type; }
        }

        public bool Indexed
        {
            get { return this._Indexed; }
        }
    }
}
