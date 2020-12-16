using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day09
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var input = new long[]
            //{
            //    35,
            //    20 ,
            //    15 ,
            //    25 ,
            //    47 ,
            //    40 ,
            //    62 ,
            //    55 ,
            //    65 ,
            //    95 ,
            //    102,
            //    117,
            //    150,
            //    182,
            //    127,
            //    219,
            //    299,
            //    277,
            //    309,
            //    576,
            //};

            //int preambleLength = 5;

            var input = (await File.ReadAllLinesAsync("input.txt")).Select(long.Parse).ToArray();
            int preambleLength = 25;

            long weakNumber = XmasCracker.FindFirstWeakNumber(input, preambleLength);
            Console.WriteLine("First weak number: {0}", weakNumber);

            long weakness = XmasCracker.FindEncryptionWeakness(input, weakNumber);
            Console.WriteLine("Encryption weakness: {0}", weakness);
        }
    }

    class XmasCracker
    {
        public static long FindFirstWeakNumber(ReadOnlyMemory<long> input, int preambleLength)
        {
            for (int i = preambleLength; i < input.Length; ++i)
            {
                var current = input.Span[i];

                var preamble = input.Slice(i - preambleLength, preambleLength);

                if (IsWeak(preamble, current))
                {
                    return current;
                }
            }

            throw new InvalidOperationException("No weak number found in input.");
        }

        public static long FindEncryptionWeakness(ReadOnlyMemory<long> input, long weakNumber)
        {
            var inputSpan = input.Span;

            int firstIndex = 0;
            int secondIndex = 0;
            bool found = false;
            for (int i = 0; i < input.Length && !found; ++i)
            {
                firstIndex = i;
                long sum = inputSpan[i];

                for (int j = i + 1; j < input.Length && !found && sum < weakNumber; ++j)
                {
                    secondIndex = j;

                    sum += inputSpan[j];

                    if (sum == weakNumber)
                    {
                        found = true;
                    }
                }
            }

            var inputRange = inputSpan.Slice(firstIndex, (secondIndex - firstIndex)).ToArray();

            return inputRange.Min() + inputRange.Max();
        }

        private static bool IsWeak(ReadOnlyMemory<long> preamble, long candidate)
        {
            var preambleSpan = preamble.Span;
            for (int i = 0; i < preamble.Length; ++i)
            {
                var left = preambleSpan[i];
                for (int j = 0; j < preamble.Length; ++j)
                {
                    var right = preambleSpan[j];

                    if ((left + right) == candidate)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
