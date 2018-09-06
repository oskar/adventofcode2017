using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day7
{
  class Program
  {
    static readonly Regex _rx = new Regex(@"^(?<name>[a-z]*)\s\((?<weight>\d*)\)(\s\-\>\s)?(?<children>.*)?$");

    static void Main(string[] args)
    {
      var programs = File.ReadLines("input.txt")
        .Select(ParseLine)
        .ToDictionary(p => p.name, p => (p.weight, p.children));

      var allChildren = programs.Values.Select(c => c.children).SelectMany(c => c).ToHashSet();
      var root = programs.Keys.FirstOrDefault(name => !allChildren.Contains(name));
      Console.WriteLine("Root program: " + root);

      var tree = CreateTree(programs, root);

      PrintTree(tree);
      CheckWeights(tree);
    }

    static (string name, int weight, string[] children) ParseLine(string l)
    {
      var groups = _rx.Matches(l)[0].Groups;
      var name = groups["name"].Value;
      var weight = Convert.ToInt32(groups["weight"].Value);
      var children = groups["children"].Length > 0 ? groups["children"].Value.Replace(" ", "").Split(',') : new string[0];
      return (name, weight, children);
    }

    static Tree CreateTree(IDictionary<string, (int weight, string[] children)> programs, string name)
    {
      var tree = new Tree(name, programs[name].weight);

      foreach (var child in programs[name].children)
      {
        tree.Children.Add(CreateTree(programs, child));
      }

      return tree;
    }

    static void PrintTree(Tree tree, int indentation = 0)
    {
      var padding = new string(' ', indentation);
      Console.WriteLine($"{padding}{tree.Name} ({tree.Weight})");
      foreach (var child in tree.Children)
      {
        PrintTree(child, indentation + 1);
      }
    }

    static int GetWeight(Tree tree) => tree.Weight + tree.Children.Sum(GetWeight);

    static int CheckWeights(Tree tree)
    {
      var childWeights = tree.Children.Select(CheckWeights);

      if (childWeights.Any())
      {
        var firstWeight = childWeights.First();
        if (!childWeights.All(w => w == firstWeight))
        {
          Console.WriteLine(
            $"Unbalanced tree: {tree.Name} ({tree.Weight})\n" +
            $"Child weights: {string.Join(", ", childWeights)}");
          PrintTree(tree);
          throw new InvalidOperationException();
        }
      }

      return tree.Weight + childWeights.Sum();
    }
  }

  class Tree
  {
    public string Name { get; }
    public int Weight { get; }
    public ICollection<Tree> Children { get; }

    public Tree(string name, int weight)
    {
      Name = name;
      Weight = weight;
      Children = new List<Tree>();
    }
  }
}
