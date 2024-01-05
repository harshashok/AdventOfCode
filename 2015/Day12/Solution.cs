using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode.Y2015.Day12;

[ProblemName("JSAbacusFramework.io")]      
class Solution : Solver {

    public object PartOne(string input) => (from m in Regex.Matches(input, @"-*\d+") select int.Parse(m.Value)).Sum();
    public object PartTwo(string input) => ParseJson(JsonDocument.Parse(input).RootElement);
    
    int ParseJson(JsonElement json)
    {
        if (json.ValueKind is JsonValueKind.Number) return json.GetInt32();
        if (json.ValueKind is JsonValueKind.Array) return json.EnumerateArray().Select(ParseJson).Sum();
        
        if (json.ValueKind is JsonValueKind.Object)
        {
            if (json.EnumerateObject().Any(x => x.Value.ToString() == "red")) return 0;
            return json.EnumerateObject().Select(x => ParseJson(x.Value)).Sum();
        }
        return 0;
    }
}
