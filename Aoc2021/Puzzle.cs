namespace Aoc2021
{
    internal abstract class Puzzle
    {
        protected IEnumerable<string> PuzzleInput { get; private set; }
        public Puzzle(string filePath) => PuzzleInput = File.ReadLines(filePath);

        public void Run()
        {
            RunPart1().Print();
            RunPart2().Print();
        }

        protected abstract object RunPart1();
        protected abstract object RunPart2();
    }
}
