using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.UI;

namespace VG.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToStringCustom(this TimeSpan value, bool isTimer = false)
        {
            if (value.TotalSeconds < 0)
                return "--:--";

            var stringBuilder = new StringBuilder();

#if CM_I2_LOC
        var days = $"{value.Days}{ScriptLocalization.Time.days}";
        var hours = $"{value.Hours}{ScriptLocalization.Time.hour}";
        var minutes = $"{value.Minutes}{ScriptLocalization.Time.min}";
        var sec = $"{value.Seconds}{ScriptLocalization.Time.sec}";
#else
            var days = $"{value.Days}d";
            var hours = $"{value.Hours}h";
            var minutes = $"{value.Minutes}m";
            var sec = $"{value.Seconds}s";
#endif

            void AddDays()
            {
                if (value.Days <= 0)
                    return;
                stringBuilder.Append(days);
            }

            void AddHours()
            {
                if (value.Hours <= 0)
                    return;
                stringBuilder.Append(hours);
            }

            void AddMinutes()
            {
                if (value.Minutes <= 0)
                    return;
                stringBuilder.Append(minutes);
            }

            void AddSeconds()
            {
                if (value.Seconds <= 0)
                    return;
                stringBuilder.Append(sec);
            }

            void AddSpace()
            {
                stringBuilder.Append(" ");
            }

            if (value.Days > 0)
            {
                AddDays();
                AddSpace();
                AddHours();
            }
            else if (value.Hours > 0)
            {
                if (isTimer)
                {
                    stringBuilder.Append(value.ToString(@"hh\:mm\:ss"));
                }
                else
                {
                    AddHours();
                    AddSpace();
                    AddMinutes();
                }
            }
            else
            {
                if (isTimer)
                {
                    stringBuilder.Append(value.ToString(@"mm\:ss"));
                }
                else
                {
                    AddMinutes();
                    AddSpace();
                    AddSeconds();
                }
            }

            return stringBuilder.ToString();
        }

        public static void ConstructOptions<T>(this Dropdown p_dropdown) where T : Enum
        {
            if (p_dropdown)
            {
                p_dropdown.ClearOptions();
                p_dropdown.AddOptions(EnumValues<T>());
            }
        }

        public static void ConstructOptions<T>(this TMP_Dropdown p_dropdown) where T : Enum
        {
            if (p_dropdown)
            {
                p_dropdown.ClearOptions();
                p_dropdown.AddOptions(EnumValues<T>());
            }
        }

        // ReSharper disable once UnusedParameter.Global (параметр нужен, чтобы обращаться к методу через переменную);
        public static List<string> EnumValues<T>(this T p_enum) where T : Enum => EnumValues<T>();

        public static List<string> EnumValues<T>() where T : Enum
        {
            var l_enumValues = (T[])Enum.GetValues(typeof(T));
            var r_values = new List<string>();

            if (l_enumValues.Length > 0)
                foreach (var i_enumValue in l_enumValues)
                    r_values.Add(i_enumValue.ToString());

            return r_values;
        }
    }
}