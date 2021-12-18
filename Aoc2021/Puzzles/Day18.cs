using Aoc2021.Puzzles.Day18Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day18 : Puzzle
    {
        public override int Day => 18;
        public override string Name => "Snailfish";
        protected override object RunPart1() => Part1();   //3935
        protected override object RunPart2() => Part2();   //4669

        public Day18()
            : base("Inputs/Day18.txt")
            //: base("Inputs/Day18Sample.txt")
        {
        }

        public object Part1()
        {
            var nodes = new List<Node>();
            foreach (var input in PuzzleInput) nodes.Add(ReadNode(input));

            var node = nodes[0];
            foreach (var n in nodes.ToArray()[1..]) node += n;

            return node.Magnitude;
        }

        public object Part2()
        {
            var inputs = PuzzleInput.ToList();
            decimal maxMag = 0;
            for(int i = 0; i < inputs.Count; i++)           //Add from 0..End for both loops
            {
                for (int j = 0; j < inputs.Count; j++)      //Since Addition is not Commutative
                {
                    if(i != j)
                    {
                        var a = ReadNode(inputs[i]);
                        var b = ReadNode(inputs[j]);
                        var mag = (a + b).Magnitude;
                        if (mag > maxMag) maxMag = mag;
                    }
                }
            }
            return maxMag;
        }

        private Node ReadNode(string input)
        {
            int ptr = 0;
            return ReadNode(input, ref ptr);
        }

        private Node ReadNode(string input, ref int ptr)
        {
            var node = new Node();
            if (input[ptr] == '[') ptr++;       //This is to handle the opening '[' of the node

            //next we expect either a nested left OR a left value. 
            if (input[ptr] == '[')
                node.Left = ReadNode(input, ref ptr);
            else if (input[ptr].IsDigit())
                node.Left = new Node() { Value = input[ptr].ToInt() };

            ptr+=2;  //Nudge along after left, and past comma

            if (input[ptr] == '[')    
                node.Right = ReadNode(input, ref ptr);
            else if (input[ptr].IsDigit())
                node.Right = new () { Value = input[ptr].ToInt() };

            ptr++; //Nudge past the closing ']'
            return node;
        }
    }

    namespace Day18Extensions
    {
        internal class DepthNode
        {
            public int Depth { get; set; }
            public Node? Node { get; set; }
            public override string ToString() => $"Depth: {Depth} | Node: {Node}";
        }

        internal class Node
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public decimal Value { get; set; }
            public bool IsLeaf => Left == null && Right == null;
            public decimal Magnitude => IsLeaf ? Value : (Left!.Magnitude * 3) + (Right!.Magnitude * 2);
            public override string ToString() => IsLeaf ? Value.ToString() : $"[{Left},{Right}]";

            public bool TrySplit()
            {
                if (Value < 10) return false;
                Left = new() { Value = Math.Floor(Value / 2) };
                Right = new() { Value = Math.Ceiling(Value / 2) };
                Value = 0;
                return true;
            }

            #region Static Overload for Addition + Subsequent processing mechanics

            public static Node operator +(Node l, Node r)
            {
                var n = new Node { Left = l, Right = r };
                var stop = false;
                while (!stop)                                   //while not done
                {
                    var exploded = ProcessExplodingNode(n);     //Try and process the first (left most) explodable node you come across
                    if (exploded) continue;                     //and loop around for more things to do

                    var split = false;
                    ProcessSplittingNode(n, ref split);         //otherwise try and split the first (left most) splitable node you come across
                    if(split) continue;                         //and loop around for more things to do

                    stop = true;                                //stop when we don't perform either action in an iteration
                }
                return n;
            }

            private static bool ProcessExplodingNode(Node n)
            {
                var list = new List<DepthNode>();               //Create a list to flatten the tree into, 
                FlattenTreeToList(n, ref list, 0);              //noting the depth of each node as it's flattened.

                var explodable = list                           //Find the first node that is "explodable"...
                    .Find(dn => dn.Depth >= 4 &&                //i.e. Depth 4 or more
                                !dn.Node!.IsLeaf);              //and it has child nodes (i.e. it isn;t itself just a LeafNode)

                if (explodable == null) return false;           //Otherwise return false

                var exId= list.IndexOf(explodable);             //Find the index of that node
                var ldnv = list[exId - 1].Node!.Value;          //That means it's singular node value representations will be one index prior 
                var rdnv = list[exId + 1].Node!.Value;          //And  1 index later

                for (int i = exId - 2; i >= 0; i--)             //for the left value, work backwards from 2 to the left (1 to the left of the Leaf Node Value) 
                    if (list[i].Node!.IsLeaf)                   //looking for another leaf nodes to combine with
                    {
                        list[i].Node!.Value += ldnv;
                        break;
                    }

                for (int i = exId + 2; i < list.Count; i++)     //for the right node. work forwards from 2 to the right (1 to the right of the Leaf Node Value)
                    if (list[i].Node!.IsLeaf)                   //looking for another leaf node to combine with
                    {
                        list[i].Node!.Value += rdnv;
                        break;
                    }

                explodable.Node!.Left = null;                   //When Done
                explodable.Node!.Right = null;                  //Nuke left & right nodes
                explodable.Node.Value = 0;                      //turn this into a zero value leaf Node
                return true;                                    //And return true since we found and exploded one node                                 
            }

            private static void FlattenTreeToList(Node n, ref List<DepthNode> list, int depth)
            {
                if (n == null) return;
                FlattenTreeToList(n.Left!, ref list, depth + 1);        //Recursively Flatten the Left Branch

                list.Add(new DepthNode() { Depth = depth, Node = n});   //Then Add self to Center

                FlattenTreeToList(n.Right!, ref list, depth + 1);       //Recursively Flatten the Right Branch
            }

            private static void ProcessSplittingNode(Node n, ref bool shouldReturn)
            {
                if (n == null || shouldReturn) return;              //don't process further, we've already done a split
                shouldReturn = n.TrySplit();                        //Try split this node if it needs splitting
                ProcessSplittingNode(n.Left!, ref shouldReturn);    //Recursively Process
                ProcessSplittingNode(n.Right!, ref shouldReturn);   //Left & Right
            }

            #endregion
        }

        internal static class Extensions
        {
            public static bool IsDigit(this char c) => c >= '0' && c <= '9';
            public static int ToInt(this char c) => int.Parse(c.ToString());
        }
    }
}