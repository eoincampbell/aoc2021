using Aoc2021;
namespace Aoc2021.Puzzles
{
    internal class Day05 : Puzzle
    {
        protected override string Title => "################ Day 05 ####################";
        protected override object RunPart1() => Process(); //4826 (5)
        protected override object RunPart2() => Process(skipDiagonals: false); //16793 (12)

        public Day05()
            : base("Inputs/Day05.txt")
            //: base("Inputs/Day05Sample.txt")
        {
            _coords = new Dictionary<(int, int), int>();
        }

        private Dictionary<(int, int), int> _coords;

        public object Process(bool skipDiagonals = true)
        {
            _coords = new Dictionary<(int, int), int>();

            foreach (var line in PuzzleInput)
            {
                //list -> tuple var deconstruction, see Extension methods.
                var (start, (end, _)) = line.Split(" -> ");
                var (sx, (sy, _)) = start.Split(",").Select(int.Parse).ToList();
                var (ex, (ey, _)) = end.Split(",").Select(int.Parse).ToList();

                //x's must be equal for a vert line, ys for a horizontal 
                if (skipDiagonals && sx != ex && sy != ey)
                    continue;

                //account for coordinates given in ascending vs. descending
                var xdiff = (sx < ex) ? 1 : ((sx > ex) ? -1 : 0);
                var ydiff = (sy < ey) ? 1 : ((sy > ey) ? -1 : 0);

                int x = sx, y = sy;
                do
                {
                    UpdateCoord(x, y);
                    x += xdiff;
                    y += ydiff;
                } while (x != ex || y != ey);

                //!= operator above will always skip last coord.
                if (x == ex && y == ey) UpdateCoord(x, y);
            }

            return _coords.Values.Count(x => x > 1);
        }

        private void UpdateCoord(int x, int y) =>
            _coords[(x, y)] = _coords.ContainsKey((x, y)) ? _coords[(x, y)] + 1 : 1;
    }
}