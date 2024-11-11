namespace RMGChess.Core
{
    public static class ArrayExtensions
    {
        public static T[] Fill<T>(this T[] array, Func<int, T> selector)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = selector(i);
            }

            return array;
        }

        public static T[,] Fill<T>(this T[,] array, Func<int, int, T> selector)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = selector(i, j);
                }
            }

            return array;
        }

        public static T[,,] Fill<T>(this T[,,] array, Func<int, int, int, T> selector)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        array[i, j, k] = selector(i, j, k);
                    }
                }
            }

            return array;
        }
    }
}
