using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VG.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class ListExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list) => list.Shuffle(new Random());
        public static IList<T> Shuffle<T>(this IList<T> list, int seed) => list.Shuffle(new Random(seed));

        public static IList<T> Shuffle<T>(this IList<T> list, Random random)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }

            return list;
        }

        public static bool ContainsAll<T>(this IList<T> list, T[] candidate)
        {
            if (IsEmptyLocate(list, candidate))
                return false;

            if (candidate.Length > list.Count)
                return false;

            for (var a = 0; a <= list.Count - candidate.Length; a++)
                if (list[a].Equals(candidate[0]))
                {
                    var i = 0;
                    for (; i < candidate.Length; i++)
                        if (false == list[a + i].Equals(candidate[i]))
                            break;

                    if (i == candidate.Length)
                        return true;
                }

            return false;
        }

        public static bool ContainsAll<T>(this IList<T> list, IList<T> candidate)
        {
            if (IsEmptyLocate(list, candidate))
                return false;

            if (candidate.Count > list.Count)
                return false;

            for (var a = 0; a <= list.Count - candidate.Count; a++)
                if (list[a].Equals(candidate[0]))
                {
                    var i = 0;
                    for (; i < candidate.Count; i++)
                        if (false == list[a + i].Equals(candidate[i]))
                            break;

                    if (i == candidate.Count)
                        return true;
                }

            return false;
        }

        public static T RandomValue<T>(this IEnumerable<T> collection)
        {
            var array = collection as T[] ?? collection.ToArray();
            return array.Length > 0 ? array.ElementAt(UnityEngine.Random.Range(0, array.Length)) : default;
        }

        public static bool IsEmptyLocate<T>(this ICollection<T> list, ICollection<T> candidate) =>
            list == null ||
            candidate == null ||
            list.Count == 0 ||
            candidate.Count == 0 ||
            candidate.Count > list.Count;

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> original) where T : ICloneable
        {
            IEnumerable<T> result = default;

            if (original != null)
            {
                var array = original as T[] ?? original.ToArray();

                if (array.Length > 0)
                {
                    result = new T[array.Length];

                    for (var i = 0; i < array.Length; i++)
                        ((T[])result)[i] = (T)array[i]?.Clone();
                }
            }

            return result;
        }

        /// <summary>
        ///     "Нечестный рандом", учитывающий "вес" каждого элемента;<br />
        ///     Возвращает index элемента в переданной коллекции;
        /// </summary>
        public static int DishonestRandom(this IEnumerable<int> chances, Random random = null)
        {
            return DishonestRandom(chances?.Select(chance => (double)chance), random);
        }

        /// <summary>
        ///     "Нечестный рандом", учитывающий "вес" каждого элемента;<br />
        ///     Возвращает index элемента в переданной коллекции;
        /// </summary>
        public static int DishonestRandom(this IEnumerable<float> chances, Random random = null)
        {
            return DishonestRandom(chances?.Select(chance => (double)chance), random);
        }

        /// <summary>
        ///     "Нечестный рандом", учитывающий "вес" каждого элемента;<br />
        ///     Возвращает index элемента в переданной коллекции;
        /// </summary>
        public static int DishonestRandom(this IEnumerable<double> chances, Random random = null)
        {
            if (chances != null)
            {
                var chancesAsArray = chances as double[] ?? chances.ToArray();
                var chancesSum = chancesAsArray.Sum(chance => Math.Max(0, chance));

                var randomValue = random.NextDouble(0, chancesSum);
                double checkedSum = 0;

                for (var chanceIndex = 0; chanceIndex < chancesAsArray.Length; chanceIndex++)
                {
                    var chance = chancesAsArray[chanceIndex];

                    if (chance > 0)
                    {
                        if (randomValue >= checkedSum &&
                            randomValue <= checkedSum + chance)
                            return chanceIndex;

                        checkedSum += chance;
                    }
                }
            }

            return 0;
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            random = random ?? new Random();
            max = Math.Max(min, max);

            return random.NextDouble() * (max - min) + min;
        }

        public static double CircularValue(double value, double limit)
        {
            if (value < 0.0f || value >= limit)
            {
                var remainderAbs = Math.Abs(value % limit);
                value = value < 0.0f ? limit - remainderAbs : remainderAbs;

                if (Math.Abs(value - limit) < 0.0001f)
                    value = 0.0f;
            }

            return value;
        }

        public static List<T0> Intersect<T0, T1>(this IEnumerable<T0> first, IEnumerable<T1> second,
            Func<T0, T1, bool> func)
        {
            var secondList = new List<T1>(second);
            var result = new List<T0>();
            foreach (var item in first)
                for (var index = 0; index < secondList.Count; index++)
                    if (func(item, secondList[index]))
                    {
                        secondList.Remove(secondList[index]);
                        result.Add(item);
                        break;
                    }

            return result;
        }

        public static T Find<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (var item in collection)
                if (predicate(item))
                    return item;
            return default;
        }

        public static bool TryFind<T>(this IEnumerable<T> collection, out T result, Predicate<T> predicate)
        {
            result = default;
            foreach (var item in collection)
                if (predicate(item))
                {
                    result = item;
                    return true;
                }

            return false;
        }
    }
}