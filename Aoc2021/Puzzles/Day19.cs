using Aoc2021.Puzzles.Day19Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day19 : Puzzle
    {
        public override int Day => 19;
        public override string Name => "";
        protected override object RunPart1() => "***Incomplete";//Part1();   //
        protected override object RunPart2() => "***Incomplete";//Part2();   //

        public Day19()
            //: base("Inputs/Day19.txt")
            : base("Inputs/Day19Sample.txt")
        {

            _scanners = new List<Scanner>();
        }

        private List<Scanner> _scanners;

        private void LoadScanners()
        {
            Scanner s = default!;
            bool isOrigin = true;
            foreach(var line in PuzzleInput)
            {
                if (line.StartsWith("---"))
                {
                    s = new Scanner(line.Replace("---", "").Trim(), isOrigin);
                    isOrigin = false;
                    _scanners.Add(s);
                }
                else if (string.IsNullOrEmpty(line)) { } //NoOp
                else
                {
                    var (x, (y, (z, _))) = line.Split(",").Select(s => int.Parse(s)).ToList();
                    var c = new Coordinate3D(x, y, z);
                    s.AddBeacon(c);
                }
            }
        }

        public object Part1()
        {
            LoadScanners();
            for (int i = 0; i < _scanners.Count - 1; i++)
            {
                for (int j = 1; j < _scanners.Count; j++)
                {
                    if(i != j)
                    {
                        var overlappingCount = _scanners[i].BeaconDistances.Keys.Intersect(_scanners[j].BeaconDistances.Keys).Count();
                        if (overlappingCount >= 12)
                        {
                            Console.WriteLine($"[{_scanners[i]}] overlaps [{_scanners[j]}] ");
                            var offset = _scanners[i].GetOriginOffset(_scanners[j]);
                        }
                    }
                }
            }
            return -1;
        }

        public static object Part2()
        {
            return -1;
        }
    }

    namespace Day19Extensions
    {
        public class Scanner
        {
            public bool IsOrigin { get; set; }
            public Coordinate3D OriginOffset {get;set;}
            public string Name { get; set; }
            public List<Coordinate3D> Beacons { get; set; }
            public Dictionary<double, (Coordinate3D start, Coordinate3D end)> BeaconDistances { get; set; }
            public Scanner(string name, bool isOrigin)
            {
                Name = name;
                IsOrigin = isOrigin;
                if (isOrigin)
                    OriginOffset = new Coordinate3D(0, 0, 0);
                Beacons = new List<Coordinate3D>();
                BeaconDistances = new Dictionary<double, (Coordinate3D start, Coordinate3D end)>();
            }

            public void AddBeacon(Coordinate3D beacon)
            {
                if (Beacons.Any())
                {
                    foreach(var b in Beacons)
                    {
                        var dist = beacon.Distance(b);
                        BeaconDistances.Add(dist, (beacon, b));
                    }
                }
                Beacons.Add(beacon);
            }

            public override string ToString() => Name;

            public IEnumerable<Coordinate3D> GetOffsetBeacons(Coordinate3D offset)
            {
                foreach(var beacon in Beacons)
                {
                    yield return beacon - offset;
                }
            }

            public Coordinate3D GetOriginOffset(Scanner other)
            {
                
                foreach(var t in this.BeaconDistances)
                {
                    foreach(var s in other.BeaconDistances)
                    {
                        if(t.Key == s.Key)
                        {
                            Console.WriteLine(t.Value.start - s.Value.start);
                            Console.WriteLine(t.Value.end - s.Value.end);
                            Console.WriteLine(t.Value.start - s.Value.end);
                            Console.WriteLine(t.Value.end - s.Value.start);
                        }
                    }
                }

                return new Coordinate3D(0, 0, 0);
            }
        }

        public record Coordinate3D(int X, int Y, int Z)
        {
            public static implicit operator Coordinate3D((int x, int y, int z) a) => new(a.x, a.y, a.z);

            public static implicit operator (int x, int y, int z)(Coordinate3D a) => (a.X, a.Y, a.Z);
            public static Coordinate3D operator +(Coordinate3D a) => a;
            public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            public static Coordinate3D operator -(Coordinate3D a) => new(-a.X, -a.Y, -a.Z);
            public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b) => a + (-b);

            public double Distance(Coordinate3D other) => Math.Sqrt(Math.Pow(X - other.X,2) + Math.Pow(Y -other.Y,2) + Math.Pow(Z - other.Z,2));
            public int ManhattanDistance(Coordinate3D other) => (int)(Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z));
            public int ManhattanMagnitude() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

            //public static Coordinate3D[] GetNeighbors() => neighbors3D;
            

            public override int GetHashCode() => (137 * X + 149 * Y + 163 * Z);

            //private static readonly Coordinate3D[] neighbors3D =
            //{
            //    (-1,-1,-1),(-1,-1,0),(-1,-1,1),(-1,0,-1),(-1,0,0),(-1,0,1),(-1,1,-1),(-1,1,0),(-1,1,1),
            //    (0,-1,-1), (0,-1,0), (0,-1,1), (0,0,-1),          (0,0,1), (0,1,-1), (0,1,0), (0,1,1),
            //    (1,-1,-1), (1,-1,0), (1,-1,1), (1,0,-1), (1,0,0), (1,0,1), (1,1,-1), (1,1,0), (1,1,1)
            //};
        }

    }
}