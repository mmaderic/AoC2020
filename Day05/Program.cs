using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static string[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var watch = new Stopwatch();
            watch.Start();

            var max = 0;
            var map = Enumerable.Range(0, 128).Select(x => new int[8]).ToArray();
            foreach (var item in Input)
            {
                var row = ReadValue(item, 0, 127, 'F', 0, 6);
                var column = ReadValue(item, 0, 7, 'L', 7, 10);

                var id = row * 8 + column;
                max = id > max ? id : max;

                map[row][column] = id;
            }

            var ids = map.SelectMany(x => x).ToArray();
            for (int i = 1; i < ids.Length - 1; i++)
            {
                if (ids[i - 1] != 0 && ids[i] == 0 && ids[i + 1] != 0)
                {
                    Console.WriteLine(i);
                    break;
                }
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds); // ~35 ms
        }

        static int ReadValue(string boardingPass, int min, int max, char c, int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                if (boardingPass[i] == c)
                    max -= (max - min + 1) / 2;
                else
                    min += (max - min + 1) / 2;
            }

            return boardingPass[6] == c ? min : max;
        }
    }
}
