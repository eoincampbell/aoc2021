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
    }
}
