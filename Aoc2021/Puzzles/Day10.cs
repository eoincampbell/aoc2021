using Aoc2021.Puzzles.Day10EXtensions;

namespace Aoc2021.Puzzles
{
    internal class Day10 : Puzzle
    {
        protected override string Title => "################ Day 10 ####################";
        protected override object RunPart1() => Part1(); //240123
        protected override object RunPart2() => Part2(); //3260812321
                                                         
        public Day10()
            : base("Inputs/Day10.txt")
            //: base("Inputs/Day10Sample.txt")
        {
            _incompleteLines = new List<Stack<char>>();
            _incompleteScores = new List<long>();
        }

        private readonly List<Stack<char>> _incompleteLines;
        private readonly List<long> _incompleteScores;
        private static char[] Openers => new char[] { '(', '[', '{', '<' };
        private static char[] Closers => new char[] { '>', '}', ']', ')' };

        private object Part1()
        {
            int total = 0;
            foreach(var line in PuzzleInput)
            {
                var s = new Stack<char>();
                var isIncomplete = true;

                foreach (var c in line)
                {
                    if (Openers.Contains(c))
                        s.Push(c);
                    
                    if (Closers.Contains(c) && c != s.Pop().Opposite())
                    {
                        total += c.WrongValue();
                        isIncomplete = false;
                        break;
                    }
                }

                if (isIncomplete)_incompleteLines.Add(s);
            }
            return total;
        }

        private object Part2()
        {
            foreach(var stack in _incompleteLines)
            {
                long total = 0;
                while (stack.Count > 0)
                    total = (total * 5) + stack.Pop().Opposite().AutoCompleteValue();

                _incompleteScores.Add(total);
            }

            return _incompleteScores.OrderBy(o => o).Skip(_incompleteScores.Count / 2).First();
        }
    }

    namespace Day10EXtensions
    {
        internal static class Extensions
        {
            internal static char Opposite(this char c) => c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                _ => '|'
            };

            internal static int WrongValue(this char c) => c switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => -1
            };

            internal static int AutoCompleteValue(this char c) => c switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => -1
            };
        }
    }
}