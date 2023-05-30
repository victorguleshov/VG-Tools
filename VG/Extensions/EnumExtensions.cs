using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VG.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class EnumExtensions
    {
        public static bool IsEnum<TEnum>(this string value) where TEnum : Enum => IsEnum<TEnum>(value, out _);

        public static bool IsEnum<TEnum>(this string value, out TEnum out_enumValue) where TEnum : Enum
        {
            var l_enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var l_value = value.ToLowerInvariant();

                foreach (var i_enumValue in l_enumValues)
                {
                    var i_enumAsString = i_enumValue.ToString().ToLowerInvariant();

                    if (l_value.Equals(i_enumAsString))
                    {
                        out_enumValue = i_enumValue;
                        return true;
                    }
                }
            }

            out_enumValue = l_enumValues[0];
            return false;
        }

        public static TEnum ToEnum<TEnum>(this string value) where TEnum : Enum
        {
            var l_enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            return ToEnum(value, l_enumValues[0]);
        }

        public static TEnum ToEnum<TEnum>(this string value, TEnum p_defaultValue) where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var l_value = value.ToLowerInvariant();

                foreach (var i_enumValue in (TEnum[])Enum.GetValues(typeof(TEnum)))
                {
                    var i_enumAsString = i_enumValue.ToString().ToLowerInvariant();

                    if (l_value.Equals(i_enumAsString))
                        return i_enumValue;
                }
            }

            return p_defaultValue;
        }

        public static bool ContainsEnum<TEnum>(this string value) where TEnum : Enum =>
            ContainsEnum<TEnum>(value, out _);

        public static bool ContainsEnum<TEnum>(this string value, out TEnum out_enumValue) where TEnum : Enum
        {
            var l_enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var l_value = value.ToLowerInvariant();

                foreach (var i_enumValue in l_enumValues)
                {
                    var i_enumAsString = i_enumValue.ToString().ToLowerInvariant();

                    if (l_value.Contains(i_enumAsString))
                    {
                        out_enumValue = i_enumValue;
                        return true;
                    }
                }
            }

            out_enumValue = l_enumValues[0];
            return false;
        }

        public static bool ContainedInEnum<TEnum>(this string value) where TEnum : Enum =>
            ContainedInEnum<TEnum>(value, out _);

        public static bool ContainedInEnum<TEnum>(this string value, out TEnum out_enumValue) where TEnum : Enum
        {
            var l_enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var l_value = value.ToLowerInvariant();

                foreach (var i_enumValue in l_enumValues)
                {
                    var i_enumAsString = i_enumValue.ToString().ToLowerInvariant();

                    if (i_enumAsString.Contains(l_value))
                    {
                        out_enumValue = i_enumValue;
                        return true;
                    }
                }
            }

            out_enumValue = l_enumValues[0];
            return false;
        }

        /// <summary>Компонует новый [Flags]Enum на основе string[];</summary>
        /// <param name="value">Массив имен, которые нужно добавить в Enum, как Flags;</param>
        /// <param name="p_setAsEverythingIfEmpty">
        ///     Установить Enum, как Everything (все флаги), если массив пуст (false ->
        ///     Nothing)?
        /// </param>
        public static TEnum ToFlagsEnum<TEnum>(this string[] value, bool p_setAsEverythingIfEmpty = false)
            where TEnum : Enum
        {
            var l_enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            long r_flagsEnumBits = 0;

            if (value != null && value.Length > 0)
            {
                foreach (var i_string in value)
                    if (i_string.IsEnum(out TEnum i_enumValue))
                    {
                        var i_enumValueLong = (long)Convert.ChangeType(i_enumValue, TypeCode.Int64);

                        if (i_enumValueLong == 1 || i_enumValueLong % 2 == 0)
                            r_flagsEnumBits |= i_enumValueLong;
                    }
            }

            else if (p_setAsEverythingIfEmpty)
            {
                foreach (var i_enumValue in l_enumValues)
                {
                    var i_enumValueLong = (long)Convert.ChangeType(i_enumValue, TypeCode.Int64);

                    if (i_enumValueLong == 1 || i_enumValueLong % 2 == 0)
                        r_flagsEnumBits |= i_enumValueLong;
                }
            }

            return (TEnum)Enum.ToObject(typeof(TEnum), r_flagsEnumBits);
        }

        public static TEnum AddFlag<TEnum>(this TEnum @this, params TEnum[] flags) where TEnum : Enum
        {
            var a = Convert.ToInt32(@this);
            foreach (var f in flags) a |= Convert.ToInt32(f);

            return (TEnum)(a as object);
        }

        public static TEnum RemoveFlag<TEnum>(this TEnum @this, params TEnum[] flags) where TEnum : Enum
        {
            var a = Convert.ToInt32(@this);
            if (a == 0) return @this;

            foreach (var f in flags) a &= ~Convert.ToInt32(f);

            return (TEnum)(a as object);
        }

        public static TEnum AsFlags<TEnum>(this List<string> strings) where TEnum : Enum
        {
            var enumAsInt = 0;
            if (strings.Any())
                foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
                foreach (var str in strings)
                    if (str.Contains(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
                        enumAsInt |= Convert.ToInt32(enumValue);

            return (TEnum)(enumAsInt as object);
        }

        public static TEnum AsFlags<TEnum>(this IEnumerable<string> strings) where TEnum : Enum =>
            AsFlags<TEnum>(strings.ToList());
    }
}