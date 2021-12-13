namespace Aoc2021.Puzzles
{
    internal class Day06 : Puzzle
    {
        protected override string Title => "################ Day 06 ####################";
        protected override object RunPart1() => Part2Better(80);  //  352872          || Sample 18:    80: 5394
        protected override object RunPart2() => Part2Better(256); //  1604361182149

        public Day06()
            : base("Inputs/Day06.txt")
            //: base("Inputs/Day06Sample.txt")
        {

        }

        private static void GoToNextDay(Dictionary<int, long> input)
        {
            var zeroDay = input[0];
            for(int i = 0; i<=8; i++)
                input[i] = i switch
                {
                    6 => input[i + 1] + zeroDay,
                    8 => zeroDay,
                    _ => input[i + 1],
                };
        }

        private object Part2Better(int days)
        {
            var inputs = PuzzleInput.First().Split(",").Select(int.Parse);

            var fish = new long[9];

            foreach(int i in inputs) fish[i]++;

            for (int d = 0; d < days; d++)
                fish[(d + 7) % 9] += fish[d % 9];

            return fish.Sum();
        }


        private object Part2(int days)
        {
            var (input, _) = PuzzleInput.ToList();

            var spawnDays = input.Split(",")
                .Select(int.Parse).GroupBy(x => x)
                .ToDictionary(k => k.Key, v => (long)v.Count()) ;

            for(int i = 0; i <= 8; i++)
                spawnDays.TryAdd(i, 0);

            for(int d = 0; d < days; d++)
                GoToNextDay(spawnDays);

            return spawnDays.Values.Sum();
        }
    }

    //var temp = input.GetValueOrDefault(0);
    //input[0] = input.GetValueOrDefault(1);             //1's go to zero
    //input[1] = input.GetValueOrDefault(2);             //2's go to 1
    //input[2] = input.GetValueOrDefault(3);             //3's go to 2
    //input[3] = input.GetValueOrDefault(4);             //4's go to 3
    //input[4] = input.GetValueOrDefault(5);             //5's go to 4
    //input[5] = input.GetValueOrDefault(6);             //6's go to 5
    //input[6] = input.GetValueOrDefault(7) + temp;      //7's go to 6 AND, 0's go to 6
    //input[7] = input.GetValueOrDefault(8);             //8's go to 7 (spawned yesterday)
    //input[8] = temp;                                   //                 but 0's also spawn a bunch of new 8's 

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