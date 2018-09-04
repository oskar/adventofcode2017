using System;
using System.IO;
using System.Linq;

namespace day5
{
  class Program
  {
    static void Main(string[] args)
    {
      var jumpOffsets = File.ReadLines("input.txt").Select(s => Convert.ToInt32(s)).ToArray();
      System.Console.WriteLine("Jump offsets: " + jumpOffsets.Length);
      var numberOfJumps = Jumper(jumpOffsets);
      System.Console.WriteLine("Number of jumps: " + numberOfJumps);
    }

    static int Jumper(int[] jumpOffsets)
    {
      var index = 0;
      var numberOfJumps = 0;

      while (index >= 0 && index < jumpOffsets.Length)
      {
        var offset = jumpOffsets[index];
        if (offset > 2)
        {
          jumpOffsets[index]--;
        }
        else
        {
          jumpOffsets[index]++;
        }
        index += offset;
        numberOfJumps++;
      }

      return numberOfJumps;
    }
  }
}
