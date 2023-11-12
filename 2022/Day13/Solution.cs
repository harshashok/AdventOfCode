using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json.Nodes;
using AngleSharp.Dom;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]      
class Solution : Solver {
    List<Pairs> pairs = new();

    public object PartOne(string input) {
        ReadInput(input);
        return pairs.Select((pairs, index) =>
            Evaluate(pairs.left, pairs.right) == Result.CorrectOrder ? index + 1 : 0).Sum();
    }

    public object PartTwo(string input) {
        var divider2 = JsonNode.Parse("[[2]]");
        var divider6 = JsonNode.Parse("[[6]]");
        var list = ReadInputPart2(input)
            .Append(divider2)
            .Append(divider6)
            .ToList();

        list.Sort(CompareTo);

        return (list.IndexOf(divider2)+1) * (list.IndexOf(divider6)+1);
    }

    private int CompareTo(JsonNode left, JsonNode right)
    {
        return Evaluate(left, right) switch
        {
            Result.CorrectOrder => -1,
            Result.WrongOrder => 1,
            _ => 0
        };
    }

    private Result Evaluate(JsonNode left, JsonNode right, int indent = 0)
    {
        if(left is JsonValue leftVal && right is JsonValue)
        {
            if ((int)left < (int)right) { return Result.CorrectOrder; }
            if ((int)left > (int)right) { return Result.WrongOrder; }
            return Result.Continue;
        }

        var arrayA = left as JsonArray ?? new JsonArray((int)left);
        var arrayB = right as JsonArray ?? new JsonArray((int)right);
        var rest = Enumerable.Zip(arrayA, arrayB);
        var result = new List<Result>();
        foreach (var item in rest)
        {
            var ret = Evaluate(item.First, item.Second, indent + 1);
            if (ret != Result.Continue)
            {
                return ret;
            }
        }

        return arrayA.Count < arrayB.Count ? Result.CorrectOrder :
            (arrayA.Count == arrayB.Count) ? Result.Continue : Result.WrongOrder;

    }

    enum Result
    {
        Continue,
        CorrectOrder,
        WrongOrder,
    }

    private void ReadInput(string input)
    {
        pairs = input.Split("\n\n")
            .Select(x =>
            {
                var arr = x.Split('\n');
                return new Pairs(arr[0], arr[1]);
            })
            .ToList();
    }

    private IEnumerable<JsonNode> ReadInputPart2(string input)
    {
        return input.Split("\n")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(line => JsonNode.Parse(line));
    }

    internal class Pairs
    {
        public JsonNode left;
        public JsonNode right;

        public Pairs(string left, string right)
        {
            this.left = JsonNode.Parse(left);
            this.right = JsonNode.Parse(right);
        }
    }
    //Perl PCRE recursive Regex that parses this : "[^\\[\\],]*(\\[(?:[^\\[\\]]|(?1))*\\])[^\\[\\],]*(*SKIP)(*F)|,"
}
