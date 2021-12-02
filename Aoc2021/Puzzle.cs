using System.Diagnostics;

namespace Aoc2021
{
    internal abstract class Puzzle
    {
        protected abstract string Title { get; }

        protected IEnumerable<string> PuzzleInput { get; private set; }

        public Puzzle(string filePath) => PuzzleInput = File.ReadLines(filePath);

        public void Run()
        {
            Title.Print();

            var sw = new Stopwatch();

            sw.Start();
            var p1 = RunPart1();
            sw.Stop();
            Console.WriteLine($"   Part 1 | Time: {sw.ElapsedMilliseconds} ms | Result: {p1}");

            sw.Reset();
            sw.Start();
            var p2 = RunPart2();
            sw.Stop();
            Console.WriteLine($"   Part 2 | Time: {sw.ElapsedMilliseconds} ms | Result: {p2}");
        }

        protected abstract object RunPart1();
        protected abstract object RunPart2();
    }
}
