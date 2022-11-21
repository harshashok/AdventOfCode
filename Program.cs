using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

Console.WriteLine("Hello, World!");

//args = new string[] { "2021/24" };
var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass)  //&& typeof(Solver).IsAssignableFrom(t)
    .OrderBy(t => t.FullName)
    .ToArray();

tsolvers.ToList().ForEach(i => Console.Write("{0}\t", i));
Console.WriteLine();

var action = Command(args, Args("([0-9]+)/([0-9]+)"), m =>
{
    var year = int.Parse(m[0]);
    var day = int.Parse(m[1]);
    Console.WriteLine($"Year : {year} | Day : {day}");
    return () => Console.WriteLine("END");

}) ??
Command(args, Args("([0-9]+)"), m =>
{
    var year = int.Parse(m[0]);
    Console.WriteLine($"Year : {year} ");
    return () => Console.WriteLine("END");

}) ??
new Action(() => {
    Console.WriteLine("No args present");
});


try
{
    action();
}
catch(Exception ex)
{
    Console.WriteLine("Exception : " + ex.InnerException?.Message);
}


Action? Command(string[] args, string[] regexes, Func<string[], Action> parse)
{
    if(args.Length != regexes.Length)
    {
        return null;
    }

    var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
    if(!matches.All(match => match.Success))
    {
        return null;
    }

    try
    {
        return parse(matches.SelectMany(m => m.Groups.Count > 1 ?
                            m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) :
                            new[] { m.Value }
                    ).ToArray());
    }
    catch
    {
        return null;
    }
}

string[] Args(params string[] regex)
{
    return regex;
}