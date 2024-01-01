using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day07;

[ProblemName("Some Assembly Required")]      
class Solution : Solver
{
    private Dictionary<string, string> WireMap = new();
    private Dictionary<string, string> InverseWireMap = new();
    private Dictionary<string, ushort> cache = new();
    public object PartOne(string input) {
        ReadInput(input);
        return Compute("a");
    }

    public object PartTwo(string input)
    {
        var b = Compute("a");
        InverseWireMap.Remove(WireMap["b"]);
        WireMap.Remove("b");
        WireMap.Add("b", b.ToString());
        InverseWireMap.Add(b.ToString(), "b");
        cache = new();
        return Compute("a");
    }
    
    ushort Compute(string expr)
    {
        if (ushort.TryParse(expr, out var resultInt))
        {
            return resultInt;
        }

        if (cache.ContainsKey(expr))
        {
            return cache[expr];
        }
        
        if (WireMap.TryGetValue(expr, out string resultStr))
        {
            return Compute(resultStr);
        }
        
        var notOpMatches = Regex.Match(expr, @"NOT (\w+)");
        if (notOpMatches.Success)
        {
            var matchArr = notOpMatches.Groups.Cast<Group>().Skip(1).First();
            var retVal = unchecked((ushort) ~Compute(matchArr.Value));
            var key = InverseWireMap[expr];
            cache.Add(key, retVal);
            return retVal;
        }

        var lshiftOpMatches = Regex.Match(expr, @"(\w+) LSHIFT (\w+)");
        if (lshiftOpMatches.Success)
        {
            var matchArr = lshiftOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) << int.Parse(matchArr[1].Value)));
            var key = InverseWireMap[expr];
            cache.Add(key, retVal);
            return retVal;
        }
        
        var rshiftOpMatches = Regex.Match(expr, @"(\w+) RSHIFT (\w+)");
        if (rshiftOpMatches.Success)
        {
            var matchArr = rshiftOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) >> int.Parse(matchArr[1].Value)));
            var key = InverseWireMap[expr];
            cache.Add(key, retVal);
            return retVal;
        }
        
        var andOpMatches = Regex.Match(expr, @"(\w+) AND (\w+)");
        if (andOpMatches.Success)
        {
            var matchArr = andOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) & Compute(matchArr[1].Value)));
            var key = InverseWireMap[expr];
            cache.Add(key, retVal);
            return retVal;
        }
        
        var orOpMatches = Regex.Match(expr, @"(\w+) OR (\w+)");
        if (orOpMatches.Success)
        {
            var matchArr = orOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) | Compute(matchArr[1].Value)));
            var key = InverseWireMap[expr];
            cache.Add(key, retVal);
            return retVal;
        }
        
        return ushort.MaxValue;
    }
    
    void ReadInput(string input)
    {
        input.Split('\n')
            .Select(line => line.Split(" -> "))
            .ToList()
            .ForEach(values =>
            {
                WireMap.Add(values[1], values[0]);
                InverseWireMap.Add(values[0], values[1]);
            });
    }
}
