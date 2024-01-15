using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day15;

[ProblemName("Science for Hungry People")]      
class Solution : Solver
{
    private List<Ingredient> ingredients = new();
    record Ingredient(string name, int capacity, int durabiity, int flavor, int texture, int calories);
    
    public object PartOne(string input) {
        ReadInput(input);
        return 0;
    }

    public object PartTwo(string input) {
        return 0;
    }

    void ComputeScore()
    {
        
    }

    void ReadInput(string input)
    {
        input.Split('\n')
            .ToList()
            .ForEach(line =>
            {
                var match = Regex.Match(line, @"(\w+): capacity (-*\d+), durability (-*\d+), flavor (-*\d+), texture (-*\d+), calories (-*\d+)");
                var arr = match.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray();
                ingredients.Add(new Ingredient(arr[0], int.Parse(arr[1]), int.Parse(arr[2]), int.Parse(arr[3]), int.Parse(arr[4]), int.Parse(arr[5])));
            });
    }
}
