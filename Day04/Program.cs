using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static string[] Input;
        static string[] EyeColors =
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var watch = new Stopwatch();
            watch.Start();

            var count = 0;
            var start = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == string.Empty)
                {
                    count += Validate(start, i);
                    start = i + 1;
                }                
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds); // ~77 ms
            Console.WriteLine(count);
        }

        static int Validate(int start, int end)
        {
            var lines = new List<string>();
            for (int i = start; i < end; i++)
                lines.Add(Input[i]);

            var passport = string.Join(" ", lines).Split(" ");
            if (passport.Length < 7)
                return 0;

            if (passport.Length == 7)
            {
                if (!string.Join(" ", passport).Contains("cid"))
                    return ValidateFields(passport);
                return 0;
            }

            return ValidateFields(passport);
        }

        static int ValidateFields(string[] passport)
        {
            var byr = ValidateYearRange(passport.First(x => x.StartsWith("byr")), 1920, 2002);
            var iyr = ValidateYearRange(passport.First(x => x.StartsWith("iyr")), 2010, 2020);
            var eyr = ValidateYearRange(passport.First(x => x.StartsWith("eyr")), 2020, 2030);
            var hgt = ValidateHeight(passport.First(x => x.StartsWith("hgt")));
            var hcl = ValidateHairColor(passport.First(x => x.StartsWith("hcl")));
            var ecl = ValidateEyeColor(passport.First(x => x.StartsWith("ecl")));
            var pid = ValidatePassportId(passport.First(x => x.StartsWith("pid")));

            return (byr && iyr && eyr && hgt && hcl && ecl && pid) ? 1 : 0;
        }

        static bool ValidateYearRange(string field, int min, int max)
        {
            var year = Convert.ToInt32(field.Split(":")[1]);
            return year >= min && year <= max;
        }

        static bool ValidateHeight(string field)
        {
            var height = field.Split(":")[1];
            if (height.EndsWith("cm"))
            {
                var num = Convert.ToInt32(height.Split("cm")[0]);
                return num >= 150 && num <= 193;
            }
            if (height.EndsWith("in"))
            {
                var num = Convert.ToInt32(height.Split("in")[0]);
                return num >= 59 && num <= 76;
            }

            return false;
        }

        static bool ValidateHairColor(string field)
        {
            var color = field.Split(":")[1];
            Regex regex = new Regex(@"^#(?:[0-9a-f]{6})$");
            Match match = regex.Match(color);

            return match.Success;
        }

        static bool ValidateEyeColor(string field)
        {
            var color = field.Split(":")[1];
            return EyeColors.Contains(color);
        }

        static bool ValidatePassportId(string field)
        {
            var number = field.Split(":")[1];
            if (number.Length != 9)
                return false;

            return int.TryParse(number, out int _);
        }
    }
}
