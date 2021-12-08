using Aoc2021;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace Aoc2021.Puzzles
{
    internal class Day07 : Puzzle
    {
        public Day07()
           : base("Inputs/Day07.txt")
        //: base("Inputs/Day07Sample.txt")
        { }

        private object Part1Better()
        {
            int middle = Input[Input.Length / 2];

            return Input.Sum(x => Math.Abs(middle - x));
        }

        private object Part2Better()
        {
            var mean = Convert.ToInt32(Math.Floor(Input.Average()));

            var fc = new Dictionary<int, int>();

            foreach (int r in Enumerable.Range(mean - 1, 3))
                foreach (int i in Input)
                {
                    var cost = Triangle(Math.Abs(r - i));
                    fc[r] = fc.ContainsKey(r) ? fc[r] + cost : cost;
                }

            return fc.Min(x => x.Value);
        }

        private object Part1or2(bool simpleCost = true)
        {
            var fc = new Dictionary<int, int>();

            foreach (int r in Enumerable.Range(Input[0], Input[^1] - Input[0]))
                foreach (int i in Input)
                {
                    var cost = simpleCost ?
                        Math.Abs(r - i) :
                        Triangle(Math.Abs(r - i));
                    fc[r] = fc.ContainsKey(r) ? fc[r] + cost : cost;
                }

            return fc.Min(x => x.Value);
        }

        private int Triangle(int n) => (n * (n + 1)) / 2;

        private int[] Input => PuzzleInput.First().Split(",").Select(int.Parse).OrderBy(x => x).ToArray();

        protected override string Title => "################ Day 07 ####################";
        protected override object RunPart1() => Part1or2(false);    //  
        protected override object RunPart2() => Part2Better(); //Part1or2(false);
    }
}