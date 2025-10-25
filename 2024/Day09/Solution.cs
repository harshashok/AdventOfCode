using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day09;

[ProblemName("Disk Fragmenter")]      
class Solution : Solver
{
    private IEnumerable<int> diskMap;
    
    public object PartOne(string input)
    {
        diskMap = ReadInput(input);
        return DefragDisk(ExpandedDiskMap())
            .Select((s, i) => (ulong)(s * i))
            .Aggregate((a, b) => a + b);
    }

    public object PartTwo(string input) {
        return ReadInput(input).Sum();
    }

    private void DefragFull(int[] blocks)
    {
        int front = 0, back = blocks.Length - 1;
        int num = 0, numLen = 0, emptyLen = 0;
        while (back > 0)
        {
            numLen = 0;
            while (blocks[back] < 0) back--;

            if (blocks[back] >= 0) num = blocks[back];
            
            while (back > 0 && blocks[back--] == num)
            {
                numLen++;
            }

            if (numLen > 0)
            {
                while (front < back)
                {
                    while (blocks[front] >= 0) front++;
                    while (blocks[front] < 0) emptyLen++;
                }
            }
        }
    }

    private IEnumerable<int> DefragDisk(int[] blocks)
    {
        int front = 0, back = blocks.Length - 1;

        while (front <= back)
        {
            while (blocks[front] >= 0) front++;
            while (blocks[back] < 0) back--;
            if (front >= back) break;
            
            (blocks[front], blocks[back]) = (blocks[back], blocks[front]);
        }
        // Console.WriteLine("{0}", string.Join("", blocks));
        // Console.WriteLine($"Front : {front} | Back : {back}");
        var disk = blocks.Take(front);
        Array.Resize(ref blocks, front);
        
        // blocks.ToList().ForEach(s =>
        // {
        //     if (s >= 0) Console.Write(s);
        //     else Console.Write($".");
        // });
        // Console.WriteLine();
        return disk;

    }

    private int[] ExpandedDiskMap()
    {
        List<int> blocks = new();
        
        int index = 0;
        foreach (var arr in diskMap.Chunk(2))
        {
            blocks.AddRange(Enumerable.Repeat(index, arr[0]));
            blocks.AddRange(Enumerable.Repeat(-1, arr[1]));
            index++;
        }

        //DEBUG
        // blocks.ForEach(s =>
        // {
        //     if (s >= 0) Console.Write(s);
        //     else Console.Write($".");
        // });
        // Console.WriteLine();
        //END DEBUG

        return blocks.ToArray();
    }

    private IEnumerable<int> ReadInput(string input) => input.Select(ch => ch - '0').Append(0);
}
