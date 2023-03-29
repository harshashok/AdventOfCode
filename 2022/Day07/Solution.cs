using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using adventofcode.Lib.Model;

namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]      
class Solution : Solver {

    TreeNode<FileNode> current;
    TreeNode<FileNode> head;
    Dictionary<string, long> dirMap = new();

    public object PartOne(string input) {
        ParseInput(input);
        return ComputeDirSizes().Select(x => x.Value)
            .Where(z => z <= 100000)
            .Sum(s => s);
    }

    public object PartTwo(string input) {
        long freedSpace = 30000000 - (70000000 - dirMap.GetValueOrDefault("/"));
        return dirMap.Select(x => x.Value)
            .Where(z => z >= freedSpace)
            .Min();
    }

    public Dictionary<string, long> ComputeDirSizes()
    {
        Stack<TreeNode<FileNode>> stack_postorder = new();
        Stack<TreeNode<FileNode>> stack = new();

        if (head != null) stack.Push(head);

        while(stack.Count != 0)
        {
            var treeNode = stack.Pop();
            var fileNode = treeNode.GetValue();
            string uniqueDirName = GetUniqueDirName(treeNode);

            if (fileNode.fileType == "dir" && !dirMap.ContainsKey(uniqueDirName))
            {
                dirMap.Add(uniqueDirName, fileNode.size);
            }

            stack_postorder.Push(treeNode);
            if(stack_postorder.Peek().GetChildren().Any())
            {
                var children = stack_postorder.Peek().GetChildren();
                children.ToList().ForEach(x => stack.Push(x));
            }
        }

        while(stack_postorder.Count != 0)
        {
            var node = stack_postorder.Pop();
            var fileNode = node.GetValue();

            if(node.GetParent() != null)
            {
                var parentNode = node.GetParent();
                string uniqueDir = GetUniqueDirName(parentNode);
                if(node.GetValue().fileType == "file")
                {
                    dirMap[uniqueDir] = dirMap.GetValueOrDefault(uniqueDir) + node.GetValue().size;
                }
                else
                {
                    dirMap[uniqueDir] = dirMap.GetValueOrDefault(uniqueDir) + dirMap[GetUniqueDirName(node)];
                }
            }
        }
        return dirMap;
    }

    private string GetUniqueDirName(TreeNode<FileNode> node)
    {
        TreeNode<FileNode> current = node;
        string dirName = string.Empty;

        while(current.GetValue().name != "/")
        {
            dirName += "-";
            dirName += current.GetValue().name;
            current = current.GetParent();
        }
        dirName += "/";

        return dirName;
    }

    public void ParseInput(string input)
    {
        var inp = input.Split("\n").First();
        foreach (var cmd in input.Split("\n"))
        {
            var args = cmd.Split(' ');
            var action =
                Command(args, Args("\\$", "cd", "(\\w*[.]*\\w*|[/]|[..])"), m =>
                {
                    string dirName = m[2];
                    return () => ChangeDir(dirName);
                }) ??
                Command(args, Args("dir", "(\\w*[.]*\\w*|[/]|[..])"), m =>
                {
                    string dirName = m[1];
                    return () => AddOrListDir(dirName);
                }) ??
                Command(args, Args("(\\d+)", "(\\w*[.]*\\w*)"), m =>
                {
                    string fileSize = m[0];
                    string fileName = m[1];
                    return () => AddOrListFile(fileName, long.Parse(fileSize));
                }) ??
                new Action(() => { });

            try
            {
                action();
            }
            catch
            {
                throw;
            }
        }
    }

    void ChangeDir(string dirName)
    {
        if (dirName.Equals("/"))
        {
            if(head == null)
            {
                TreeNode<FileNode> node = new(new FileNode("dir", "/", 0));
                head = node;
            }
            current = head;
        }
        else if(dirName.Equals(".."))
        {
            current = current.GetParent();
        }
        else
        {
            current = current.GetChildren()
                .Where(x => x.GetValue().name == dirName)
                .FirstOrDefault();
        }
    }

    void AddOrListDir(string dirName)
    {
        var t = current.GetChildren()
                .Where(x => x.GetValue().name == dirName)
                .FirstOrDefault();

        if (t == null) current.AddChild(new FileNode("dir", dirName, 0)); 
    }

    void AddOrListFile(string fileName, long s)
    {
        var t = current.GetChildren()
        .Where(x => x.GetValue().name == fileName && x.GetValue().size == s)
        .FirstOrDefault();

        if (t == null) current.AddChild(new FileNode("file", fileName, s));
    }

    Action Command(string[] args, string[] regexes, Func<string[], Action> parse)
    {
        if (args.Length != regexes.Length)
        {
            return null;
        }
        var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
        if (!matches.All(match => match.Success))
        {
            return null;
        }
        try
        {

            return parse(matches.SelectMany(m =>
                    m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value)
                                       : new[] { m.Value }
                ).ToArray());
        }
        catch
        {
            return null;
        }
    }

    string[] Args(params string[] regex) => regex;

    record FileNode(string fileType, string name, long size);
}
