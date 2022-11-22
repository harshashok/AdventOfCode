using System;
namespace AdventOfCode;

class Updater
{
    public async Task Update(int year, int day)
    {
        
        var dir = SolverExtensions.WorkingDir(year, day);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}

