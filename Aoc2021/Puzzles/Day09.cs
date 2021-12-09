using Aoc2021;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace Aoc2021.Puzzles
{
    internal class Day09 : Puzzle
    {
        public Day09()
        : base("Inputs/Day09.txt")
        //: base("Inputs/Day09Sample.txt")
        {
            _basins = new Dictionary<(int w, int h), Dictionary<(int w, int h), bool>>();
        }

        private int[,] _map;

        private int MapWidth => _map.GetUpperBound(0);

        private int MapHeight => _map.GetUpperBound(1);

        private Dictionary<(int w, int h), Dictionary<(int w, int h), bool>> _basins;   
        //key'd by bottom of basin, value is all cells in the basin and whether they've been processed or not.

        private object Part2()
        {
            foreach(var b in _basins)
            {
                var (x, y) = b.Key; //Get the bottom of a basin from part1

                
                _basins[b.Key].Add((x,y), false); //Add itself to the basin

                while(_basins[b.Key].Any(x => !x.Value))    //Keep processing this basin for more adjacent cells if the cell is not processed
                {
                    var (cx,cy) = _basins[b.Key].First(x => !x.Value).Key;

                    var linkedCells = Part2ProcessCell(cx, cy);  //Find all adjacent non-height-9 cells

                    _basins[b.Key][(cx, cy)] = true;        //consider the current cell processed

                    foreach (var (lcx, lcy) in linkedCells) //recursively add any found adjacent cells that aren't already added
                        if (!_basins[b.Key].ContainsKey((lcx, lcy)))
                            _basins[b.Key].Add((lcx, lcy), false);
                }
            }

            //order basins by size, grab top 3, and multiply
            var result = _basins.Values.Select(x => x.Count()).OrderByDescending(y => y).Take(3).Product();

            return result;
        }

        private IEnumerable<(int x, int y)> Part2ProcessCell(int x, int y)
        {
            if (y > 0 && _map[x, y] < _map[x, y - 1] && _map[x, y - 1] != 9)
                yield return (x, y - 1);

            if(y < MapHeight && _map[x, y] < _map[x, y + 1] && _map[x, y + 1] != 9)
                yield return (x, y + 1);

            if (x > 0 && _map[x, y] < _map[x - 1, y] && _map[x-1, y] != 9)
                yield return (x - 1, y);

            if (x < MapWidth && _map[x, y] < _map[x + 1, y] && _map[x+1, y] != 9)
                yield return (x + 1, y);
        }

        private object Part1()
        {
            var input = PuzzleInput.Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();
            _map = input.To2DArray();

            int count = 0;
            for (int x = 0; x <= MapWidth; x++)
               for (int y = 0; y <= MapWidth; y++)
               {
                    var c = ProcessCell(x, y);
                    if(c > 0)
                    {
                        count += c;
                        _basins.Add((x, y), new Dictionary<(int x, int y), bool>());
                    }
               }
            return count;
        }

        private int ProcessCell(int w, int h)
        {
            var lowerThanNorth = (h > 0 && _map[w, h] < _map[w, h - 1]) || h == 0;
            var lowerThanSouth = (h < MapHeight && _map[w, h] < _map[w, h + 1]) || h == MapHeight;
            var lowerThanWest = (w > 0 && _map[w, h] < _map[w - 1, h]) || w == 0;
            var lowerThanEast = (w < MapWidth && _map[w, h] < _map[w + 1, h]) || w == MapWidth;

            if(lowerThanNorth && lowerThanSouth && lowerThanEast && lowerThanWest)
                return _map[w, h] + 1;
            
            return -1;
        }

        protected override string Title => "################ Day 09 ####################";
        protected override object RunPart1() => Part1(); //532
        protected override object RunPart2() => Part2(); //1110780

    }
}