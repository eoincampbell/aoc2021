

    //Old Part 1

    //private object Part1()
    //{
    //    LoadInsertionRules();
    //    var polymer = PuzzleInput.FirstOrDefault()!;

    //    for (int step = 1; step <= 10; step++)
    //    {
    //        var newElements = new List<string>();
    //        for (int i = 0; i < polymer.Length - 1; i++)
    //            newElements.Add(_insertionRules[polymer[i..(i + 2)]]);

    //        var output = polymer
    //            .Select(x => x.ToString())
    //            .ZipWithDefault(newElements, (f, s) => new[] { f, s })
    //            .SelectMany(f => f);

    //        polymer = string.Concat(output);
    //    }

    //    var elements = polymer
    //        .GroupBy(g => g)
    //        .Select(y => new { y.Key, Count = y.Count() })
    //        .OrderByDescending(o => o.Count)
    //        .ToList();

    //    return elements[0].Count - elements[^1].Count;
    //}