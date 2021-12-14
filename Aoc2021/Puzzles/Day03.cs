namespace Aoc2021.Puzzles
{
    internal class Day03 : Puzzle
    {
        public override int Day => 3;
        protected override object RunPart1() => Part1();  //841526
        protected override object RunPart2() => Part2();  //4790390

        public Day03()
            : base("Inputs/Day03.txt") { }

        private object Part1()
        {
            var data = PuzzleInput.ToList();
            string gam = "", eps = "";

            for (int i = 0; i < data[0].Length; i++)
            {
                var moreOnes = HasMoreOnes(data, i);
                gam += moreOnes ? '1' : '0';
                eps += moreOnes ? '0' : '1';
            }

            return Convert.ToInt32(gam, 2) * Convert.ToInt32(eps, 2);
        }

        private object Part2()
        {
            List<string> oxy = PuzzleInput.ToList(), c02 = PuzzleInput.ToList();
            for (int i = 0; i < oxy[0].Length; i++)
            {
                DoRemovals(oxy, i, RemoveWhenMoreOnes: '0', RemoveWhenLessOnes: '1');
                DoRemovals(c02, i, RemoveWhenMoreOnes: '1', RemoveWhenLessOnes: '0');
            }

            return Convert.ToInt32(oxy[0], 2) * Convert.ToInt32(c02[0], 2);
        }

        private static void DoRemovals(List<string> data, int idx, char RemoveWhenMoreOnes, char RemoveWhenLessOnes)
        {
            if (data.Count == 1) return;

            var moreOnes = HasMoreOnes(data, idx);
            var filter = (moreOnes ? RemoveWhenMoreOnes : RemoveWhenLessOnes);
            data.RemoveAll(x => x[idx] == filter);
        }

        private static bool HasMoreOnes(List<string> data, int idx) =>
            data.Count(x => x[idx] == '1') >= data.Count(x => x[idx] == '0');
    }
}
