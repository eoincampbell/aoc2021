namespace Aoc2021.Puzzles
{
    internal class Day02 : Puzzle
    {
        public override int Day => 2;
        public override string Name => "Dive!";
        protected override object RunPart1() => Travel();       //1383564
        protected override object RunPart2() => Travel(false);  //1488311643

        public Day02()
            : base("Inputs/Day02.txt")
        {
            input = PuzzleInput
                .Select(x => x.Split(" "))
                .Select(y => new Instruction(y[0], int.Parse(y[1])))
                .ToList();
        }

        private record Instruction(string Direction, int Units);

        private readonly List<Instruction> input;

        private object Travel(bool part1 = true)
        {
            int a = 0, h= 0, d = 0;

            foreach (var i in input)
            {
                switch (i)
                {
                    case ("forward", int u): h += u; d += (u * a); break;
                    case ("up", int u): a -= u; break;
                    case ("down", int u): a += u; break;
                }
            }

            return part1 ? (h * a) : (h * d);
        }
    }
}
