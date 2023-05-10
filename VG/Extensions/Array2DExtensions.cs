using System;
using System.Collections.Generic;
using System.Linq;

namespace VG.Extensions
{
    public static class Array2DExtensions
    {
        public static T[] GetColumn<T> (this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range (0, matrix.GetLength (0))
                .Select (x => matrix[x, columnNumber])
                .ToArray ();
        }

        public static T[] GetRow<T> (this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range (0, matrix.GetLength (1))
                .Select (x => matrix[rowNumber, x])
                .ToArray ();
        }

        public static T[][] ToArray2D<T> (this IEnumerable<T> enumerable, IEnumerable<int> dimensions)
        {
            if (enumerable == null)
                return default;

            T[] array = enumerable.ToArray ();

            if (dimensions == null)
                dimensions = new[] { array.Length };

            int[] dimensionsAsArray = dimensions as int[] ?? dimensions.ToArray ();

            int zeroLength = dimensionsAsArray.Length;

            int dimensionsSum = 0;

            foreach (int dimensionValue in dimensionsAsArray)
                dimensionsSum += Math.Max (0, dimensionValue);

            if (dimensionsSum < array.Length)
                zeroLength++;

            T[][] array2D = new T[zeroLength][];

            dimensionsSum = 0;
            for (int iteration = 0; iteration < zeroLength; iteration++)
            {
                int dimensionValue = iteration < dimensionsAsArray.Length ? dimensionsAsArray[iteration] : array.Length - dimensionsSum;
                dimensionValue = Math.Max (0, Math.Min (dimensionValue, array.Length - dimensionsSum));

                array2D[iteration] = new T[dimensionValue];

                int i = 0;
                for (int j = dimensionsSum; j < dimensionsSum + dimensionValue; j++)
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