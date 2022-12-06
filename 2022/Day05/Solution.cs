using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;

namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]      
class Solution : Solver {

    Dictionary<int, Stack<string>> stackList;
    List<Move> moves;

    public object PartOne(string input) {
        ParseInputToStack(input);
        
        moves.ForEach(m =>
              {
                  var sourceStack = stackList.GetValueOrDefault(m.source);
                  var destStack = stackList.GetValueOrDefault(m.destination);
                  int ctr = m.qty;
                  while (ctr > 0)
                  {
                      destStack.Push(sourceStack.Pop());
                      ctr--;
                  };
              }
        );

        return string.Join("", stackList.Select(x => x.Value.Pop()))
                   .Replace("[", string.Empty)
                   .Replace("]", string.Empty);
    }

    public object PartTwo(string input) {
        ParseInputToStack(input);

        moves.ForEach(m =>
        {
            var sourceStack = stackList.GetValueOrDefault(m.source);
            var destStack = stackList.GetValueOrDefault(m.destination);
            int ctr = m.qty;
            Stack<string> crane = new();
            while (ctr > 0)
            {
                crane.Push(sourceStack.Pop());
                ctr--;
            }
            while(crane.TryPeek(out _))
            {
                destStack.Push(crane.Pop());
            }
        });

        return string.Join("", stackList.Select(x => x.Value.Pop()))
                   .Replace("[", string.Empty)
                   .Replace("]", string.Empty);
    }

    private void ParseInputToStack(string input)
    {
        var strArray = input.Split("\n\n");
        var craneMoves = strArray[1].Split("\n");
        var crateInput = strArray[0].Split("\n");
        var stackSize = Regex.Matches(crateInput[^1], @"([0-9])")
                        .Select(m => m.Value).Count();

        string pattern_stack = @"([\s]{4})|(\[[A-Z]\])";
        string pattern_moves = @"move\s([0-9]+)\sfrom\s([0-9]+)\sto\s([0-9]+)";
        Regex regex_stack = new(pattern_stack);
        Regex regex_moves = new(pattern_moves);

        stackList = new();
        moves = new();
        Enumerable.Range(1, stackSize)
            .ToList()
            .ForEach(i => stackList.Add(i, new()));

        int ctr = 1;
        for(int i = crateInput.Length-2; i>=0; i--)
        {
            Regex.Matches(crateInput[i], pattern_stack)
                .Select(x => x.Value)
                .ToList()
                .ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x))
                        stackList.GetValueOrDefault(ctr).Push(x);
                    ctr++;
                });
            ctr = 1;
        }

        foreach(string line in craneMoves)
        {
            var matches = Regex.Matches(line, pattern_moves)
                  .SelectMany(m =>
                m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value)
                                   : new[] { m.Value }
            ).ToArray();
            var arr = Array.ConvertAll(matches, int.Parse);
            moves.Add(new Move(arr[0], arr[1], arr[2]));
        }
    }
    record Move(int qty, int source, int destination);
}
