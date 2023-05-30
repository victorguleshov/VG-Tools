using System;
using System.Collections.Generic;
using System.Linq;

namespace VG.Extensions
{
    public static class Array2DExtensions
    {
        public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
        }

        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }

        public static T[][] ToArray2D<T>(this IEnumerable<T> enumerable, IEnumerable<int> dimensions)
        {
            if (enumerable == null)
                return default;

            var array = enumerable.ToArray();

            if (dimensions == null)
                dimensions = new[] { array.Length };

            var dimensionsAsArray = dimensions as int[] ?? dimensions.ToArray();

            var zeroLength = dimensionsAsArray.Length;

            var dimensionsSum = 0;

            foreach (var dimensionValue in dimensionsAsArray)
                dimensionsSum += Math.Max(0, dimensionValue);

            if (dimensionsSum < array.Length)
                zeroLength++;

            var array2D = new T[zeroLength][];

            dimensionsSum = 0;
            for (var iteration = 0; iteration < zeroLength; iteration++)
            {
                var dimensionValue = iteration < dimensionsAsArray.Length
                    ? dimensionsAsArray[iteration]
                    : array.Length - dimensionsSum;
                dimensionValue = Math.Max(0, Math.Min(dimensionValue, array.Length - dimensionsSum));

                array2D[iteration] = new T[dimensionValue];

                var i = 0;
                for (var j = dimensionsSum; j < dimensionsSum + dimensionValue; j++)
                {
                    array2D[iteration][i] = array[j];
                    i++;
                }

                dimensionsSum += dimensionValue;
            }

            return array2D;
        }
    }
}