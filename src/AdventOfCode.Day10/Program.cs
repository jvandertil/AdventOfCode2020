using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day10
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //IEnumerable<int> adapters = new[]
            //{
            //    16,
            //    10,
            //    15,
            //    5,
            //    1,
            //    11,
            //    7,
            //    19,
            //    6,
            //    12,
            //    4,
            //}.OrderBy(x => x);

            //IEnumerable<int> adapters = new[]
            //{
            //28,
            //    33,
            //    18,
            //    42,
            //    31,
            //    14,
            //    46,
            //    20,
            //    48,
            //    47,
            //    24,
            //    23,
            //    49,
            //    45,
            //    19,
            //    38,
            //    39,
            //    11,
            //    1 ,
            //    32,
            //    25,
            //    35,
            //    8 ,
            //    17,
            //    7 ,
            //    9 ,
            //    4 ,
            //    2 ,
            //    34,
            //    10,
            //    3 ,
            //}.OrderBy(x => x);

            IEnumerable<int> adapters = (await File.ReadAllLinesAsync("input.txt")).Select(int.Parse).OrderBy(x => x);

            long multipliedDifference = MultipliedJoltDifferences(adapters.Prepend(0).ToArray());

            Console.WriteLine("Multiplied jolt difference: {0}", multipliedDifference);

            var graph = CreateGraph(adapters.Prepend(0).ToArray());
            var max = adapters.Max();
            var count = graph.CalculatePaths(0, max);

            Console.WriteLine("Number of variations: {0}", count);
        }

        static Graph CreateGraph(int[] input)
        {
            var graph = new Graph();

            int previous = input[0];
            graph.AddVertex(previous);

            int index = 0;
            for (int i = 1; i < input.Length; ++i)
            {
                var current = input[i];
                graph.AddVertex(current);

                if ((current - previous) >= 3)
                {
                    CreateConnectedGraph(input.AsSpan().Slice(index, (i - index)));
                    graph.ConnectVertices(previous, current);
                    index = i;
                }

                previous = current;
            }

            CreateConnectedGraph(input.AsSpan().Slice(index));

            return graph;

            void CreateConnectedGraph(ReadOnlySpan<int> input)
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    for (int j = i + 1; j < input.Length; ++j)
                    {
                        int diff = input[j] - input[i];

                        if (diff <= 3)
                        {
                            graph.ConnectVertices(input[i], input[j]);
                        }
                    }
                }
            }
        }

        static long MultipliedJoltDifferences(int[] input)
        {
            long diffOne = 0;
            long diffThree = 1;
            foreach (var pair in ToPairs(input))
            {
                int diff = (pair.Item2 - pair.Item1);

                if (diff == 1)
                {
                    diffOne++;
                }
                else
                {
                    diffThree++;
                }
            }

            return diffOne * diffThree;

            static IEnumerable<(int, int)> ToPairs(int[] input)
            {
                if (input.Length % 2 != 0)
                {
                    throw new ArgumentException("Uneven number of elements.");
                }

                for (int i = 0; i < input.Length - 1; ++i)
                {
                    yield return (input[i], input[i + 1]);
                }
            }
        }
    }
}
