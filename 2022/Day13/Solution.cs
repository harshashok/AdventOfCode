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
    static bool DEBUG = false;

    public object PartOne(string input) {
        ReadInput(input);
        //int element = 1;
        //var pair = pairs.ElementAt(element);
        //Console.WriteLine($"== Pair {element + 1} ==");
        //return Evaluate(pair.left, pair.right);


        //var rrr = pairs.Select((pair, index) =>
        //{
        //    //Console.WriteLine($"== Pair {index + 1} ==");
        //    var retVal = Evaluate(pair.left, pair.right);
        //    //Console.WriteLine();
        //    return retVal == Result.CorrectOrder ? index + 1 : 0;
        //});

        //return rrr.Sum();

        return pairs.Select((pairs, index) => Evaluate(pairs.left, pairs.right) == Result.CorrectOrder ? index + 1 : 0).Sum();

        //return pairs.Select((pair, index) => Compare(pair.left, pair.right) < 0 ? index + 1 : 0).Sum();
    }

    public object PartTwo(string input) {
        return 0;
    }


    private Result Evaluate(JsonNode left, JsonNode right, int indent = 0)
    {
        Log(left.ToJsonString(), right.ToJsonString(), indent+1);

        if(left is JsonValue leftVal && right is JsonValue)
        {
            //if ((int)left == (int)right) return Result.Continue;
            if ((int)left < (int)right) { if(DEBUG) Console.WriteLine("Left side is smaller, so inputs are in the right order"); return Result.CorrectOrder; }
            if ((int)left > (int)right) { if(DEBUG) Console.WriteLine("Right side is smaller, so inputs are not in the right order"); return Result.WrongOrder; }
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

        if (arrayA.Count < arrayB.Count) { if(DEBUG) Console.WriteLine("Left side ran out of items, so inputs are in the right order"); }
        if (arrayA.Count > arrayB.Count) { if(DEBUG) Console.WriteLine("Right side ran out of items, so inputs are not in the right order"); }
        return arrayA.Count < arrayB.Count ? Result.CorrectOrder :
            (arrayA.Count == arrayB.Count) ? Result.Continue : Result.WrongOrder;

    }

    //to delete.
    int Compare(JsonNode left, JsonNode right)
    {
        if (left is JsonValue leftVal && right is JsonValue rightVal)
        {
            return left.GetValue<int>() - right.GetValue<int>();
        }

        var leftArray = left switch { JsonArray a => a, _ => new JsonArray(left.GetValue<int>()) };
        var rightArray = right switch { JsonArray a => a, _ => new JsonArray(right.GetValue<int>()) };
        foreach (var (leftItem, rightItem) in Enumerable.Zip(leftArray, rightArray))
        {
            var c = Compare(leftItem, rightItem);
            if (c != 0)
            {
                return c;
            }
        }
        return leftArray.Count - rightArray.Count;
    }

    //to delete
    private Result tempCompare(JsonArray arrayA, JsonArray arrayB)
    {
        if (arrayA.Count <= arrayB.Count) Console.WriteLine("Left side ran out of items, so inputs are in the right order");
        if (arrayA.Count > arrayB.Count) Console.WriteLine("Right side ran out of items, so inputs are not in the right order");
        return arrayA.Count <= arrayB.Count ? Result.CorrectOrder : Result.WrongOrder;
    }

    enum Result
    {
        Continue,
        CorrectOrder,
        WrongOrder,
    }

    //to delete
    private static bool IsListForm(string str) => str.StartsWith('[') && str.EndsWith(']');

    //to delete
    private static void Log(string left, string right, int indent)
    {
        if (!DEBUG) return;
        string tab = new string(' ', indent);
        Console.WriteLine($"{tab}- Compare {left} vs {right}");
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
