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
    private Dictionary<string, int> cache = new();
    public object PartOne(string input) {
        ReadInput(input);
        var result = Compute("f");
        return result;
    }

    public object PartTwo(string input) {
        return 0;
    }

    /**
     *  InExpr -> outExpr
     *       x -> 123
     *       d -> x AND y
     *       f -> x LSHIFT 2
     */
    void ComputeExpr(string inExpr, string outExpr)
    {
        
    }

    ushort Compute(string expr)
    {
        if (ushort.TryParse(expr, out var resultInt))
        {
            return resultInt;
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
            return retVal;
        }

        var lshiftOpMatches = Regex.Match(expr, @"(\w+) LSHIFT (\w+)");
        if (lshiftOpMatches.Success)
        {
            var matchArr = lshiftOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) << int.Parse(matchArr[1].Value)));
            return retVal;
        }
        
        var rshiftOpMatches = Regex.Match(expr, @"(\w+) RSHIFT (\w+)");
        if (rshiftOpMatches.Success)
        {
            var matchArr = rshiftOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) >> int.Parse(matchArr[1].Value)));
            return retVal;
        }
        
        var andOpMatches = Regex.Match(expr, @"(\w+) AND (\w+)");
        if (andOpMatches.Success)
        {
            var matchArr = andOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) & Compute(matchArr[1].Value)));
            return retVal;
        }
        
        var orOpMatches = Regex.Match(expr, @"(\w+) OR (\w+)");
        if (orOpMatches.Success)
        {
            var matchArr = orOpMatches.Groups.Cast<Group>().Skip(1).ToArray();
            var retVal = unchecked((ushort)(Compute(matchArr[0].Value) | Compute(matchArr[1].Value)));
            return retVal;
        }
        
        return ushort.MaxValue;
    }
    
    void ReadInput(string input)
    {
        input.Split('\n')
            .Select(line => line.Split(" -> "))
            .ToList()
            .ForEach(values => WireMap.Add(values[1], values[0]));
    }
}
