using Aoc2021.Puzzles.Day11Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day11 : Puzzle
    {
        public Day11()
        : base("Inputs/Day11.txt")
        //: base("Inputs/Day11Sample.txt")
        //: base("Inputs/Day11Sample2.txt")
        {
            _map = PuzzleInput
                .Select(y => y.Select(x => int.Parse(x.ToString())))
                .To2DArray();
        }

        private static (int y, int x)[] CoordModifiers => new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        private static bool EnablePrinting => false;
        private readonly int[,] _map;
        private int MapWidth => _map.GetUpperBound(0);
        private int MapHeight => _map.GetUpperBound(1);
        private int _flashCount;
        //Toggle to Show State after each Step;

        private object Part1()
        {
            _flashCount = 0;
            _map.Dump(EnablePrinting, 0);

            for (int i = 1; i <= 100; i++) //steps 1-100
                Step(i);

            return _flashCount;
        }

        private object Part2()
        {
            for (int i = 101; i <= 400; i++) //steps 101-400
            {
                Step(i);
                if (BigFlash()) return i;
            }

            return -1;
        }

        private void Step(int i)
        {
            //Increase everyone by 1
            for (int y = 0; y <= MapHeight; y++ )
                for(int x = 0; x <= MapWidth; x++)
                    _map[y, x]++;

            //Find any >9's that havent yet been processed
            //they might get processed internally in the recursive loops so this is just a starting point and covers stragglers
            for (int y = 0; y <= MapHeight; y++)
                for (int x = 0; x <= MapWidth; x++)
                    if(_map[y,x] > 9)
                        Flash(y, x);

            //Find any -1's that have flashed and reset to 0;
            for (int y = 0; y <= MapHeight; y++)
                for (int x = 0; x <= MapWidth; x++)
                    if (_map[y, x] == -1)
                        _map[y, x] = 0;

            _map.Dump(EnablePrinting, i);
        }

        private void Flash(int y, int x)
        {
            //this guy doesn't flash, move on
            if (_map[y, x] <= 9)
                return;

            //Flash the current octopus and record it.
            _map[y, x] = -1;
            _flashCount++;

            foreach (var m in CoordModifiers)
            {
                //check that the modifer is in bounds or bail.
                if (!IsInBounds(y, x, m.y, m.x))
                    continue;

                int ny = y + m.y;
                int nx = x + m.x;

                //for anyone who hasn't flashed this step, bump their energy
                if (_map[ny, nx] != -1)
                    _map[ny, nx]++;

                //for any who are now over energy 9, recursively process them
                if (_map[ny, nx] > 9)
                    Flash(ny, nx);
            }
        }

        private bool IsInBounds(int y, int x, int my, int mx) =>
            y + my >= 0 && y + my <= MapHeight && x + mx >= 0 && x + mx <= MapWidth;

        private bool BigFlash()
        {
            int sum = 0;
            for (int y = 0; y <= MapHeight; y++)
                for (int x = 0; x <= MapWidth; x++)
                    sum += _map[y, x];

            return sum == 0;
        }

        protected override string Title => "################ Day 11 ####################";
        protected override object RunPart1() => Part1(); //1669  (Sample: 1656)
        protected override object RunPart2() => Part2(); //351   (Sample: 195)
    }

    namespace Day11Extensions
    {
        internal static class Extensions
        {
            internal static void Dump(this int[,] map, bool EnablePrinting, int step)
            {
                if (!EnablePrinting) return;

                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"After Step: {step}");
                for (int y = 0; y <= map.GetUpperBound(0); y++)
                {
                    for(int x = 0; x <= map.GetUpperBound(1); x++)
                        Console.Write(map[y, x]);

                    Console.WriteLine();
                }
            }
        }
    }
}