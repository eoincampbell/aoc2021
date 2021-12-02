namespace Aoc2021
{
    internal class Day1 : Puzzle
    {
        public Day1() : base("Inputs/Day1.txt") { }

        private int[] arr => PuzzleInput.Select(int.Parse).ToArray();

        private object CompareDepths(int lookAhead)
        {
            var count = 0;
            for (int i = 0; i < arr.Length - lookAhead; i++)
                if (arr[i] < arr[i + lookAhead])
                    count++;

            return count;
        }

        protected override object RunPart1() => CompareDepths(1);
        protected override object RunPart2() => CompareDepths(3);
    }
}
