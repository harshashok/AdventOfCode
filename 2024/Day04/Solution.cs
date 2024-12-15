using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day04;

[ProblemName("Ceres Search")]      
class Solution : Solver
{
    private char[][] gridMap;
    private int Xsize;
    private int Ysize;

    public object PartOne(string input) {
        ReadInput(input);
        int count = 0;
        for (int x = 0; x < Xsize; x++)
            for (int y = 0; y < Ysize; y++)
            {
                if (gridMap[x][y] == 'X')
                {
                    if (Top(x, y)) count++;
                    if (Down(x, y)) count++;
                    if (Right(x, y)) count++;
                    if (Left(x, y)) count++;
                    
                    if (TopRight(x, y)) count++;
                    if (TopLeft(x, y)) count++;
                    if (BottomLeft(x, y)) count++;
                    if (BottomRight(x, y)) count++;
                }
            }
        return count;
    }

    public object PartTwo(string input) {
        int count = 0;
        for (int x = 0; x < Xsize; x++)
            for (int y = 0; y < Ysize; y++)
            {
                if (gridMap[x][y] == 'A')
                {
                    if (XLeftDiagonal(x, y) && XRightDiagonal(x,y)) count++;
                }
            }
        return count;
    }

    private bool Right(int x, int y) =>
        (InBounds(x, y+1) && gridMap[x][y+1] == 'M') &&
        (InBounds(x, y+2) && gridMap[x][y+2] == 'A') &&
        (InBounds(x, y+3) && gridMap[x][y+3] == 'S');
    
    private bool Left(int x, int y) =>
        (InBounds(x, y-1) && gridMap[x][y-1] == 'M') &&
        (InBounds(x, y-2) && gridMap[x][y-2] == 'A') &&
        (InBounds(x, y-3) && gridMap[x][y-3] == 'S');
    
    private bool Top(int x, int y) =>
        (InBounds(x-1, y) && gridMap[x-1][y] == 'M') &&
        (InBounds(x-2, y) && gridMap[x-2][y] == 'A') &&
        (InBounds(x-3, y) && gridMap[x-3][y] == 'S');
    
    private bool Down(int x, int y) =>
        (InBounds(x+1, y) && gridMap[x+1][y] == 'M') &&
        (InBounds(x+2, y) && gridMap[x+2][y] == 'A') &&
        (InBounds(x+3, y) && gridMap[x+3][y] == 'S');
    
    private bool TopRight(int x, int y) =>
        (InBounds(x-1, y+1) && gridMap[x-1][y+1] == 'M') &&
        (InBounds(x-2, y+2) && gridMap[x-2][y+2] == 'A') &&
        (InBounds(x-3, y+3) && gridMap[x-3][y+3] == 'S');
    
    private bool TopLeft(int x, int y) =>
        (InBounds(x-1, y-1) && gridMap[x-1][y-1] == 'M') &&
        (InBounds(x-2, y-2) && gridMap[x-2][y-2] == 'A') &&
        (InBounds(x-3, y-3) && gridMap[x-3][y-3] == 'S');
    
    private bool BottomLeft(int x, int y) =>
        (InBounds(x+1, y-1) && gridMap[x+1][y-1] == 'M') &&
        (InBounds(x+2, y-2) && gridMap[x+2][y-2] == 'A') &&
        (InBounds(x+3, y-3) && gridMap[x+3][y-3] == 'S');
    
    private bool BottomRight(int x, int y) =>
        (InBounds(x+1, y+1) && gridMap[x+1][y+1] == 'M') &&
        (InBounds(x+2, y+2) && gridMap[x+2][y+2] == 'A') &&
        (InBounds(x+3, y+3) && gridMap[x+3][y+3] == 'S');

    private bool XLeftDiagonal(int x, int y) =>
        ((InBounds(x-1, y-1) && gridMap[x-1][y-1] == 'M') && (InBounds(x+1, y+1) && gridMap[x+1][y+1] == 'S')) ||
        ((InBounds(x-1, y-1) && gridMap[x-1][y-1] == 'S') && (InBounds(x+1, y+1) && gridMap[x+1][y+1] == 'M'));
    
    private bool XRightDiagonal(int x, int y) =>
        ((InBounds(x-1, y+1) && gridMap[x-1][y+1] == 'M') && (InBounds(x+1, y-1) && gridMap[x+1][y-1] == 'S')) ||
        ((InBounds(x-1, y+1) && gridMap[x-1][y+1] == 'S') && (InBounds(x+1, y-1) && gridMap[x+1][y-1] == 'M'));

    private bool InBounds(int x, int y) => (x >= 0 && x < Xsize) && (y >= 0 && y < Ysize);

    private void ReadInput(string input)
    {
        gridMap = input.Split('\n')
            .Select(x => x.ToArray())
            .ToArray();
        
        Xsize = gridMap.Length;
        Ysize = gridMap[0].Length;
    }
}
