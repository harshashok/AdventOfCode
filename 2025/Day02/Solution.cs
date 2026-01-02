using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Html;

namespace AdventOfCode.Y2025.Day02;

[ProblemName("Gift Shop")]      
class Solution : Solver
{
    private IEnumerable<(long start, long end)> IDRanges;

    public object PartOne(string input)
    {
        IDRanges = ParseInputFile(input);
        return IDRanges.AsParallel()
            .Sum(IdRangeInvalidSumHalves);
    }

    public object PartTwo(string input) =>
        IDRanges.AsParallel()
            .Sum(IsProdIdRangeInvalidV2);

    private long IsProdIdRangeInvalidV2((long start, long end) range) =>
        EnumerateRange(range)
            .Sum(IsProdIdInvalid);

    private long IsProdIdInvalid(long num)
    {
        int digitSum = DigitSum(num);
        int digitGrouping = digitSum / 2;
        
        while (digitGrouping > 0)
        {
            if (digitSum % digitGrouping != 0)
            {
                digitGrouping--;
                continue;
            }

            var set = num.ToString()
                .Chunk(digitGrouping)
                .Select(x => new string(x))
                .ToHashSet();

            if (set.Count == 1) 
                return num;

            digitGrouping--;
        }
        return 0;
    }
    
    private long IdRangeInvalidSumHalves((long start, long end) range) =>
        EnumerateRange(range)
            .Sum(num =>
            {
                if (!IsEvenDigitsNumber(num)) 
                    return 0;
                
                int digitSumHalf = DigitSum(num) / 2;
                (long first, long second) halves = ( num % (long)Math.Pow(10, digitSumHalf), num / (long)Math.Pow(10, digitSumHalf));
                return (halves.first == halves.second) ? num : 0;
            });

    private bool IsEvenDigitsNumber(long n) => DigitSum(n) % 2 == 0;
    private int DigitSum(long n) => (n != 0) ? (int)Math.Floor(Math.Log10(n)) + 1 : 1;
    
    IEnumerable<long> EnumerateRange((long start, long end) range)
    {
        for (long i = range.start; i <= range.end; i++)
        {
            yield return i;
        }
    }
    
    private IEnumerable<(long, long)> ParseInputFile(string input) =>
        input.Split(',')
            .Select(range => range.Split('-'))
            .Select(arr => (long.Parse(arr[0]), long.Parse(arr[1])));
}
