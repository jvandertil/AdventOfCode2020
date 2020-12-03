using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Day02
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int validSledRentalPasswordCount = 0;
            int validTobogganPasswordCount = 0;

            await foreach (var entry in GetEntriesAsync())
            {
                if (entry.ConfirmsToSledRentalPolicy())
                {
                    validSledRentalPasswordCount++;
                }

                if (entry.ConfirmsToTobogganCorporatePolicy())
                {
                    validTobogganPasswordCount++;
                }
            }

            Console.WriteLine("Number of valid sled rental passwords: {0}", validSledRentalPasswordCount);
            Console.WriteLine("Number of valid toboggan passwords: {0}", validTobogganPasswordCount);
        }

        static async IAsyncEnumerable<PasswordEntry> GetEntriesAsync()
        {
            using var file = File.OpenRead("input.txt");
            using var reader = new StreamReader(file);

            string readLine;
            while ((readLine = await reader.ReadLineAsync()) is not null)
            {
                yield return ParseLine(readLine);
            }
        }

        static PasswordEntry ParseLine(string entry)
        {
            var line = entry.AsSpan();

            // Format: '1-3 a: abcde'
            // Format: '16-17 d: ddddgdddddkddddsxddd'
            int dash = line.IndexOf('-');
            int space = line.IndexOf(' ');
            int colon = line.IndexOf(':');

            var minRequired = line.Slice(0, dash);
            var maxRequired = line.Slice((dash + 1), (space - dash));
            var letter = line.Slice(space + 1, 1)[0];

            var password = line.Slice(colon + 2);

            return new PasswordEntry(
                new PasswordPolicy(
                    int.Parse(minRequired, provider: CultureInfo.InvariantCulture),
                    int.Parse(maxRequired, provider: CultureInfo.InvariantCulture),
                    letter
                ),
                new string(password)
            );
        }
    }

    record PasswordEntry(PasswordPolicy Policy, string Input)
    {
        public bool ConfirmsToSledRentalPolicy()
        {
            int count = 0;
            foreach (var letter in Input)
            {
                if (letter == Policy.Letter)
                {
                    count++;
                }
            }

            int minRequired = Policy.FirstNumber;
            int maxRequired = Policy.SecondNumber;

            return count >= minRequired && count <= maxRequired;
        }

        public bool ConfirmsToTobogganCorporatePolicy()
        {
            // Adjusted for 0 based indexing.
            int firstIndex = Policy.FirstNumber - 1;
            int secondIndex = Policy.SecondNumber - 1;

            return (Input[firstIndex] == Policy.Letter) ^ (Input[secondIndex] == Policy.Letter);
        }
    }

    record PasswordPolicy(int FirstNumber, int SecondNumber, char Letter);
}
