namespace Aoc2021.Puzzles
{
    internal class Day07 : Puzzle
    {
        public override int Day => 7;
        public override string Name => "The Treachery of Whales";
        protected override object RunPart1() => Part1(); //344138
        protected override object RunPart2() => Part2(); //94862124

        public Day07()
           : base("Inputs/Day07.txt")
        //: base("Inputs/Day07Sample.txt")
        { }

        private object Part1()
        {
            int middle = Input[Input.Length / 2];
            return Input.Sum(x => Math.Abs(middle - x));
        }

        private object Part2()
        {
            var mean = Convert.ToInt32(Math.Floor(Input.Average()));
            var fc = new Dictionary<int, int>();

            foreach (int r in Enumerable.Range(mean - 1, 3))
            {
                foreach (int i in Input)
                {
                    var cost = Triangle(Math.Abs(r - i));
                    fc[r] = fc.ContainsKey(r) ? fc[r] + cost : cost;
                }
            }

            return fc.Min(x => x.Value);
        }

        private static int Triangle(int n) => (n * (n + 1)) / 2;

        private int[] Input => PuzzleInput.First().Split(",").Select(int.Parse).OrderBy(x => x).ToArray();
    }
}