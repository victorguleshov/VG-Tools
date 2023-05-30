using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VG.Collections
{
    public static class ReadOnlyDictionaryExtensions
    {
        public static ReadOnlyDictionary<T0, T1> AsReadOnly<T0, T1>(this Dictionary<T0, T1> dictionary) =>
            new(dictionary);
    }
}