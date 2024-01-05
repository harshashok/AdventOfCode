using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Common;

namespace AdventOfCode.Y2015.Day11;

[ProblemName("Corporate Policy")]      
class Solution : Solver {

    public object PartOne(string input) => ValidPasswords(input).First();

    public object PartTwo(string input) => ValidPasswords(input).Skip(1).First();
    
    IEnumerable<string> ValidPasswords(string input)
    {
        return NextPassPhrase(input).Where(pass =>
        {
            var validChars = !"iol".Any(pass.Contains);
            if (!validChars) return false;
            
            var series3Chars = Enumerable.Range(0, pass.Length - 2).Any(i =>
                (pass[i] + 1 == pass[i + 1]) && (pass[i + 1] + 1 == pass[i + 2]));
            if (!series3Chars) return false;
            
            var pairs2 = Enumerable.Range(0, pass.Length - 1)
                .Select(i => pass.Substring(i, 2)).Where(x => x[0] == x[1])
                .Distinct().Count() > 1;
            if (!pairs2) return false;
            
            return true;
        });
    }

    IEnumerable<string> NextPassPhrase(string pass)
    {
        while (true)
        {
            StringBuilder sb = new();
            for (int i = pass.Length-1; i >= 0; i--)
            {
                var ch = pass[i] + 1;
                if (ch > 'z')
                {
                    ch = 'a';
                    sb.Insert(0, (char)ch);
                }
                else
                {
                    sb.Insert(0, (char)ch);
                    sb.Insert(0, pass.Substring(0, i));
                    break;
                }
            }
            pass = sb.ToString();
            yield return pass;
        }
    }
}
