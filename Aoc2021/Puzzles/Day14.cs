namespace Aoc2021.Puzzles
{
    internal class Day14 : Puzzle
    {
        public override int Day => 14;
        public override string Name => "Extended Polymerization";
        protected override object RunPart1() => Process(10); //3143
        protected override object RunPart2() => Process(40); //4110215602456

        private readonly Dictionary<string, string> _insertionRules;
        private string Polymer => PuzzleInput.FirstOrDefault()!;

        public Day14(): base("Inputs/Day14.txt")
        {
            _insertionRules = new();
            foreach (var line in PuzzleInput.Skip(2))
            {
                var (source, target, _) = line.Split(" -> ");
                _insertionRules.Add(source, target);
            }
        }

        private object Process(int steps)
        {
            var pairs = Enumerable
                .Range(0, Polymer.Length - 1)                           // 10 char polymer has 9 pairs
                .Select(idx => Polymer[idx..(idx + 2)])                 // foreach generate a consecutive pair... Yay IndexRanges
                .GroupBy(p => p)                                        // Group the pairs (in case they are dupes)
                .ToDictionary(d => d.Key, d => d.LongCount());          // Add the Pairs to a tracking dict with their occurrence count

            var el = Polymer                                            // Also add each char element to a tracking dicct
                .GroupBy(p => p)                                        // for tracking the occurrence count
                .ToDictionary(d => d.Key.ToString(), d => d.LongCount());

            for (int step = 1; step <= steps; step++)                   //Step
            {
                var np = new Dictionary<string, long>();                //Dict to hold new pairs cos modifying in foreach is bad
                foreach (var (key, cnt) in pairs)                       //foreach current pair
                {
                    var e = _insertionRules[key];                       //find out what goes between it
                    string f = key[0] + e,                              //create the 2 new pairs with that new element
                           s = e + key[1];

                    np[f] = np.ContainsKey(f) ? (np[f] + cnt) : cnt;    //add or inc. the count for those new pairs
                    np[s] = np.ContainsKey(s) ? (np[s] + cnt) : cnt;
                    el[e] = el.ContainsKey(e) ? (el[e] + cnt) : cnt;    //also increase the count for the number of occurences of that element
                }
                pairs = np;                                             //load our new pairs for the next iteration
            }
            return el.Max(m => m.Value) - el.Min(m => m.Value);         //grab the diff between the largest and smallest
        }
    }
}