namespace Aoc2021.Puzzles
{
    internal class Day08 : Puzzle
    {
        public override int Day => 8;
        protected override object RunPart1() => Part1(); //321
        protected override object RunPart2() => Part2(); //1028926

        public Day08()
           : base("Inputs/Day08.txt")
            //: base("Inputs/Day08Sample.txt")
        { }

        private object Part1()
        {
            int count = 0;
            foreach(var line in PuzzleInput)
            {
                var (input, output, _) = line.Split("|").ToList();
                count += output.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Count(x => x.Length == 7 ||//eight 
                                x.Length == 4 ||//four
                                x.Length == 3 ||//seven 
                                x.Length == 2); //one
            }

            return count;
        }

        private object Part2()
        {
            int count = 0;
            foreach (var line in PuzzleInput)
                count += ProcessLineBetter(line);

            return count;
        }

        private static string FigureOutDigit(List<string> input, Func<string, bool> predicate)
        {
            var find = input.First(predicate);
            input.Remove(find);
            return new string(find.OrderBy(c=>c).ToArray()); //Added the order by for the final comparison step with the output
        }

        private static int ProcessLineBetter(string line)
        {
            var (input, output, _) = line.Split("|").ToList();

            var inputdigits = input.Split(" ").ToList();
            var outputdigits = output.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var digits = new string[10];

            digits[1] = FigureOutDigit(inputdigits, x => x.Length == 2); //1 only lights 2 segments
            digits[7] = FigureOutDigit(inputdigits, x => x.Length == 3); //7 only lights 3 segments
            digits[4] = FigureOutDigit(inputdigits, x => x.Length == 4); //4 only lights 4 segments
            digits[8] = FigureOutDigit(inputdigits, x => x.Length == 7); //8 lights all segments

            //2,3,5 all light 5 segments
            //9,6,0 all light 6 segments

            digits[3] = FigureOutDigit(inputdigits, x => x.Length == 5 && x.Intersect(digits[1]).Count() == 2); //only 3 shares both segments with 1
            digits[9] = FigureOutDigit(inputdigits, x => x.Length == 6 && x.Intersect(digits[3]).Count() == 5); //9 contains all 5 of 3's segments 
            digits[0] = FigureOutDigit(inputdigits, x => x.Length == 6 && x.Intersect(digits[7]).Count() == 3); //0 contains all 3 of 7's segments
            digits[6] = FigureOutDigit(inputdigits, x => x.Length == 6);                                        // is the only 6 segment remaining.
            digits[5] = FigureOutDigit(inputdigits, x => x.Length == 5 && x.Intersect(digits[4]).Count() == 3); //5 has a 3 sgement overlap with 4.
            digits[2] = FigureOutDigit(inputdigits, x => x.Length == 5);                                        //2 remains

            int a = FindOutputDigitInInput(digits, outputdigits[0]);
            int b = FindOutputDigitInInput(digits, outputdigits[1]);
            int c = FindOutputDigitInInput(digits, outputdigits[2]);
            int d = FindOutputDigitInInput(digits, outputdigits[3]);

            return (a * 1000) + (b * 100) + (c * 10) + d;
        }

        private static int FindOutputDigitInInput(string[] digits, string digit)
        {
            //input and output could be out of order... e.g abcde and decba are equivalent displays.
            var d = new string(digit.OrderBy(c => c).ToArray());

            for (int i = 0; i < digits.Length; i++)
                if (digits[i] == d) return i;

            return -1000000;
        }
    }
}