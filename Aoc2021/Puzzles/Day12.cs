using Aoc2021.Puzzles.Day12Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day12 : Puzzle
    {
        public Day12()
            : base("Inputs/Day12.txt")
            //: base("Inputs/Day12Sample1.txt") 10  | 36
            //: base("Inputs/Day12Sample2.txt") 19  | 103
            //: base("Inputs/Day12Sample3.txt") 226 | 3509
        {
            _caves = new Dictionary<string, Cave>();
            foreach (var line in PuzzleInput)
            {
                var (a, b, _) = line.Split('-');
                _caves.TryAdd(a, new Cave(a));
                _caves.TryAdd(b, new Cave(b));
                _caves[a].Link(_caves[b]);
                _caves[b].Link(_caves[a]);
            }
            _routes = new List<List<Cave>>();
        }

        protected override string Title => "################ Day 12 ####################";
        protected override object RunPart1() => Part1(); //3738  
        protected override object RunPart2() => Part2(); //120506   

        private readonly Dictionary<string, Cave> _caves;
        private List<List<Cave>> _routes;

        private void ResetRoutes() => _routes = new List<List<Cave>>();

        private object Part1()
        {
            ResetRoutes();
            GenerateRoutes("start", new List<Cave>(), new HashSet<string>(), Mode.Part1, false);
            return _routes.Count;
        }

        private object Part2()
        {
            ResetRoutes();
            GenerateRoutes("start", new List<Cave>(), new HashSet<string>(), Mode.Part2, false);
            return _routes.Count;
        }

        private void GenerateRoutes(string current, List<Cave> route, HashSet<string> smallCavesVisited, Mode mode, bool smallCaveVisitedTwice)
        {
            var routeCopy = new List<Cave>(route) { _caves[current] };

            if (_caves[current].IsSmall)
            {
                if (smallCavesVisited.Contains(current))
                    smallCaveVisitedTwice = true;
                else
                    smallCavesVisited.Add(current);
            }

            if (current == "end")
            {
                _routes.Add(routeCopy);
                return;
            }

            var linkedCaves = mode == Mode.Part1 || smallCaveVisitedTwice
                ? _caves[current].LinkedCaves.Where(k => !smallCavesVisited.Contains(k.Key) && !k.Value.IsStart).ToList()
                : _caves[current].LinkedCaves.Where(k => !k.Value.IsStart).ToList();

            foreach (var nextCave in linkedCaves)
            {
                GenerateRoutes(nextCave.Key, routeCopy, new HashSet<string>(smallCavesVisited), mode, smallCaveVisitedTwice);
            }
        }
    }

    namespace Day12Extensions
    {
        internal enum Mode
        {
            Part1,
            Part2
        }

        internal class Cave
        {
            public string Name { get; set; }
            public bool IsSmall => Name[0] >= 'a' && Name[0] <= 'z';
            public bool IsStart => Name == "start";
            public Dictionary<string, Cave> LinkedCaves { get; set; }

            public Cave(string name)
            {
                Name = name;
                LinkedCaves = new Dictionary<string, Cave>();
            }

            public void Link(Cave n) => LinkedCaves.Add(n.Name, n);

            public override string ToString() => Name;
        }
    }
}