using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day01;

[ProblemName("Historian Hysteria")]      
class Solution : Solver
{
    private List<int> listOne = new();
    private List<int> listTwo = new();

    public object PartOne(string input)
    {
        ReadInput(input);
        return listOne.Zip(listTwo).Sum(x => Math.Abs(x.First - x.Second));
    }

    public object PartTwo(string input)
    {
        var map = listTwo.GroupBy(g => g).ToDictionary(x => x.Key, x => x.Count());
        return listOne.Select(x => x * map.GetValueOrDefault(x, 0)).Sum(); 
    }

    public void ReadInput(string input)
    {
        input.Split('\n').ToList()
            .ForEach(l =>
            {
                var items = l.Split("   ", StringSplitOptions.None);
                listOne.Add(int.Parse(items[0]));
                listTwo.Add(int.Parse(items[1]));
            });
        listOne.Sort();
        listTwo.Sort();
    }
}
