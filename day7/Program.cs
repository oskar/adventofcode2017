using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day7
{
  class Program
  {
    static readonly Regex _rx = new Regex(@"^(?<name>[a-z]*)\s\(\d*\)(\s\-\>\s)?(?<children>.*)?$");

    static void Main(string[] args)
    {
      var programs = File.ReadLines("input.txt")
        .Select(ParseLine)
        .ToDictionary(p => p.name, p => p.children);

      var allChildren = programs.Values.SelectMany(c => c).ToHashSet();
      var root = programs.Keys.FirstOrDefault(name => !allChildren.Contains(name));
      Console.WriteLine("Root program: " + root);
    }

    static (string name, string[] children) ParseLine(string l)
    {
      var groups = _rx.Matches(l)[0].Groups;
      var name = groups["name"];
      var children = groups["children"].Length > 0 ? groups["children"].Value.Replace(" ", "").Split(',') : new string[0];
      return (name: name.Value, children: children);
    }

    static void PrintProgram(string name, string[] children) => Console.WriteLine(name + ": " + (children.Length > 0 ? string.Join(",", children) : "-"));
  }
}
