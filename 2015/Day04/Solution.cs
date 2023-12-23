using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Y2015.Day04;

[ProblemName("The Ideal Stocking Stuffer")]      
class Solution : Solver {

    public object PartOne(string input) => FindMD5(input, "00000");

    public object PartTwo(string input) => FindMD5(input, "000000");

    int FindMD5(string input, string prefix)
    {
        var bag = new ConcurrentBag<int>();
        Parallel.ForEach(
            Numbers(),           //TSource
            () => MD5.Create(),  //localInit
            (i, state, md5) =>   //body
            {
                var seed = input + i;
                var inputBytes = ASCIIEncoding.ASCII.GetBytes(seed);
                var outputHash = md5.ComputeHash(inputBytes);
                var result = Convert.ToHexString(outputHash);

                if (result.StartsWith(prefix))
                {
                    bag.Add(i);
                    state.Stop();
                }
                return md5;
                
            },
            (_) => {}   //localFinally
        );
        return bag.Min();
    }
    
    IEnumerable<int> Numbers()
    {
        for (int i = 0;;i++)
        {
            yield return i;
        }
    }
}
