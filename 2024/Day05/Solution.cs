using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;

namespace AdventOfCode.Y2024.Day05;

[ProblemName("Print Queue")]      
class Solution : Solver
{
    private Dictionary<int, HashSet<int>> beforeAfterOrder = new();
    private IEnumerable<int[]> pagesList;

    public object PartOne(string input)
    {
        ReadInput(input);
        return pagesList.Where(InOrder).Select(arr => arr[arr.Length / 2]).Sum();
    }

    public object PartTwo(string input) =>
        pagesList.Where(pages => !InOrder(pages))
            .Select(page => page.OrderBy(num => num, ComparePrecedence).ToArray())
            .Select(arr => arr[arr.Length / 2]).Sum();

    private bool InOrder(int[] pages) =>
        Enumerable.Range(0, pages.Length)
            .Select(i =>
            {
                var before = pages.Take(i).ToHashSet();
                var afterSet = beforeAfterOrder.TryGetValue(pages[i], out var set) ? set : new();
                return !(before.Intersect(afterSet).Any());
            }).All(x => x);
    
    private Comparer<int> ComparePrecedence => Comparer<int>.Create((x, y) => (beforeAfterOrder.TryGetValue(x, out var set) ? set : new()).Contains(y) ? -1 : 1);

    private void ReadInput(string input)
    {
        var arr = input.Split("\n\n");
        arr[0].Split('\n').ToList().ForEach(line =>
        {
            var nums = line.Split('|');
            AddValueToSet(int.Parse(nums[0]), int.Parse(nums[1]));
        });
        pagesList = arr[1].Split('\n').Select(line => line.Split(','))
            .Select(a => Array.ConvertAll(a, int.Parse));
    }

    private void AddValueToSet(int key, int value)
    {
        HashSet<int> set = beforeAfterOrder.TryGetValue(key, out var s) ? s : new();
        set.Add(value);
        beforeAfterOrder[key] = set;
    }
}
