using Aoc2021.Puzzles.Day21Extensions;
namespace Aoc2021.Puzzles
{
    internal class Day21 : Puzzle
    {
        public override int Day => 21;
        public override string Name => "Dirac Dice";
        protected override object RunPart1() => Play(); //757770
        protected override object RunPart2() => -1;

        private readonly Player _p1;
        private readonly Player _p2;
        private int _dice;
        private int _rollCount;

        public Day21()
            : base("Inputs/Day21.txt")
        
        {
            _dice = 0;
            //Sample Input
            //_p1 = new Player(4, 0);
            //_p2 = new Player(8, 0);


            //Real Input
            _p1 = new Player(6, 0);
            _p2 = new Player(8, 0);

            _rollCount = 0;
        }

        public int Roll()
        {
            _dice++;
            _rollCount++;
            if (_dice == 101) _dice = 1;
            return _dice;
        }

        public void Advance(ref int _pos, int dice)
        {
            _pos += dice;

            _pos--;
            _pos %= 10;
            _pos++;
        }

        public object Play()
        {
            while(true)
            {
                int a = Roll(), b = Roll(), c = Roll();
                _p1.Advance(a + b + c);
                Console.WriteLine($"Player 1 rolls {a}+{b}+{c} and moves to space {_p1.Position} for a total score of {_p1.Score}");
                if(_p1.Score >= 1000)
                    return _p2.Score * _rollCount;

                int d = Roll(), e = Roll(), f = Roll();
                _p2.Advance(d + e + f);
                Console.WriteLine($"Player 2 rolls {d}+{e}+{f} and moves to space {_p2.Position} for a total score of {_p2.Score}");
                if (_p2.Score >= 1000)
                    return _p1.Score * _rollCount;
                
            }
        }
    }

    namespace Day21Extensions
    {
        public record Player
        {
            public int Position { get; private set; }
            public int Score { get; private set; }
            public Player(int position, int score)
            {
                Position = position;
                Score = score;
            }

            public void Advance(int dice)
            {
                Position += dice;
                Position = ((Position - 1) % 10) + 1;
                Score += Position;
            }
        }

        internal static class Extensions
        {
              
        }
    }
}