using System.Diagnostics;

namespace Aoc2021.Puzzles
{
    internal abstract class Puzzle
    {
        public static string Header => "  Day | Name                      |  Part | Execution Time | Result";
        public static string Seperator => "------+---------------------------+-------+----------------+-------------------------" + Environment.NewLine;
        public static string Formatter => "{0,5:00} | {1, -25} | {2,5} | {3,11:#,###} µs | {4,-15}{5}";

        protected Puzzle(string filePath) => PuzzleInput = File.ReadLines(filePath);

        public abstract int Day { get; }
        public abstract string Name { get; }
        protected abstract object RunPart1();
        protected abstract object RunPart2();

        protected IEnumerable<string> PuzzleInput { get; }

        public string Run()
        {
            var msg = Seperator;
            var sw = new Stopwatch();

            sw.Start();
            var p1 = RunPart1();
            sw.Stop();
            msg += string.Format(Formatter, Day, Name, 1, sw.ElapsedTicks / 10, p1, Environment.NewLine);
            sw.Reset();

            sw.Start();
            var p2 = RunPart2();
            sw.Stop();
            msg += string.Format(Formatter, "", "", 2, sw.ElapsedTicks / 10, p2, string.Empty);

            return msg;
        }
    }
}
