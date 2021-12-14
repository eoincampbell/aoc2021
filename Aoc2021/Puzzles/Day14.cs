namespace Aoc2021.Puzzles
{
    internal class Day14 : Puzzle
    {
        public override int Day => 14;
        protected override object RunPart1() => Part1(); //3143
        protected override object RunPart2() => Part2(); //4110215602456

        private Dictionary<string, string> _insertionRules = new();

        public Day14()
            : base("Inputs/Day14.txt")
        //: base("Inputs/Day14Sample.txt")
        {
        }

        private void LoadInsertionRules()
        {
            _insertionRules = new Dictionary<string, string>();

            foreach (var line in PuzzleInput.Skip(2))
            {
                var (source, target, _) = line.Split(" -> ");
                _insertionRules.Add(source, target);
            }
        }

        private object Part1()
        {
            LoadInsertionRules();
            var polymer = PuzzleInput.FirstOrDefault()!;

            for (int step = 1; step <= 10; step++)
            {
                var newElements = new List<string>();
                for(int i = 0; i < polymer.Length -1; i++)
                    newElements.Add(_insertionRules[polymer[i..(i + 2)]]);

                var output = polymer
                    .Select(x => x.ToString())
                    .ZipWithDefault(newElements, (f, s) => new[] { f, s })
                    .SelectMany(f => f);

                polymer = string.Concat(output);
            }

            var elements = polymer
                .GroupBy(g => g)
                .Select(y => new { y.Key, Count = y.Count() })
                .OrderByDescending(o => o.Count)
                .ToList();

            return elements[0].Count - elements[^1].Count;
        }

        private object Part2()
        {
            LoadInsertionRules();
            var polymer = PuzzleInput.FirstOrDefault()!;

            //create a dict to track counts of all pairs
            var pairs = Enumerable
                .Range(0, polymer.Length - 1)
                .Select(idx => polymer[idx..(idx + 2)]) //Yay IndexRanges
                .GroupBy(p => p)
                .ToDictionary(d => d.Key, d => d.LongCount());

            //track char counts in a different dict.
            var charCounts = polymer
                .GroupBy(p => p)
                .ToDictionary(d => d.Key.ToString(), d => d.LongCount());

            for (int step = 1; step <= 40; step++)
            {
                //each step, we're going to end up with a new set of pairs.
                //go through all the pairs we currently have
                    //look it up in the dictionary of polymer formulae
                    //split it and make 2 new pairs.
                    //e.g.NX -> NC and CX by injecting 'C'
                    //find NX in our pair counter and get the current count
                    //find NC in our pair counter (or add) and increment by old NX value
                    //find CX in our pair counter(or add) and increment by old NX value
                    //find C in our char counter (or) add and increment by old NX value

                var newPairs = new Dictionary<string, long>();

                foreach (var oldPair in pairs)
                {
                    var ele = _insertionRules[oldPair.Key];
                    string f = oldPair.Key[0] + ele
                        , s = ele + oldPair.Key[1];

                    var c = oldPair.Value;
                    newPairs[f] = newPairs.ContainsKey(f) ? (newPairs[f] + c) : c;
                    newPairs[s] = newPairs.ContainsKey(s) ? (newPairs[s] + c) : c;
                    charCounts[ele] = charCounts.ContainsKey(ele) ? (charCounts[ele] + c) : c;
                }

                pairs = newPairs;
            }
            return charCounts.Max(m => m.Value) - charCounts.Min(m => m.Value);
        }
    }
}