namespace Aoc2021.Puzzles
{
    internal class Day01 : Puzzle
    {
        public override int Day => 1;
        public override string Name => "Sonar Sweep";
        protected override object RunPart1() => CompareDepths(1);   //1564
        protected override object RunPart2() => CompareDepths(3);   //1611

        public Day01() 
            : base("Inputs/Day01.txt")
            => _arr = PuzzleInput.Select(int.Parse).ToArray();

        private readonly int[] _arr;

        private object CompareDepths(int lookAhead)
        {
            var count = 0;
            for (int i = 0; i < _arr.Length - lookAhead; i++)
                if (_arr[i] < _arr[i + lookAhead]) count++;

            return count;
        }
    }
}
