﻿using Aoc2021.Puzzles.Day16Extensions;
using Aoc2021.Puzzles.Day17Extensions;

namespace Aoc2021.Puzzles
{
    internal class Day17 : Puzzle
    {
        public override int Day => 17;
        protected override object RunPart1() => Part1();   //17766
        protected override object RunPart2() => Part2();   //1733

        private readonly Point _topLeft;
        private readonly Point _bottomRight;

        public Day17()
            : base("Inputs/Day17.txt")
        {
            //Sample Data target area: x = 20..30, y = -10..-5
            _topLeft = new Point(20, -5);
            _bottomRight = new Point(30, -10);

            //Real Data target area: x=48..70, y=-189..-148
            _topLeft = new Point(48, -148);
            _bottomRight = new Point(70, -189);
        }

        /****************************
            if the bottom of the box was at -6.
            send an upward trajectory with a vY of 5 
            dvY => +5,+4,+3,+2,+1,-0,-1,-2,-3,-4,-5,-6 
            5..-5 cancel out
            so you can just consider the negative coordinate of the bottom of the box. 
            max height will be N*(N-1)/2 where N = Abs(Bottom Of Box)
            //See /Notes/Day17.jpg
        */
        public object Part1() => Math.Abs(_bottomRight.Y)* (Math.Abs(_bottomRight.Y)-1) / 2;

        public object Part2()
        {
            var box = new Box(_topLeft, _bottomRight);
            var valid = new HashSet<Velocity>();

            for(int x = 0; x <= _bottomRight.X; x++)
            //Only need an X Value up to bottomRight.X or you overshoot it in 1 step
            {
                for (int y = _bottomRight.Y; y <= _bottomRight.Y * -1; y++)
                //Only need a Y Value between the +/- of Abs(bottomRight.Y) or you overshoot it.
                    //Positive Value, goes up, then returns to 0, and then overshoots in the next step
                    //Negative Value, immediately overshoots on Step 1
                {
                    var v = new Velocity(x, y);

                    Point currentPos;
                    do
                    {
                        currentPos = v.PerformStep();
                        if (box.IsInBox(currentPos))
                        {
                            valid.Add(v);
                            break;
                        }
                    }
                    while (!box.IsBelowBox(currentPos));
                }
            }
            return valid.Count;
        }
    }

    namespace Day17Extensions
    {
        public record Point(int X, int Y);

        public class Velocity
        {
            public int InitVelX { get; }
            public int InitVelY { get; }
            public int VelX { get; private set; }
            public int VelY { get; private set; }
            public int Step { get; private set; }
            private Point _current;

            public Velocity(int vX, int vY)
            {
                InitVelX = VelX = vX;
                InitVelY = VelY = vY;
                _current = new Point(0, 0);
                Step = 0;
            }

            public Point PerformStep()
            {
                _current = new Point(_current.X + VelX, _current.Y + VelY);
                VelX = VelX > 0 ? VelX - 1 : 0;
                VelY--;
                Step++;
                return _current;
            }

            public override string ToString() => $"{InitVelX},{InitVelY}";
        }

        public record Box (Point TopLeft, Point BottomRight)
        {
            public bool IsInBox(Point p) =>
                    TopLeft.X <= p.X &&
                    p.X <= BottomRight.X &&
                    TopLeft.Y >= p.Y &&
                    p.Y >= BottomRight.Y;

            public bool IsBelowBox(Point p) => p.Y < BottomRight.Y;
        }
    }
}