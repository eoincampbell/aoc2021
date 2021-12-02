
namespace Aoc2021
{
    internal class Day2 : Puzzle
    {
        public Day2() : base("Inputs/Day2.txt") {
            input = PuzzleInput
                .Select(x => x.Split(" "))
                .Select(y => (y[0], int.Parse(y[1])))
                .ToList();
        }

        private List<(string dir, int units)> input;

        private object Travel(bool part1 = true)
        {
            int a = 0, h= 0, d = 0;

            foreach (var i in input)
                switch (i)
                {
                    case ("forward", int u): h += u; d += (u * a); break;
                    case ("up", int u):   a -= u; break;
                    case ("down", int u): a += u; break;
                }

            return part1 ? (h * a) : (h * d);
        }

        protected override string Title => "################ Day 02 ####################";
        protected override object RunPart1() => Travel();       //1383564
        protected override object RunPart2() => Travel(false);  //1488311643
    }
}
