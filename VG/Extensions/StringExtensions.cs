using System;
using System.Collections.Generic;
using System.Linq;

namespace VG.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string @this, string value, StringComparison comparisonType) =>
            @this.IndexOf(value, comparisonType) >= 0;

        public static bool ValueIfNull(this bool? b, bool value) => b ?? value;

        /// <summary>Конвертирует в Nullable Int</summary>
        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out var i)) return i;
            return null;
        }

        /// <summary>Конвертирует в Nullable Bool</summary>
        public static bool? ToNullableBool(this string s)
        {
            if (bool.TryParse(s, out var b)) return b;
            if (int.TryParse(s, out var i)) return i > 0;
            return null;
        }

        /// <summary>Разделяет строку на слова, по пробелам/переносам/точкам/запятым, и прочему мусору</summary>
        public static string[] SplitByWords(this string s)
        {
            return s.Split(new[] { ' ', ',', '.', '|', ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitByWords(this string s, params char[] separators) =>
            s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        public static string[] SplitByWords(this string s, params string[] separators) =>
            s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        public static bool TrySplitByWords(this string s, out string[] result)
        {
            result = s.SplitByWords();
            return result.Length > 1;
        }

        public static bool TrySplitByWords(this string s, out string[] result, params char[] separators)
        {
            result = s.SplitByWords(separators);
            return result.Length > 1;
        }

        public static bool TrySplitByWords(this string s, out string[] result, params string[] separators)
        {
            result = s.SplitByWords(separators);
            return result.Length > 1;
        }

        /// <summary>Парсит и возвращает Enum[] из string[]</summary>
        public static IEnumerable<TEnum> GetEnumsFromWords<TEnum>(this IEnumerable<string> s) where TEnum : struct
        {
            var enums = new List<TEnum>();
            foreach (var item in s)
                if (Enum.TryParse(item, true, out TEnum key))
                    enums.Add(key);

            return enums;
        }

        /// <summary>Превращает колекцию "T" в колекцию "string"</summary>
        public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> keys)
        {
            return keys.Select(item => item != null ? item.ToString() : "");
        }
    }
}