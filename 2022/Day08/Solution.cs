using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;

namespace AdventOfCode.Y2022.Day08;

[ProblemName("Treetop Tree House")]      
class Solution : Solver {

    int[][] forestMap;
    int XSize;
    int YSize;

    public object PartOne(string input) {
        ParseForest(input);
        //Test(2, 2);
        return CountVisibleTrees();
    }

    public object PartTwo(string input) {
        return 0;
    }

    public void ParseForest(string input)
    {
        forestMap = input.Split('\n').ToList()
                                 .Select(x => x.Select(y => (int)char.GetNumericValue(y)).ToArray())
                                 .ToArray();
        XSize = forestMap.Length;
        YSize = forestMap[0].Length;
        //Console.WriteLine($"XSize : {XSize} | YSize: {YSize}");
    }

    private int CountVisibleTrees()
    {
        int visible = 0;
        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                visible = IsVisible(x, y) ? visible+1 : visible+0;
            }
        }
        return visible;
    }

    private bool IsVisible(int x, int y)
    {
        if (IsEdgeTree(x, y)) return true;

        return IsVisible_LeftRight(x, y) || IsVisible_TopBottom(x, y);
    }

    private bool IsEdgeTree(int x, int y)
    {
        return (x == 0 || x == XSize - 1 || y == 0 || y == YSize - 1);
    }

    private bool IsVisible_TopBottom(int x, int y)
    {
        var retTop =  !Enumerable.Range(0, x)
                    .Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y])
                    .Any();

        var retBottom =  !Enumerable.Range(x, YSize-x)
            .Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y])
            .Any();

        return retTop || retBottom;
    }

    private bool IsVisible_LeftRight(int x, int y)
    {
        var retLeft = !Enumerable.Range(0, y)
                .Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y])
                .Any();

        var retRight =  !Enumerable.Range(y, XSize-y)
        .Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y])
        .Any();

        return retLeft || retRight;
    }

    private void Test(int x, int y)
    {
        var rr = Enumerable.Range(y, XSize-y)
        .Where(i => (i != y));



        rr.ToList().ForEach(i => Console.Write("{0}\t", i));
        Console.WriteLine();
    }
}
