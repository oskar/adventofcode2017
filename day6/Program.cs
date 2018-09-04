using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6
{
  class Program
  {
    static void Main(string[] args)
    {
      var banks = File.ReadAllText("input.txt").Split("\t").Select(s => Convert.ToInt32(s)).ToArray();
      PrintBanks(banks);
      var cycles = Redistribute(banks);
      Console.WriteLine("Cycles in loop: " + cycles);
    }

    static int Redistribute(int[] banks)
    {
      var previousBanks = new Dictionary<string, int>();

      while (true)
      {
        var hash = GetBanksHash(banks);
        if(previousBanks.ContainsKey(hash))
        {
          return previousBanks.Count - previousBanks[hash];
        }
        previousBanks.Add(hash, previousBanks.Count);
        var highestIndex = GetHighestIndex(banks);
        Reallocate(banks, highestIndex);
      }
    }

    static string GetBanksHash(int[] banks) => string.Join(',', banks);
    static void PrintBanks(int[] banks) => Console.WriteLine("Banks: " + GetBanksHash(banks));

    static void Reallocate(int[] banks, int index)
    {
      var blocks = banks[index];
      banks[index] = 0;

      for (int i = 1; i <= blocks; i++)
      {
        banks[(index + i) % banks.Length]++;
      }

      PrintBanks(banks);
    }

    static int GetHighestIndex(int[] banks)
    {
      var highestIndex = 0;

      for (var i = 0; i < banks.Length; i++)
      {
        if (banks[i] > banks[highestIndex])
        {
          highestIndex = i;
        }
      }

      return highestIndex;
    }
  }
}
