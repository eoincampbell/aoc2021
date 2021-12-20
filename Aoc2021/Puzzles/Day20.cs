using Aoc2021.Puzzles.Day20Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day20 : Puzzle
    {
        public override int Day => 20;
        public override string Name => "";
        protected override object RunPart1() => RunAlgo(2);     //
        protected override object RunPart2() => RunAlgo(50);    //

        private Dictionary<Coordinate2D, int> _map;
        private int[] _algo;
        private int _minx;
        private int _miny;
        private int _maxx;
        private int _maxy;

        public static bool EnablePrinting => false;

        public Day20()
            : base("Inputs/Day20.txt")
            //: base("Inputs/Day20Sample.txt")
        {
            _algo = Array.Empty<int>();
            _map = new();
        }

        private void LoadInput()
        {
            var raw = PuzzleInput.ToArray();
            _algo = raw[0].ToCharArray().Select(s => s == '#' ? 1 : 0).ToArray();

            var inputs = raw[2..].ToList();

            _map = new Dictionary<Coordinate2D, int>();

            _miny = 0;
            _minx = 0;
            _maxy = inputs.Count;
            _maxx = inputs[0].Length;

            for (int y = _miny; y < _maxy; y++)
            {
                for (int x = _minx; x < _maxx; x++)
                {
                    _map.Add(new Coordinate2D(x, y), inputs[y][x] == '#' ? 1 : 0);
                }
            }
        }

        public object RunAlgo(int steps)
        {
            LoadInput();
            for (int i = 0; i < steps; i++)
            {
                _map.Dump(_miny, _minx, _maxy, _maxx);
                //extend the infinte map in all directions by an index.
                _miny--;
                _minx--;
                _maxy++;
                _maxx++;

                int def = i % 2 == 0 ? 0 : 1;

                var tmpMap = new Dictionary<Coordinate2D, int>();

                for (int y = _miny; y < _maxy; y++)
                {
                    for (int x = _minx; x < _maxx; x++)
                    {
                        var cc = new Coordinate2D(x, y);
                        var nn = cc.Neighbors();
                        var ss = "";

                        foreach(var n in nn)
                        {
                            
                            ss += _map.GetValueOrDefault(n, def);
                        }

                        var algoIdx = Convert.ToInt32(ss, 2);
                        var value = _algo[algoIdx];
                        tmpMap[cc] = value;
                    }
                }

                _map = tmpMap;
            }
            _map.Dump(_miny, _minx, _maxy, _maxx);
            return _map.Values.Sum();
        }
    }

    namespace Day20Extensions
    {
        internal static class Extensions
        {
            public static List<Coordinate2D> Neighbors(this Coordinate2D val)
            {
                return new List<Coordinate2D>()
                {
                    new Coordinate2D(val.x - 1, val.y - 1), //NW
                    new Coordinate2D(val.x, val.y - 1),     //N
                    new Coordinate2D(val.x + 1, val.y - 1), //NE

                    new Coordinate2D(val.x - 1, val.y),     //W
                    new Coordinate2D(val.x, val.y),
                    new Coordinate2D(val.x + 1, val.y),     //E

                    new Coordinate2D(val.x - 1, val.y + 1), //SW
                    new Coordinate2D(val.x, val.y + 1),     //S
                    new Coordinate2D(val.x + 1, val.y + 1), //SE
                };
            }

            internal static void Dump(this Dictionary<Coordinate2D, int> map, int miny, int minx, int maxy, int maxx)
            {
                if (!Day20.EnablePrinting) return;

                Console.WriteLine("-------------------------------------------");
                for (int y = miny; y < maxy; y++)
                {
                    for (int x = minx; x < maxx; x++)
                    {
                        var c = new Coordinate2D(x, y);
                        var val = map.GetValueOrDefault(c, 0);
                        Console.Write(val == 1 ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }

        public class Coordinate2D
        {
            public static readonly Coordinate2D origin = new(0, 0);
            public static readonly Coordinate2D unit_x = new(1, 0);
            public static readonly Coordinate2D unit_y = new(0, 1);
            public readonly int x;
            public readonly int y;

            public Coordinate2D(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Coordinate2D((int x, int y) coord)
            {
                this.x = coord.x;
                this.y = coord.y;
            }

            public Coordinate2D RotateCW(int degrees, Coordinate2D center)
            {
                Coordinate2D offset = center - this;
                return center + offset.RotateCW(degrees);
            }
            public Coordinate2D RotateCW(int degrees)
            {
                return ((degrees / 90) % 4) switch
                {
                    0 => this,
                    1 => RotateCW(),
                    2 => -this,
                    3 => RotateCCW(),
                    _ => this,
                };
            }

            private Coordinate2D RotateCW()
            {
                return new Coordinate2D(y, -x);
            }

            public Coordinate2D RotateCCW(int degrees, Coordinate2D center)
            {
                Coordinate2D offset = center - this;
                return center + offset.RotateCCW(degrees);
            }
            public Coordinate2D RotateCCW(int degrees)
            {
                return ((degrees / 90) % 4) switch
                {
                    0 => this,
                    1 => RotateCCW(),
                    2 => -this,
                    3 => RotateCW(),
                    _ => this,
                };
            }

            private Coordinate2D RotateCCW()
            {
                return new Coordinate2D(-y, x);
            }

            public static Coordinate2D operator +(Coordinate2D a) => a;
            public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b) => new(a.x + b.x, a.y + b.y);
            public static Coordinate2D operator -(Coordinate2D a) => new(-a.x, -a.y);
            public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b) => a + (-b);
            public static Coordinate2D operator *(int scale, Coordinate2D a) => new(scale * a.x, scale * a.y);
            public static bool operator ==(Coordinate2D a, Coordinate2D b) => (a.x == b.x && a.y == b.y);
            public static bool operator !=(Coordinate2D a, Coordinate2D b) => (a.x != b.x || a.y != b.y);

            public static implicit operator Coordinate2D((int x, int y) a) => new(a.x, a.y);

            public static implicit operator (int x, int y)(Coordinate2D a) => (a.x, a.y);

            public int ManDistance(Coordinate2D other)
            {
                int x = Math.Abs(this.x - other.x);
                int y = Math.Abs(this.y - other.y);
                return x + y;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                if (obj.GetType() != typeof(Coordinate2D)) return false;
                return this == (Coordinate2D)obj;
            }

            public override int GetHashCode()
            {
                return (100 * x + y).GetHashCode();
            }

            public override string ToString()
            {
                return string.Concat("(", x, ", ", y, ")");
            }

        }

    }
}