using System.Diagnostics;

namespace Aoc2021.Puzzles
{
    internal abstract class Puzzle
    {
        public static string Header => "  Day |  Part | Execution Time |          Result";

        protected IEnumerable<string> PuzzleInput { get; }

        protected Puzzle(string filePath) => PuzzleInput = File.ReadLines(filePath);

        public abstract int Day { get; }
        protected abstract object RunPart1();
        protected abstract object RunPart2();

        public string Run()
        {
            var msg = string.Empty;
            var sw = new Stopwatch();

            sw.Start();
            var p1 = RunPart1();
            sw.Stop();
            msg += ($"{Day,5:00} | {"1",5} | {sw.ElapsedTicks/ 10,11:#,###} µs | {p1,15}{Environment.NewLine}");

            sw.Reset();

            sw.Start();
            var p2 = RunPart2();
            sw.Stop();
            msg += $"{Day,5:00} | {"2",5} | {sw.ElapsedTicks / 10,11:#,###} µs | {p2,15}";

            return msg;
        }
    }
}
