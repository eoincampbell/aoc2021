using Aoc2021.Puzzles.Day15Extensions;
namespace Aoc2021.Puzzles
{
    internal class Day15 : Puzzle
    {
        public override int Day => 15;
        protected override object RunPart1() => Run();      //388   (Sample: 40)
        protected override object RunPart2() => Run(5,5);   //2819  (Sample: 315)

        public Day15()
            : base("Inputs/Day15.txt")
            //: base("Inputs/Day15Sample.txt")
        {
            _input = PuzzleInput
                .Select(x => x.Select(y => int.Parse(y.ToString())))
                .To2DArray();
        }

        private readonly int[,] _input;
        private Dictionary<Node, int> _graph = new();
        private int InputWidth => _input.GetUpperBound(0) + 1;
        private int InputHeight => _input.GetUpperBound(1) + 1;
        private int GraphWidth(int tiles) => InputWidth * tiles;
        private int GraphHeight(int tiles) => InputHeight * tiles;

        private void ConfigureMap(int hTitles = 1, int vTiles = 1)
        {
            _graph = new Dictionary<Node, int>();

            for (int y = 0; y < GraphHeight(vTiles); y++)
            {
                for (int x = 0; x < GraphWidth(hTitles); x++)
                {
                    int inc = (y / InputHeight) + (x / InputWidth);
                    int yOrig = y % InputHeight, xOrig = x % InputWidth;
                    int value = _input[xOrig, yOrig] + inc;
                    _graph[(x, y)] = value > 9 ? value - 9 : value;
                }
            }
        }

        private object Run(int hTitles = 1, int vTiles = 1)
        {
            ConfigureMap(hTitles, vTiles);
            var src = (0, 0);
            var tgt = (GraphWidth(hTitles) - 1, GraphHeight(vTiles) - 1);
            return GetShortestPathTo(src, tgt);
        }

        private int GetShortestPathTo(Node source, Node target)
        {
            var route = AStar(source, target);      //Generate Route
            return route[1..].Sum(node => _graph[node]);  //Calc Path excluding starting node
        }

        private Node[] AStar(Node source, Node target)
        {
            //See:  https://brilliant.org/wiki/a-star-search/ (incl. Python Pseudo Code)
            //Uses Manhattan Distance for Est.

            var fn = new Dictionary<Node, int> { [source] = 0 };        //Tracker for the Node "Estimates"
            var gn = new Dictionary<Node, int> { [source] = 0 };        //Tracker for the Node "Scores"
            var open = new PriorityQueue<Node, long>();                 //Create a queue to process all our nodes.
            open.Enqueue(source, fn[source]);                           //This is a priority so nodes will pop in order of accumulated distance with lowest score
            var processed = new Dictionary<Node, Node>();               //keep a dictionary for tracking process nodes and the route where they came from

            while (open.TryDequeue(out var cur, out long _))            //keep popping nodes... this queue will grow as 
            {                                                           //neighbours of the start point are added.
                if (cur.Equals(target))
                    return ReconstructPath(processed, cur);             //exit condition, return recreate path from our closed node list

                foreach (var n in cur.Neighbours())                     //otherwise, iterate over the neighbours (which might be out of bounds)
                {
                    var score = gn[cur] +
                        _graph.GetValueOrDefault(n, int.MaxValue);      //calculate a new score based on this node and the neighbour (if its' out of bounds, use a big number)

                    if (score < gn.GetValueOrDefault(n, int.MaxValue)   //if that new score is less than the score of the neighbor (and the neighbor actually exists)
                        && _graph.ContainsKey(n))
                    {
                        processed[n] = cur;                             //mark the current node as processed from the neighbour
                        gn[n] = score;                                  //update the neighbours score with the new score
                        fn[n] = score +
                            cur.ManhattanDistance(target);              //update the neighbours estimate to the end as being that new score + Manhattan Distance
                        open.Enqueue(n, fn[n]);                         //Add the neighbour to the queue for it's own processing, with the estimate as the prioritization
                    }
                }
            }

            return Array.Empty<Node>();
        }

        private static Node[] ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            var res = new List<Node> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                res.Add(current);
            }
            res.Reverse();
            return res.ToArray();
        }
    }

    namespace Day15Extensions
    {
        internal static class Extensions
        {
            public static void Dump(this Dictionary<Node,int> map, int w, int h)
            {
                int maxY = h, maxX = w;

                for (int y = 0; y < maxY; y++)
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        Console.Write(map[(y,x)]);
                    }
                    Console.WriteLine("");
                }
            }
        }

        internal record Node(int X, int Y)
        {
            public IEnumerable<Node> Neighbours()
            {
                yield return new Node(X - 1, Y);
                yield return new Node(X + 1, Y);
                yield return new Node(X, Y - 1);
                yield return new Node(X, Y + 1);
            }

            public int ManhattanDistance(Node other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

            public static implicit operator Node((int x, int y) a) => new(a.x, a.y);
            //this is neat, allows you to pass an (x,y) tuple and it'll auto cast it to a point. handy for Dict[key]
        }
    }
}