using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace DContainer.Castle
{
    internal class ThreadSafeDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> inner = new Dictionary<TKey, TValue>();
        private readonly Lock @lock = Lock.Create();

        public bool Contains(TKey key)
        {
            using (@lock.ForReading())
            {
                return inner.ContainsKey(key);
            }
        }

        /// <summary>
        ///   Returns all values and clears the dictionary
        /// </summary>
        /// <returns> </returns>
        public TValue[] EjectAllValues()
        {
            using (@lock.ForWriting())
            {
                var values = inner.Values.ToArray();
                inner.Clear();
                return values;
            }
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
        {
            using (var token = @lock.ForReadingUpgradeable())
            {
                TValue value;
                if (TryGet(key, out value))
                {
                    return value;
                }
                token.Upgrade();
                if (TryGet(key, out value))
                {
                    return value;
                }
                value = factory(key);
                inner.Add(key, value);
                return value;
            }
        }

        public bool Remove(TKey key)
        {
            using (var token = @lock.ForWriting())
            {
                return inner.Remove(key);
            }
        }

        public bool Remove(TKey key, Action<TValue> afterRemove)
        {
            TValue value;
            bool hasValue = false;
            using (var token = @lock.ForWriting())
            {
                hasValue = TryGet(key, out value);
                if (hasValue)
                {
                    inner.Remove(key);
                }
            }
            if (hasValue)
            {
                afterRemove(value);
            }
            return false;
        }

        public TValue GetOrThrow(TKey key)
        {
            using (var token = @lock.ForReading())
            {
                TValue value;
                if (TryGet(key, out value))
                {
                    return value;
                }
            }
            throw new ArgumentException(string.Format("Item for key {0} was not found.", key));
        }

        private bool TryGet(TKey key, out TValue value)
        {
            return inner.TryGetValue(key, out value);
        }
    }
}
