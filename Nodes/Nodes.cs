using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodes
{
    public class Nodes<TKey, TValue> : IEnumerable<Nodes<TKey, TValue>>, IEquatable<Nodes<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public Nodes(TKey key, TValue value) : this(key, value, null)
        {
        }

        public Nodes(TKey key, TValue value, Nodes<TKey, TValue> parent)
        {
            Key = key;
            Value = value;
            Parent = parent;
        }

        public Nodes<TKey, TValue> Parent { get; set; }
        public Nodes<TKey, TValue> Left { get; set; }
        public Nodes<TKey, TValue> Right { get; set; }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Balance { get; set; }

        public bool Equals(Nodes<TKey, TValue> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return Key.CompareTo(other.Key) == 0;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }

            return Equals(other as Nodes<TKey, TValue>);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<Nodes<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
