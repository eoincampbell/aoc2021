﻿namespace Aoc2021.Puzzles
{
    internal class Day01 : Puzzle
    {
        public Day01() : 
            base("Inputs/Day01.txt") 
        {
            arr = PuzzleInput.Select(int.Parse).ToArray();
        }

        private int[] arr;

        private object CompareDepths(int lookAhead)
        {
            var count = 0;
            for (int i = 0; i < arr.Length - lookAhead; i++)
                if (arr[i] < arr[i + lookAhead])
                    count++;

            return count;
        }

        protected override string Title => "################ Day 01 ####################";
        protected override object RunPart1() => CompareDepths(1);   //1564
        protected override object RunPart2() => CompareDepths(3);   //1611
    }
}