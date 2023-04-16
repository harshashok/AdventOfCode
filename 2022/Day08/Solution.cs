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

    enum Direction
    {
        up,
        down,
        left,
        right
    }
    public object PartOne(string input) {
        ParseForest(input);
        return CountVisibleTrees();
    }

    public object PartTwo(string input) {
        return ComputeScenicNumber().Max();
    }

    public void ParseForest(string input)
    {
        forestMap = input.Split('\n')
                      .Select(x => x.Select(y => (int)char.GetNumericValue(y)).ToArray())
                      .ToArray();

        XSize = forestMap.Length;
        YSize = forestMap[0].Length;
    }

    private int CountVisibleTrees()
    {
        int visible = 0;
        for (int x = 0; x < XSize; x++)
            for (int y = 0; y < YSize; y++)
            {
                visible = IsVisible(x, y) ? visible+1 : visible+0;
            }
        return visible;
    }

    private List<int> ComputeScenicNumber()
    {
        List<int> scenicNumbers = new();
        for (int x = 0; x < XSize; x++)
            for (int y = 0; y < YSize; y++)
            {
                scenicNumbers.Add(TestScenic(x, y));
            }
        return scenicNumbers;
    }

    private int TestScenic(int x, int y)
    {
        if (IsEdgeTree(x, y)) return 0;

        //View from top.
        var top = IsVisible_Top(x, y) ? Enumerable.Range(0, x).Where(i => (i != x)).Count() : 
            Enumerable.Range(0, x).Reverse()
            .TakeWhile(i => forestMap[i][y] < forestMap[x][y])
            .Count() + 1;

        var bottom = IsVisible_Bottom(x, y) ? Enumerable.Range(x, XSize - x).Where(i => (i != x)).Count() :
            Enumerable.Range(x+1, XSize)
            .TakeWhile(i => forestMap[i][y] < forestMap[x][y])
            .Count() + 1;

        var left = IsVisible_Left(x, y) ? Enumerable.Range(0, y).Where(i => (i != y)).Count() :
            Enumerable.Range(0, y).Reverse()
            .TakeWhile(i => forestMap[x][i] < forestMap[x][y])
            .Count() + 1;

        var right = IsVisible_Right(x, y) ? Enumerable.Range(y, YSize - y).Where(i => (i != y)).Count() :
            Enumerable.Range(y+1, YSize)
            .TakeWhile(i => forestMap[x][i] < forestMap[x][y])
            .Count() + 1;

        return top * bottom * left * right;
    }

    private bool IsVisible(int x, int y) => IsEdgeTree(x, y) || IsVisible_Top(x,y) || IsVisible_Bottom(x,y) || IsVisible_Left(x, y) || IsVisible_Right(x, y);

    private bool IsEdgeTree(int x, int y) => (x == 0 || x == XSize - 1 || y == 0 || y == YSize - 1);

    private bool IsVisible_Top(int x, int y) => (!(Enumerable.Range(0, x).Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y])).Any());
    private bool IsVisible_Bottom(int x, int y) => (!(Enumerable.Range(x, XSize - x).Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y])).Any());
    private bool IsVisible_Left(int x, int y) => (!(Enumerable.Range(0, y).Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y])).Any());
    private bool IsVisible_Right(int x, int y) => (!(Enumerable.Range(y, YSize - y).Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y])).Any());

    private IEnumerable<int> Visible_Top(int x, int y) =>  (Enumerable.Range(0, x).Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y]));
    private IEnumerable<int> Visible_Bottom(int x, int y) => (Enumerable.Range(x, XSize - x).Where(i => (i != x) && forestMap[i][y] >= forestMap[x][y]));
    private IEnumerable<int> Visible_Left(int x, int y) => (Enumerable.Range(0, y).Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y]));
    private IEnumerable<int> Visible_Right(int x, int y) => (Enumerable.Range(y, YSize - y).Where(i => (i != y) && forestMap[x][i] >= forestMap[x][y]));
}
