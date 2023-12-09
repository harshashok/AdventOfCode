using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]      
class Solution : Solver {
    Dictionary<Point, string> map = new();
    List<Number> numMap = new();
    record Number (string value, Point point);
    Regex number_regex = new(@"\d+");
    Regex symbol_regex = new(@"[^.\d]");

    public object PartOne(string input) {
        ReadInput(input);
        var ret = numMap.Select(num => IsPartNumber(num) ? int.Parse(num.value) : 0);
        return ret.Sum();
        //return IsPartNumber("617");

    }

    public object PartTwo(string input) {
        return 0;
    }

    /**
     *    .....
     *    .234.
     *    .....
     */
    bool IsPartNumber(Number part)
    {
        int numLength = part.value.Length;
        Point start = part.point - new Size(0,1);
        Point end = part.point + new Size(0, numLength);

        if (IsSymbol(start) || IsSymbol(end)) return true;

        //check top rect line
        for (var top = start + new Size(-1,0); top != end + new Size(-1,1); top += new Size(0, 1))
        {
            if (IsSymbol(top)) return true;
        }

        //check bottom rect line
        for (var bottom = start + new Size(1, 0); bottom != end + new Size(1, 1); bottom += new Size(0, 1))
        {
            if (IsSymbol(bottom)) return true;
        }  
        return false;
    }

    bool IsSymbol(Point p)
    {
        return map.TryGetValue(p, out string val) ?
               (int.TryParse(val, out _) ? false : true) :
               false;
    }
    void ReadInput(string input)
    {
        int index = 0;
        input.Split('\n').ToList()
            .ForEach((line => ParseLine(line, index++)));
    }

    void ParseLine(string line, int lineNumber)
    {
        var number_result = number_regex.Matches(line);
        var symbol_result = symbol_regex.Matches(line);

        number_result.ToList()
            .ForEach(m =>
            {
                Point startpoint = new Point(lineNumber, m.Index);
                Point endPoint = startpoint + new Size(0, m.Length);
                numMap.Add(new Number(m.Value, startpoint));
                for (var start = startpoint; start != endPoint; start+=new Size(0, 1))
                {
                    map.Add(start, m.Value);
                }
            });

        symbol_result.ToList().ForEach(m => map.Add(new Point(lineNumber, m.Index), m.Value));
    }
}
