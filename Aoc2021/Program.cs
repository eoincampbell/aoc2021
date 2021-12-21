using Aoc2021;
using Aoc2021.Puzzles;

var days = new List<Puzzle>
{
    new Day01(),
    new Day02(),
    new Day03(),
    new Day04(),
    new Day05(),
    new Day06(),
    new Day07(),
    new Day08(),
    new Day09(),
    new Day10(),
    new Day11(),
    new Day12(),
    new Day13(),
    new Day14(),
    new Day15(),
    new Day16(),
    new Day17(),
    new Day18(),
    new Day19(),
    new Day20(),
    new Day21(),
};

bool currentDayOnly = true;

Puzzle.Header.Print();
foreach (var day in currentDayOnly ? days.OrderByDescending(x => x.Day).Take(1): days )
{
    day.Run().Print();
}