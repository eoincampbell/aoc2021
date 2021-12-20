using Aoc2021.Puzzles.Day20Extensions;
using System.Text;

namespace Aoc2021.Puzzles
{
    internal class Day20 : Puzzle
    {
        public override int Day => 20;
        public override string Name => "Trench Map";
        protected override object RunPart1() => RunAlgo(2);     //5065
        protected override object RunPart2() => RunAlgo(50);    //14790

        private Dictionary<Point, int> _map;
        private int[] _algo;
        private Box _box;
        private Box _bigBox;

        public static bool EnablePrinting => false;

        public Day20()
            : base("Inputs/Day20.txt")
            //: base("Inputs/Day20Sample.txt")
        {
            _algo = Array.Empty<int>();
            _map = new();
            _box = _bigBox = default!;
        }

        private void LoadInput()
        {
            var t = PuzzleInput.Skip(2).ToList();
            _algo = PuzzleInput.First().Select(s => s == '#' ? 1 : 0).ToArray();
            _map = new();
            _box = new(0, 0, t[0].Length, t.Count);
            _bigBox = new(-50, -50, t[0].Length+50, t.Count+50);
            for (int y = _box.MinY; y < _box.MaxY; y++)
            {
                for (int x = _box.MinX; x < _box.MaxX; x++)
                    _map.Add(new(x, y), t[y][x] == '#' ? 1 : 0);
            }
        }

        public object RunAlgo(int steps)
        {
            LoadInput();
            for (int i = 0; i < steps; i++)
            {
                _map.Dump(_bigBox);
                _box = _box.Expand();          //Expand the box by one index in each direction
                                               //since that will be effected by the current edge of the box
                int def = i % 2 == 0 ? 0 : 1;  //THIS FUCKING GOTCHA...
                /*             
                So outside the known area is definitely only dark on step zero.
                It might "flicker" based on the input algo for a
                series of black cells if index 0 ("000000000")
                resolves to a '#' 

                so a cell that's way out in the void, that's dark, and 
                surround by dark, will flip to bright (if algo[0] = #)
                then all the void is bright, and on the next iteration 
                a cells thats way out in the void, that's bright, and surrounded 
                by brights, will flip to dark (if algo[512] = .)
                */
                var tmpMap = new Dictionary<Point, int>();
                for (int y = _box.MinY; y < _box.MaxY; y++)
                {
                    for (int x = _box.MinX; x < _box.MaxX; x++)
                    {
                        var p = new Point(x, y);
                        var ss = string.Empty;
                        foreach (var n in p.Neighbors())
                            ss += _map.GetValueOrDefault(n, def);   //Get values from known area, or use flickering default

                        var algoIdx = Convert.ToInt32(ss, 2);       //Get new value for that cell from algo
                        tmpMap[p] = _algo[algoIdx];
                    }
                }
                _map = tmpMap;
            }

            _map.Dump(_bigBox);
            return _map.Values.Sum();
        }
    }

    namespace Day20Extensions
    {
        internal static class Extensions
        {
            public static List<Point> Neighbors(this Point val) => new()
            {
                new(val.X - 1 , val.Y - 1), //NW
                new(val.X     , val.Y - 1), //N
                new(val.X + 1 , val.Y - 1), //NE
                new(val.X - 1 , val.Y),     //W
                new(val.X     , val.Y),     //SELF/CENTER
                new(val.X + 1 , val.Y),     //E
                new(val.X - 1 , val.Y + 1), //SW
                new(val.X     , val.Y + 1), //S
                new(val.X + 1 , val.Y + 1), //SE
            };

            internal static void Dump(this Dictionary<Point, int> map, Box box)
            {
                if (!Day20.EnablePrinting) return;
                var sb = new StringBuilder();
                for (int y = box.MinY; y < box.MaxY; y++)
                {
                    for (int x = box.MinX; x < box.MaxX; x++)
                    {
                        sb.Append(map.GetValueOrDefault(new(x, y), 0) == 1 ? '█' : ' ');
                    }
                    sb.AppendLine();
                }
                Thread.Sleep(200);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(sb.ToString());
            }
        }

        public record Point(int X, int Y);

        public record Box(int MinX, int MinY, int MaxX, int MaxY)
        {
            public Box Expand() => new (MinX - 1, MinY - 1, MaxX + 1, MaxY + 1);
        }
    }
}