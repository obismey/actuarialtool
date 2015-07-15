using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obismey.actuarialtools.plugins.reserving.core
{
    public interface ISegment : IEnumerable<ISegment>, IEquatable<ISegment>, IComparable<ISegment>
    {
        bool HasChildren { get; }

        IReadOnlyDictionary<string, object> Values { get; }
    }
    
    public interface IReadOnlyDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        bool Contains(K key);
        V Get(K key);
        bool TryGet(K key, out V value);

        int Count { get; }
        IEnumerable<K> Keys { get; }
        IEnumerable<V> Values { get; }
    }

    public interface ISegementProvider
    {
        //IEnumerable<ISegment> CreateSegments(ITable table, params string[] fields);
        //IEnumerable<ISegment> CreateSegments(IReadOnlyTable table, params string[] fields);
        IEnumerable<ISegment> CreateSegments(System.Data.DataView table, params string[] fields);
    }
}
 