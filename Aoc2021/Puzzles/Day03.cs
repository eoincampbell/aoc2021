
namespace Aoc2021.Puzzles
{
    internal class Day03 : Puzzle
    {
        public Day03() : 
            base("Inputs/Day03.txt")
            //base("Inputs/Day03Sample.txt")
        {
        }

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
            var oxy = PuzzleInput.ToList();
            var c02 = PuzzleInput.ToList();

            for (int i = 0; i < oxy[0].Length; i++)
            {
                DoRemovals(oxy, i, RemoveWhenMoreOnes: '0', RemoveWhenLessOnes: '1');
                DoRemovals(c02, i, RemoveWhenMoreOnes: '1', RemoveWhenLessOnes: '0');
            }

            return Convert.ToInt32(oxy[0], 2) * Convert.ToInt32(c02[0], 2);
        }

        private void DoRemovals(List<string> data, int idx, char RemoveWhenMoreOnes, char RemoveWhenLessOnes)
        {
            var moreOnes = HasMoreOnes(data, idx);
            var filter = (moreOnes ? RemoveWhenMoreOnes : RemoveWhenLessOnes);
            if (data.Count > 1)
                data.RemoveAll(x => x[idx] == filter);
        }

        private static bool HasMoreOnes(List<string> data, int idx) 
            => (data.Count(x => x[idx] == '1') >= data.Count - data.Count(x => x[idx] == '1'));

        protected override string Title => "################ Day 03 ####################";
        protected override object RunPart1() => Part1();  //841526
        protected override object RunPart2() => Part2();  //4790390
    }
}
