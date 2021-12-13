using Aoc2021.Puzzles.Day13Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day13 : Puzzle
    {
        protected override int Day => 13;
        protected override object RunPart1() => Part1(); //  666
        protected override object RunPart2() => Part2(); //  CJHAZHKU (97 Chars)

        private HashSet<Coordinate> _coordinates;
        private List<Instruction> _instructions;

        public Day13()
            : base("Inputs/Day13.txt")
            //: base("Inputs/Day13Sample.txt") 
        {
            _coordinates = new HashSet<Coordinate>();
            _instructions = new List<Instruction>();
        }

        private void ResetContents()
        {
            _coordinates = new HashSet<Coordinate>();
            _instructions = new List<Instruction>();
            var addCoordinates = true;
            foreach (var line in PuzzleInput)
            {
                if (string.IsNullOrEmpty(line))
                {
                    addCoordinates = false;
                }
                else if (addCoordinates)
                {
                    var (x, y, _) = line.Split(',').Select(x => int.Parse(x)).ToList();
                    _coordinates.Add(new Coordinate(x, y));
                }
                else
                {
                    var (axis, position, _) = line.Replace("fold along ", "").Split("=").ToList();
                    _instructions.Add(new Instruction(axis[0], int.Parse(position)));
                }
            }
        }

        private object Part1()
        {
            ResetContents();
            var instruction = _instructions[0];
            var newCoordinates = new HashSet<Coordinate>();
            foreach (var c in _coordinates) newCoordinates.Add(Fold(instruction, c));
            return newCoordinates.Count;
        }

        private static Coordinate Fold(Instruction i, Coordinate c)
        {
            if (i.Axis == 'x' && c.X > i.Position)
                return new Coordinate(c.X - ((c.X - i.Position) * 2), c.Y);
            else if (i.Axis == 'y' && c.Y > i.Position)
                return new Coordinate(c.X, c.Y - ((c.Y - i.Position) * 2));
            else
                return c;
        }

        private object Part2()
        {
            ResetContents();
            foreach (var i in _instructions)
            {
                var newCoordinates = new HashSet<Coordinate>();
                foreach (var c in _coordinates) newCoordinates.Add(Fold(i, c));
                _coordinates = newCoordinates;
            }
            _coordinates.Dump();
            return _coordinates.Count;
        }
    }

    namespace Day13Extensions
    {
        internal static class Extensions
        {
            public static void Dump(this HashSet<Coordinate> coordinates)
            {
                int maxY = coordinates.Max(c => c.Y), maxX = coordinates.Max(c => c.X);

                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        Console.Write((coordinates.Contains(new Coordinate(x, y)) ? "##" : "  ") +
                            (x != maxX ? string.Empty : Environment.NewLine));
                    }
                }
            }
        }

        internal record Coordinate (int X, int Y);
        internal record Instruction(char Axis, int Position);
    }
}