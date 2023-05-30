using System;

namespace VG.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Shuffle<T>(this T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);
                (array[k], array[n]) = (array[n], array[k]);
            }

            return array;
        }

        public static T[] Shuffle<T>(this T[] array, int seed) => Shuffle(array, new Random(seed));

        public static T[] Shuffle<T>(this T[] array, Random random)
        {
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = random.Next(0, n + 1);
                (array[k], array[n]) = (array[n], array[k]);
            }

            return array;
        }

        public static bool ContainsAll<T>(this T[] array, T[] candidate)
        {
            if (IsEmptyLocate(array, candidate))
                return false;

            if (candidate.Length > array.Length)
                return false;

            for (var a = 0; a <= array.Length - candidate.Length; a++)
                if (array[a].Equals(candidate[0]))
                {
                    var i = 0;
                    for (; i < candidate.Length; i++)
                        if (false == array[a + i].Equals(candidate[i]))
                            break;
                    if (i == candidate.Length)
                        return true;
                }

            return false;
        }

        private static bool IsEmptyLocate<T>(T[] array, T[] candidate) =>
            array == null
            || candidate == null
            || array.Length == 0
            || candidate.Length == 0
            || candidate.Length > array.Length;
    }
}