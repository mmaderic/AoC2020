﻿using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static string[] Input;
        static readonly string[] EyeColors =
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        static readonly Regex HairColor = new Regex(@"^#(?:[0-9a-f]{6})$");

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var count = RunTask();
            var (miliseconds, ticks) = Benchmark.Execute(() => RunTask());

            Console.WriteLine(count);
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~16 ms
        }

        static int RunTask()
        {
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

            return count;
        }

        static int Validate(int start, int end)
        {
            var passport = string.Join(" ", Input[start..end]);
            var passportFields = passport.Split(" ");
            if (passportFields.Length < 7)
                return 0;

            if (passportFields.Length == 7)
            {
                if (!passport.Contains("cid"))
                    return ValidateFields(passportFields);
                return 0;
            }

            return ValidateFields(passportFields);
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
            var match = HairColor.Match(color);

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
