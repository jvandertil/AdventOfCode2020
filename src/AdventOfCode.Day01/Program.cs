using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var lines = await File.ReadAllLinesAsync("input.txt");
            var input = lines.Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();

            var pair = GetPair(input, 2020);

            Console.WriteLine("pair that sums to 2020: {0} * {1} = {2} ", pair.Item1, pair.Item2, pair.Item1 * pair.Item2);

            var triplet = GetTriplet(input, 2020);
            Console.WriteLine("triplet that sums to 2020: {0} * {1} * {2} = {3} ", triplet.Item1, triplet.Item2, triplet.Item3, triplet.Item1 * triplet.Item2 * triplet.Item3);
        }

        static (int, int) GetPair(int[] input, int wantedSum)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                for (int j = 0; j < input.Length; ++j)
                {
                    var sum = input[i] + input[j];
                    if (sum == wantedSum)
                    {
                        return (input[i], input[j]);
                    }
                }
            }

            throw new InvalidOperationException("Not found.");
        }

        static (int, int, int) GetTriplet(int[] input, int wantedSum)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                for (int j = 0; j < input.Length; ++j)
                {
                    for (int k = 0; k < input.Length; ++k)
                    {
                        var sum = input[i] + input[j] + input[k];
                        if (sum == wantedSum)
                        {
                            return (input[i], input[j], input[k]);
                        }
                    }
                }
            }

            throw new InvalidOperationException("Not found.");
        }
    }
}
