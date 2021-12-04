using System.Diagnostics;

namespace Aoc2021.Puzzles
{
    internal abstract class Puzzle
    {
        protected abstract string Title { get; }

        protected IEnumerable<string> PuzzleInput { get; }

        public Puzzle(string filePath) => PuzzleInput = File.ReadLines(filePath);

        public string Run()
        {
            var msg = Title + Environment.NewLine;

            var sw = new Stopwatch();

            sw.Start();
            var p1 = RunPart1();
            sw.Stop();
            msg += ($"   Part 1 | Time: {sw.ElapsedMilliseconds} ms | Result: {p1}{Environment.NewLine}");

            sw.Reset();

            sw.Start();
            var p2 = RunPart2();
            sw.Stop();
            msg += $"   Part 2 | Time: {sw.ElapsedMilliseconds} ms | Result: {p2}";

            return msg;
        }

        protected abstract object RunPart1();
        protected abstract object RunPart2();
    }
}
