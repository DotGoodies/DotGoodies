using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DotGoodies.Basics
{
    public static class ConcurrentCollectionsExtensions
    {
        public static bool RemoveExisting<TKey, TValue>(
            this ConcurrentDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue expectedValue)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(new KeyValuePair<TKey, TValue>(key, expectedValue));
        }

    }
}
