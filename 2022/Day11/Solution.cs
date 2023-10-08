using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.CodeAnalysis.Differencing;
using AngleSharp.Dom;
using System.Drawing;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]      
class Solution : Solver {

    static Dictionary<int, Monkey> monkeyList;

    static bool logExec = false;

    public object PartOne(string input) {
        ReadInput(input);
        Rounds(20, x => x/3);
        return monkeyList.Select(x => x.Value)
            .OrderByDescending(x => x.inspectionCount)
            .Take(2)
            .Aggregate(1, (accumulated, x) => accumulated * x.inspectionCount);
    }

    public object PartTwo(string input) {
        ReadInput(input);
        long mod = monkeyList.Select(x => x.Value)
            .Aggregate(1, (accumulate, x) => accumulate * x.testCase);
        Rounds(10000, x => x % mod);

        return monkeyList.Select(x => x.Value)
            .OrderByDescending(x => x.inspectionCount)
            .Take(2)
            .Aggregate(1L, (accumulated, x) => accumulated * x.inspectionCount);
    }

    private void Rounds(int rounds, Func<long, long> worryMod)
    {
        for (int i = 1; i <= rounds; i++)
        {
            for (int j = 0; j < monkeyList.Count; j++)
            {
                var monkey = monkeyList.GetValueOrDefault(j);
                monkey.Inspection(worryMod);
            }
        }
    }

    private void ReadInput(string input)
    {
        monkeyList = new();
        input.Split("\n\n")
            .Select(b => b.Split("\n"))
            .ToList()
            .ForEach(x =>
            {
                var monkey = ParseMonkey(x);
                monkeyList.Add(monkey.monkeyNumber, monkey);
            });
    }

    private Monkey ParseMonkey(string[] monkeyStr)
    {
        //regex patterns
        string pattern_monkey_number = @"Monkey\s([0-9]):";
        string pattern_start_items   = @"Starting\sitems:\s*([0-9,\s]*)";
        string pattern_operation     = @"Operation:\s*new\s=\s(old|[0-9]*)\s([\*\+\-\/])\s([0-9]+|old)";
        string pattern_test          = @"Test:\sdivisible\sby\s([0-9]+)";
        string pattern_test_true     = @"If\strue:\sthrow\sto\smonkey\s([0-9])";
        string pattern_test_false    = @"If\sfalse:\sthrow\sto\smonkey\s([0-9])";

        //regex : matching
        var match_monkey_number = Regex.Match(monkeyStr[0], pattern_monkey_number);
        var match_start_items   = Regex.Match(monkeyStr[1], pattern_start_items);
        var match_operation     = Regex.Match(monkeyStr[2], pattern_operation);
        var match_test          = Regex.Match(monkeyStr[3], pattern_test);
        var match_test_true     = Regex.Match(monkeyStr[4], pattern_test_true);
        var match_test_false    = Regex.Match(monkeyStr[5], pattern_test_false);

        //regex : groups
        var result_monkey_number = ParseMatchResult(match_monkey_number);
        var result_start_items   = ParseMatchResult(match_start_items);
        var result_operation     = ParseMatchResult(match_operation);
        var result_test          = ParseMatchResult(match_test);
        var result_test_true     = ParseMatchResult(match_test_true);
        var result_test_false    = ParseMatchResult(match_test_false);

        //regex : values
        int value_monkey_number = int.Parse(result_monkey_number.First());
        var value_start_items = Array.ConvertAll(
            result_start_items.First()
                .Trim().Split(",").ToArray(), long.Parse);
        var value_operation = result_operation.ToArray();
        var value_test = result_test.First();
        var value_test_true = result_test_true.First();
        var value_test_false = result_test_false.First();

        Monkey monkey = new Monkey();
        monkey.monkeyNumber = value_monkey_number;
        monkey.items = new Queue<long>(value_start_items);
        monkey.operation = (value_operation[0], char.Parse(value_operation[1]), value_operation[2]);
        monkey.testCase = int.Parse(value_test);
        monkey.testCaseTrue = int.Parse(value_test_true);
        monkey.testCaseFalse = int.Parse(value_test_false);
        monkey.inspectionCount = 0;

        //todo : refactor to just using constructor.
        return monkey;
    }

    private IEnumerable<string> ParseMatchResult(Match m) =>
        m.Groups.Count > 1 ?
        m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) :
        new[] { m.Value };

    internal class Monkey
    {
        public int monkeyNumber;
        public Queue<long> items;
        public (string A, char op, string B) operation;
        public int testCase;
        public int testCaseTrue;
        public int testCaseFalse;
        public int inspectionCount;

        public Monkey() { }

        public Monkey(int monkeyNumber, Queue<long> items, (string A, char op, string B) operation, int testCase, int testCaseTrue, int testCaseFalse)
        {
            this.monkeyNumber = monkeyNumber;
            this.items = items;
            this.operation = operation;
            this.testCase = testCase;
            this.testCaseTrue = testCaseTrue;
            this.testCaseFalse = testCaseFalse;
            this.inspectionCount = 0;
        }

        public void Inspection(Func<long, long> worryModifier)
        {
            //  Monkey inspects an item with a worry level of 79.
            Log($"Monkey {this.monkeyNumber}");
            while (items.Any())
            {
                var itemq = items.Dequeue();
                inspectionCount++;
                Log($"\tMonkey inspects an item with a worry level of {itemq}.");

                itemq = PerformOperation(itemq);
                Log($"\t\tWorry level is multiplied by {operation} to {itemq}.");

                itemq = worryModifier(itemq);
                Log($"\t\tMonkey gets bored with item. Worry level is divided by 3 to {itemq}.");

                if (itemq % testCase == 0)
                {
                    ThrowToMonkey(testCaseTrue, itemq);
                    Log($"\t\tCurrent worry level is divisible by {testCase}.");
                    Log($"\t\tItem with worry level {itemq} is thrown to monkey {testCaseTrue}.");
                }
                else
                {
                    ThrowToMonkey(testCaseFalse, itemq);
                    Log($"\t\tCurrent worry level is not divisible by {testCase}.");
                    Log($"\t\tItem with worry level {itemq} is thrown to monkey {testCaseFalse}.");
                }
            }
        }

        private void ThrowToMonkey(int monkeyNumber, long item) =>
            monkeyList.GetValueOrDefault(monkeyNumber).items.Enqueue(item);

        private long PerformOperation(long item)
        {
            if (operation.A != "old") throw new InvalidOperationException($"{operation.A} is not a valid A arg");

            long A = item;
            long B = 0;

            if (int.TryParse(operation.B, out int result))
            {
                B = result;
            }
            else if (operation.B == "old")
            {
                B = item;
            }

            return operation.op switch
            {
                '+' => A + B,
                '-' => A - B,
                '*' => A * B,
                '/' => A / B,
                _ => throw new ArgumentOutOfRangeException(operation.op.ToString())
            };
        }
    }

    public static void Log(string log, bool forceLog = false)
    {
        if (logExec || forceLog) Console.WriteLine(log);
    }
}
