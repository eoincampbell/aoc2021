using Aoc2021.Puzzles.Day21Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Aoc2021.Puzzles
{
    internal class Day21 : Puzzle
    {
        public override int Day => 21;
        public override string Name => "Dirac Dice";
        protected override object RunPart1() => Part1(); //757770
        protected override object RunPart2() => Part2(); //712381680443927

        private readonly Player _p1;
        private readonly Player _p2;

        public Day21()
            : base("Inputs/Day21.txt")
        
        {
            _p1 = new Player("Player 1", 6, 0);
            _p2 = new Player("Player 2", 8, 0);
        }

        private object Part1()
        {
            int dice = 0, rolls = 0;
            var isP1 = true;
            var game = new Game(_p1 with { }, _p2 with { });

            while (game.GetWinner(1000) == 0)
            {
                int a = Roll(), b = Roll(), c = Roll();
                game = game.StepDeterministic(isP1, a + b + c);
                isP1 = !isP1;
            }

            var loser = Math.Min(game.P1.Score, game.P2.Score);
            return rolls * loser;

            int Roll()
            {
                if (++dice > 100) dice = 1;
                rolls++;
                return dice;
            }
        }

        private object Part2()
        {
            const int win = 21;
            var game = new Game(_p1 with { }, _p2 with { });
            var games = new HashSet<GameState>();
            games.Add(new GameState(game, 1));
            var isP1 = true;
            long p1Wins = 0, p2Wins = 0;
            int round = 1;

            while (games.Count > 0)
            {
                //Generate a CrossJoin set of all current games * the next 27 outcomes (7 of which are unique)
                //if there was no win state this would scale like 
                //1   :1
                //7   :27
                //49  :729
                //343 :19683
                //... and so on
                var cartesianProduct = new List<GameState>();
                foreach (var current in games)
                    foreach (var newOutcome in current.Game.StepQuantum(isP1))
                        cartesianProduct.Add(
                            new GameState(newOutcome.Game, current.Count * newOutcome.Count));

                //Take that cross join and then group it by the Unique GameStates (and sum the counts together)
                var next = cartesianProduct
                    .GroupBy(x => x.Game)
                    .Select(s => new GameState(s.Key, s.Sum(x => x.Count)))
                    .ToHashSet();

                //Identify the set of unique games within that set that now have a winer
                //pull them out, group the counts by the winner and put them in a tracker
                var newWins = next
                    .Where(w => w.Game.HasWinner(win))
                    .Select(s => new PlayerState(s.Game.GetWinner(win), s))
                    .ToList();

                //if there are any new wins this round, assign them to the players
                p1Wins += newWins.Where(w => w.Player == 1).Sum(w => w.Games.Count);
                p2Wins += newWins.Where(w => w.Player == 2).Sum(w => w.Games.Count);

                Console.WriteLine($"================================================");
                Console.WriteLine($"Round:              {round}");
                Console.WriteLine($"Total Unique Games: {next.LongCount()}");
                Console.WriteLine($"Total Games:        {next.Sum(x => x.Count)}");
                Console.WriteLine($"Player 1 Wins:      {p1Wins}");
                Console.WriteLine($"Player 2 Wins:      {p2Wins}");

                //And then remove those completed games from the tracker
                games = next.Except(newWins.Select(w => w.Games)).ToHashSet();

                isP1 = !isP1; //switch turns
                round++;
            }

            return Math.Max(p1Wins, p2Wins);
        }
    }

    namespace Day21Extensions
    {
        internal record Player(string Name, int Position, int Score = 0)
        {
            public Player Move(int steps)
            {
                var p = ((Position + steps - 1) % 10) + 1;
                return new Player(Name, p, Score + p);
            }

            public bool Wins(int threshold) => Score >= threshold;
        }

        internal record QuantumMove(int Moves, long Count);

        internal record GameState(Game Game, long Count);
        internal record PlayerState(int Player , GameState Games);

        internal record Game(Player P1, Player P2)
        {
            private static List<QuantumMove> QuantumMoves = GenerateQuantumMoves();

            private static List<QuantumMove> GenerateQuantumMoves()
            {
                var temp = new List<int>();
                for (var r1 = 1; r1 <= 3; r1++)
                    for (var r2 = 1; r2 <= 3; r2++)
                        for (var r3 = 1; r3 <= 3; r3++)
                            temp.Add(r1 + r2 + r3);

                return temp.GroupBy(x => x).Select(s => new QuantumMove(s.Key, s.LongCount())).ToList();
            }

            public int GetWinner(int max)
            {
                if (P1.Wins(max)) return 1;
                if (P2.Wins(max)) return 2;
                return 0;
            }
            public bool HasWinner(int max) => P1.Wins(max) || P2.Wins(max);

            public Game StepDeterministic(bool isP1, int dice)
            {
                return isP1
                    ? this with { P1 = P1.Move(dice) }
                    : this with { P2 = P2.Move(dice) };
            }

            public IEnumerable<GameState> StepQuantum(bool isP1)
            {
                return isP1
                    ? QuantumMoves.Select(m => new GameState(this with { P1 = P1.Move(m.Moves) }, m.Count))
                    : QuantumMoves.Select(m => new GameState(this with { P2 = P2.Move(m.Moves) }, m.Count));
            }
        }
    }
}