using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day19;

[ProblemName("Medicine for Rudolph")]      
class Solution : Solver
{
    private List<Map> transformations = new();
    private string molecule;
    record class Map(string key, string value); 
        
    public object PartOne(string input)
    {
        ReadInput(input);
        return ApplyTransformation().Count();
    }

    public object PartTwo(string input)
    {
        //return ApplyReduction();
        return 0;
    }

    //y = X[:k] + j + X[k+len(i):]
    HashSet<string> ApplyTransformation()
    {
        HashSet<string> result = new();
        foreach (var trfm in transformations)
        {
            int tLength = trfm.key.Length;
            for (int i = 0; i < molecule.Length; i++)
            {
                if ((i + tLength <= molecule.Length) && (molecule.Substring(i, tLength) == trfm.key))
                {
                    string res = molecule.Substring(0, i) + trfm.value + molecule.Substring(i + tLength);
                    result.Add(res);
                }
            }
        }
        return result;
    }

    int ApplyReduction()
    {
        int count = 0;
        while (molecule != "e")
        {
            foreach (var trfm in transformations)
            {
                if (molecule.Contains(trfm.value))
                {
                    molecule = Replace(molecule, trfm.value, trfm.key, molecule.LastIndexOf(trfm.value, StringComparison.Ordinal));
                    count++;
                }
            }
            if (count > 2000) break; 
        }

        return count;
    }

    string Replace(string str, string src, string dest, int index)
    {
        return str.Substring(0, index) + dest + str.Substring(index + src.Length);
    }

    void ReadInput(string input)
    {
        var blocks = input.Split("\n\n");
        blocks[0].Split('\n').ToList()
            .ForEach(line =>
            {
                var arr = line.Split(" => ");
                transformations.Add(new Map(arr[0], arr[1]));
            });
        molecule = blocks[1];
    }
    
    // var distinct = input.Split('\n').ToHashSet();
    // var all = input.Split('\n');
    // var ret = all.GroupBy(x => x).SelectMany(g => g.Skip(1)).ToList();
    // return ret.Count;
}
