using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
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

}
