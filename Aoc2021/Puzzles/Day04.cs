namespace Aoc2021.Puzzles
{
    internal class Day04 : Puzzle
    {
        public override int Day => 4;
        public override string Name => "Giant Squid";
        //a `b => false` predicate will always result in a zero, so this will return as soon as the first board BINGOs
        //a `b => !b.Bingo` predicate will only return a zero count after the last card BINGOs 
        protected override object RunPart2() => PlayBingo(b => !b.Bingo);   //7296
        protected override object RunPart1() => PlayBingo(_ => false);      //89001

        public Day04()
            : base("Inputs/Day04.txt") {
            _drum = Array.Empty<int>();
            _boards = new List<Board>();
            ProcessInput();
        }

        private int[] _drum;
        private List<Board> _boards;

        private void ProcessInput()
        {
            var input = PuzzleInput.ToList();

            _drum = input[0].Split(",").Select(x => int.Parse(x)).ToArray();
            _boards = new List<Board>();

            for (int i = 1; i < input.Count; i += 6)
            {
                var values = new List<int>();
                for (int j = 1; j < 6; j++)
                {
                    values.AddRange(input[i + j]
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x)));
                }

                var b = new Board(values);

                _boards.Add(b);
            }
        }

        private object PlayBingo(Func<Board, bool> predicate)
        {
            foreach (var b in _boards) b.Reset();

            foreach (var number in _drum)
            {
                foreach (var board in _boards)
                {
                    board.MarkCard(number);
                    if (board.Bingo && !_boards.Any(predicate))
                        return board.SumOfLeftOvers * number;
                }
            }

            return -1;
        }

        private class Board
        {
            private record BoardNumber(int Number, bool Marked)
            {
                public bool Marked { get; set; } = Marked;
            }

            private readonly List<BoardNumber> _values;

            public Board(List<int> values) =>
                _values = values.ConvertAll(x => new BoardNumber(x, false));

            public void MarkCard(int number)
            {
                var x = _values.Find(x => x.Number == number);
                if(x != null) x.Marked = true;
            }

            public void Reset()
            {
                foreach (var v in _values) v.Marked = false;
            }

            public int SumOfLeftOvers => _values.Sum(x => x.Marked ? 0 : x.Number);

            public bool Bingo
            {
                get
                {
                    return (_values[0].Marked && _values[1].Marked && _values[2].Marked && _values[3].Marked && _values[4].Marked) ||
                        (_values[5].Marked && _values[6].Marked && _values[7].Marked && _values[8].Marked && _values[9].Marked) ||
                        (_values[10].Marked && _values[11].Marked && _values[12].Marked && _values[13].Marked && _values[14].Marked) ||
                        (_values[15].Marked && _values[16].Marked && _values[17].Marked && _values[18].Marked && _values[19].Marked) ||
                        (_values[20].Marked && _values[21].Marked && _values[22].Marked && _values[23].Marked && _values[24].Marked) ||
                        (_values[0].Marked && _values[5].Marked && _values[10].Marked && _values[15].Marked && _values[20].Marked) ||
                        (_values[1].Marked && _values[6].Marked && _values[11].Marked && _values[16].Marked && _values[21].Marked) ||
                        (_values[2].Marked && _values[7].Marked && _values[12].Marked && _values[17].Marked && _values[22].Marked) ||
                        (_values[3].Marked && _values[8].Marked && _values[13].Marked && _values[18].Marked && _values[23].Marked) ||
                        (_values[4].Marked && _values[9].Marked && _values[14].Marked && _values[19].Marked && _values[24].Marked);
                }
            }
        }
    }
}
