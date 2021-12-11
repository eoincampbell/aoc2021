using System.Linq;
using Aoc2021.Puzzles.Day11Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day11 : Puzzle
    {
        //Toggle to Show State after each Step;
        public static bool EnablePrinting => false;

        public Day11()
            : base("Inputs/Day11.txt")
        {
            _map = PuzzleInput.Select(y => y.Select(x => int.Parse(x.ToString()))).To2DArray();
        }

        private static (int y, int x)[] CoordModifiers => new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        private readonly int[,] _map;
        private int MapWidth => _map.GetUpperBound(0);
        private int MapHeight => _map.GetUpperBound(1);
        private int _flashCount;

        private object Part1()
        {
            _map.Dump(0);
            for (var step = 1; step <= 100; step++) //steps 1-100
                Step(step);

            return _flashCount;
        }

        private object Part2()
        {
            var step = 100;
            do
            {
                Step(step++);
            } while (!BigFlash());
            return step;
        }

        private void Step(int i)
        {
            //Increase everyone by 1
            for (int y = 0; y <= MapHeight; y++)
                for (int x = 0; x <= MapWidth; x++)
                    _map[y, x]++;

            //Find any >9's that havent yet been processed
            //they might get processed internally in the recursive loops so this is just a starting point and covers stragglers
            for (int y = 0; y <= MapHeight; y++)
                for (int x = 0; x <= MapWidth; x++)
                    if(_map[y, x] > 9)
                        Flash(y, x);

            _map.Dump(i);
        }

        private void Flash(int y, int x)
        {
            //this guy doesn't flash, move on
            if (_map[y, x] <= 9) return;

            //Flash the current octopus and record it.
            _map[y, x] = 0;
            _flashCount++;

            foreach (var m in CoordModifiers)
            {
                if (!IsInBounds(y, x, m.y, m.x)) continue; //check that the modifer is in bounds or bail.

                int ny = y + m.y, nx = x + m.x;

                if (_map[ny, nx] != 0) _map[ny, nx]++; //increase energy of non-flashed

                if (_map[ny, nx] > 9) Flash(ny, nx); //flash if ready to
            }
        }

        private bool IsInBounds(int y, int x, int my, int mx) =>
            y + my >= 0 &&
            y + my <= MapHeight &&
            x + mx >= 0 &&
            x + mx <= MapWidth;

        private bool BigFlash() => _map.Cast<int>().Sum() == 0;
        

        protected override string Title => "################ Day 11 ####################";
        protected override object RunPart1() => Part1(); //1669  (Sample: 1656)
        protected override object RunPart2() => Part2(); //351   (Sample: 195)
    }

    namespace Day11Extensions
    {
        internal static class Extensions
        {
            internal static void Dump(this int[,] map, int step)
            {
                if (!Day11.EnablePrinting) return;

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