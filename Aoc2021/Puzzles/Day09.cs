namespace Aoc2021.Puzzles
{
    internal class Day09 : Puzzle
    {
        public Day09()
        : base("Inputs/Day09.txt")
        //: base("Inputs/Day09Sample.txt")
        {
            _map = PuzzleInput
                .Select(x => x.Select(y => int.Parse(y.ToString())))
                .To2DArray();

            _basins = new Dictionary<(int x, int y), Dictionary<(int x, int y), bool>>();
        }

        private readonly int[,] _map;
        private readonly Dictionary<(int x, int y), Dictionary<(int x, int y), bool>> _basins;
        //key   ->  bottom of basin,
        //value ->  all cells in the basin and whether they've been processed or not.

        private int MapWidth => _map.GetUpperBound(0);
        private int MapHeight => _map.GetUpperBound(1);

        private int Part1ProcessCell(int x, int y)
        {
            var n = (y > 0          && _map[x, y] < _map[x, y - 1]) || y == 0;
            var s = (y < MapHeight  && _map[x, y] < _map[x, y + 1]) || y == MapHeight;
            var w = (x > 0          && _map[x, y] < _map[x - 1, y]) || x == 0;
            var e = (x < MapWidth   && _map[x, y] < _map[x + 1, y]) || x == MapWidth;

            return (n && s && w && e) ? (_map[x, y] + 1) : -1;
        }

        private IEnumerable<(int x, int y)> Part2ProcessCell(int x, int y)
        {
            if (y > 0           && _map[x, y] < _map[x, y - 1] && _map[x, y - 1] != 9)  yield return (x, y - 1); //north
            if (y < MapHeight   && _map[x, y] < _map[x, y + 1] && _map[x, y + 1] != 9)  yield return (x, y + 1); //south
            if (x > 0           && _map[x, y] < _map[x - 1, y] && _map[x - 1, y] != 9)  yield return (x - 1, y); //west
            if (x < MapWidth    && _map[x, y] < _map[x + 1, y] && _map[x + 1, y] != 9)  yield return (x + 1, y); //east
        }

        private object Part1()
        {
            int count = 0;
            for (int x = 0; x <= MapWidth; x++)
            {
                for (int y = 0; y <= MapWidth; y++)
                {
                    var c = Part1ProcessCell(x, y);
                    if (c > 0)
                    {
                        count += c;
                        _basins.Add((x, y), new Dictionary<(int x, int y), bool>());
                    }
                }
            }
            return count;
        }

        private object Part2()
        {
            foreach(var b in _basins)
            {
                var d = _basins[b.Key];                             //Grab the cells that make up this basin
                d.Add(b.Key, false);                                //Add itself to the basin
                while(d.Any(c => !c.Value))                         //Keep processing this basin for more adjacent cells if there is an unprocessed cell
                {
                    var (cx,cy) = d.First(x => !x.Value).Key;       //Grab an unprocessed cell
                    var linkedCells = Part2ProcessCell(cx, cy);     //Find all adjacent non-height-9 cells
                    d[(cx, cy)] = true;                             //Now onsider this cell processed

                    foreach (var (lcx, lcy) in linkedCells)         //foreach higher adjacent cell found
                    {
                        if (!d.ContainsKey((lcx, lcy)))             //add it to this basin if it hasn't already been added
                        {
                            d.Add((lcx, lcy), false);
                        }
                    }
                }
            }

            //order basins by size, grab top 3, and multiply
            return _basins
                .Values
                .Select(b => b.Count)
                .OrderByDescending(o => o)
                .Take(3)
                .Product();
        }

        protected override string Title => "################ Day 09 ####################";
        protected override object RunPart1() => Part1(); //532
        protected override object RunPart2() => Part2(); //1110780

    }
}