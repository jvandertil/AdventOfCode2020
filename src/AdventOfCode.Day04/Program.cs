using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = ReadFromFile("input.txt");
            //var input = TestInput();
            var passportTokens = InputTokenizer.Tokenize(input);
            Console.WriteLine("passports found: " + passportTokens.Count);

            var passports = passportTokens.Select(x => PassportParser.Parse(x)).ToList();

            Console.WriteLine("Passports with all fields: " + passports.Count(x => x.ValidateFieldsExist()));
            Console.WriteLine("Passports with valid fields: " + passports.Count(x => x.ValidateFieldsValid()));
        }

        static string ReadFromFile(string file)
        {
            return File.ReadAllText(file);
        }

        static string TestInput()
        {
            return @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
        }
    }

    internal static class InputTokenizer
    {
        public static IReadOnlyList<string> Tokenize(string input)
        {
            using var stringReader = new StringReader(input);

            List<string> passports = new List<string>();

            string line;

            string buffer = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    passports.Add(buffer);
                    buffer = string.Empty;
                }
                else
                {
                    buffer += line;
                    buffer += " ";
                }
            }

            if (buffer != string.Empty)
            {
                passports.Add(buffer);
            }

            return passports;
        }
    }

    internal static class PassportParser
    {
        public static Passport Parse(string input)
        {
            var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return new Passport(tokens.Select(x => x.Split(':')).ToDictionary(x => x[0].Trim(), x => x[1].Trim()));
        }
    }

    class Passport
    {
        private readonly IReadOnlyDictionary<string, string> _properties;

        public Passport(IReadOnlyDictionary<string, string> properties)
        {
            _properties = properties;
        }

        public bool ValidateFieldsExist()
        {
            return HasProperty("byr")
                && HasProperty("iyr")
                && HasProperty("eyr")
                && HasProperty("hgt")
                && HasProperty("hcl")
                && HasProperty("ecl")
                && HasProperty("pid");
        }

        public bool ValidateFieldsValid()
        {
            return ValidBirthYear()
                && ValidIssueYear()
                && ValidExpirationYear()
                && ValidHeight()
                && ValidHairColor()
                && ValidEyeColor()
                && ValidPassportId();
        }

        private bool ValidBirthYear()
        {
            return HasProperty("byr")
                && int.TryParse(Property("byr"), out int year)
                && (year >= 1920 && year <= 2002);
        }

        private bool ValidIssueYear()
        {
            return HasProperty("iyr")
                && int.TryParse(Property("iyr"), out int year)
                && (year >= 2010 && year <= 2020);
        }

        private bool ValidExpirationYear()
        {
            return HasProperty("eyr")
                && int.TryParse(Property("eyr"), out int year)
                && (year >= 2020 && year <= 2030);
        }

        private bool ValidHeight()
        {
            if (!HasProperty("hgt"))
            {
                return false;
            }

            var heightString = Property("hgt");

            if (heightString.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                return int.TryParse(heightString.Substring(0, heightString.Length - 2), out int height)
                    && (height >= 59 && height <= 76);
            }
            else if (heightString.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                return int.TryParse(heightString.Substring(0, heightString.Length - 2), out int height)
                    && (height >= 150 && height <= 193);
            }
            else
            {
                return false;
            }
        }

        private bool ValidHairColor()
        {
            return HasProperty("hcl")
                && Regex.IsMatch(Property("hcl"), "^#([0-9a-f]){6}$");
        }

        private bool ValidEyeColor()
        {
            string[] validValues =
            {
                "amb",
                "blu",
                "brn",
                "gry",
                "grn",
                "hzl",
                "oth",
            };

            return HasProperty("ecl")
                && validValues.Contains(Property("ecl"));
        }

        private bool ValidPassportId()
        {
            return HasProperty("pid")
                && Regex.IsMatch(Property("pid"), "^[0-9]{9}$");
        }

        private bool HasProperty(string property)
            => _properties.ContainsKey(property);

        private string Property(string propertyName)
            => _properties[propertyName];
    }
}
