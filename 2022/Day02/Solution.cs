using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]      
class Solution : Solver {

    public object PartOne(string input) {

        var games = input.Split("\n")
                        .Select(x => x.Split(' ', StringSplitOptions.None));

        return games.Select(x => MatchResult(keyMap.GetValueOrDefault(x[1]), keyMap.GetValueOrDefault(x[0])))
                           .Sum();

    }

    public object PartTwo(string input) {
        var games = input.Split("\n")
                .Select(x => x.Split(' ', StringSplitOptions.None));

        return games.Select(x => MatchResult(StrategyHand(stratMap.GetValueOrDefault(x[1]), keyMap.GetValueOrDefault(x[0])), keyMap.GetValueOrDefault(x[0])))
                           .Sum();
    }

    enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissor = 3
    }
    enum Match
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }

    Dictionary<string, Hand> keyMap = new Dictionary<string, Hand>()
    {
        {"A", Hand.Rock },
        {"B", Hand.Paper },
        {"C", Hand.Scissor },
        {"X", Hand.Rock },
        {"Y", Hand.Paper },
        {"Z", Hand.Scissor },
    };

    Dictionary<string, Match> stratMap = new Dictionary<string, Match>()
    {
        {"X", Match.Lose },
        {"Y", Match.Draw },
        {"Z", Match.Win },
    };

    private int MatchResult(Hand mine, Hand theirs)
    {
        if (mine == Hand.Rock && theirs == Hand.Scissor) return (int)Match.Win + (int)mine;
        if (mine == Hand.Rock && theirs == Hand.Paper)   return (int)Match.Lose + (int)mine;

        if (mine == Hand.Scissor && theirs == Hand.Paper) return (int)Match.Win + +(int)mine;
        if (mine == Hand.Scissor && theirs == Hand.Rock)  return (int) Match.Lose + +(int)mine;

        if (mine == Hand.Paper && theirs == Hand.Rock)    return (int) Match.Win + +(int)mine;
        if (mine == Hand.Paper && theirs == Hand.Scissor) return (int) Match.Lose + +(int)mine;

        return (int)Match.Draw + (int)mine;
    }

    private Hand StrategyHand(Match strategy, Hand oppHand)
    {
        //var strategy = stratMap.GetValueOrDefault(val);

        if(strategy == Match.Win)
        {
            if (oppHand == Hand.Rock)    return Hand.Paper;
            if (oppHand == Hand.Paper)   return Hand.Scissor;
            if (oppHand == Hand.Scissor) return Hand.Rock;
        }

        if(strategy == Match.Lose)
        {
            if (oppHand == Hand.Rock) return Hand.Scissor;
            if (oppHand == Hand.Paper) return Hand.Rock;
            if (oppHand == Hand.Scissor) return Hand.Paper;
        }

        //strategy : Match.Draw
        return oppHand;
    }
}

