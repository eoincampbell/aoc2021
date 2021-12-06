using Aoc2021;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace Aoc2021.Puzzles
{
    internal class Day06 : Puzzle
    {
        public Day06()
            : base("Inputs/Day06.txt") { } //: base("Inputs/Day06Sample.txt")

        private static void GoToNextDay(Dictionary<int, long> input)
        {
            var temp = input.GetValueOrDefault(0);
            input[0] = input.GetValueOrDefault(1);             //1's go to zero
            input[1] = input.GetValueOrDefault(2);             //2's go to 1
            input[2] = input.GetValueOrDefault(3);             //3's go to 2
            input[3] = input.GetValueOrDefault(4);             //4's go to 3
            input[4] = input.GetValueOrDefault(5);             //5's go to 4
            input[5] = input.GetValueOrDefault(6);             //6's go to 5
            input[6] = input.GetValueOrDefault(7) + temp;      //7's go to 6 AND, 0's go to 6
            input[7] = input.GetValueOrDefault(8);             //8's go to 7 (spawned yesterday)
            input[8] = temp;                                   //                 but 0's also spawn a bunch of new 8's 
        }

        private object Part2(int days)
        {
            var spawnDays = PuzzleInput
                .First()
                .Split(",")
                .Select(int.Parse)
                .GroupBy(x => x)
                .ToDictionary(k => k.Key, v => (long)v.Count()) ;

            for(int d = 0; d < days; d++) 
                GoToNextDay(spawnDays);
            
            return spawnDays.Values.Sum();
        }

        protected override string Title => "################ Day 06 ####################";
        protected override object RunPart1() => Part2(80);  //  352872          || Sample 18:    80: 5394
        protected override object RunPart2() => Part2(256); //  1604361182149
    }

    //Naive part 1 
    //private object Part1(int days)
    //{
    //    List<long> inputs = PuzzleInput.First().Split(",").Select(x => long.Parse(x)).ToList();

    //    for (int i = 0; i < days; i++)
    //    {
    //        int count = inputs.Count;
    //        for (int j = 0; j < count; j++)
    //        {
    //            if (inputs[j] == 0)
    //            {
    //                inputs[j] = 6;
    //                inputs.Add(8);
    //            }
    //            else
    //            {
    //                inputs[j]--;
    //            }
    //        }
    //    }

    //    return inputs.Count();
    //}
}