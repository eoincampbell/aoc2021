namespace Aoc2021
{
    internal static class Extensions
    {
        internal static void Print(this object input) => Console.WriteLine(input);

        internal static T[][] Transpose<T>(this T[][] jaggedArray)
        {
            var elemMin = jaggedArray.Select(x => x.Length).Min();
            return jaggedArray
                .SelectMany(x => x.Take(elemMin).Select((y, i) => new { val = y, idx = i }))
                .GroupBy(x => x.idx, x => x.val, (x, y) => y.ToArray()).ToArray();
        }

        internal static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {

            first = list.Count > 0 ? list[0] : default(T); // or throw
            rest = list.Skip(1).ToList();
        }

        internal static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            second = list.Count > 1 ? list[1] : default; // or throw
            rest = list.Skip(2).ToList();
        }

        internal static T[,] To2DArray<T>(this List<List<T>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            int max = source.Select(l => l).Max(l => l.Count());

            var result = new T[source.Count(), max];

            for (int i = 0; i < source.Count(); i++)
            {
                for (int j = 0; j < source[i].Count(); j++)
                {
                    result[i, j] = source[i][j];
                }
            }

            return result;
        }

        internal static int Product(this IEnumerable<int> source)
        {
            int i = 1;
            foreach (var x in source)
                i *= x;

            return i;
        }
    }
}
