using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static string[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var watch = new Stopwatch();
            watch.Start();

            Loop(ValidateItemEquality);
            Loop(ValidateItemIndexes);

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds); // ~45 ms
        }

        static void Loop(Func<string, bool> method)
        {
            var count = 0;
            foreach (var item in Input)
                count += Convert.ToInt32(method.Invoke(item));

            Console.WriteLine(count);
        }

        static bool ValidateItemEquality(string item)
        {
            var (min, max, target, password) = ParseValue(item);

            int count = 0;
            foreach (var character in password)
            {
                if (character == target)
                    count++;
            }

            return count >= min && count <= max;
        }

        static bool ValidateItemIndexes(string item)
        {
            var (first, second, target, password) = ParseValue(item);     

            return password[--first] == target ^ password[--second] == target;
        }

        static (int, int, char, string) ParseValue(string value)
        {
            var parts = value.Split(" ");
            var (a, b) = GetNumbers(parts[0]);
            var target = parts[1][0];
            var password = parts[2];

            return (a, b, target, password);
        }

        static (int, int) GetNumbers(string item)
        {
            var parts = item.Split("-").Select(x => Convert.ToInt32(x));
            return (parts.ElementAt(0), parts.ElementAt(1));
        }
    }
}
