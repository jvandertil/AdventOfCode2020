using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace AdventOfCode.Day05
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var input = await File.ReadAllLinesAsync("input.txt");
            var seats = input.Select(Seat.Decode).OrderBy(x => x.Id).ToArray();

            Console.WriteLine("Highest id in seats: {0}", seats.Max(x => x.Id));
            Console.WriteLine("Your seat: {0}", FindMissingSeatId(seats));
        }

        public static int FindMissingSeatId(Seat[] seats)
        {
            int prev = seats[0].Id;
            for (int i = 1; i < seats.Length; ++i)
            {
                int currentId = seats[i].Id;
                if (currentId != (prev + 1))
                {
                    return prev + 1;
                }

                prev = currentId;
            }

            throw new InvalidOperationException("Seat not found. Strange.");
        }
    }

    public class Seat
    {
        public int Row { get; init; }
        public int Column { get; init; }

        public int Id => (Row * 8) + Column;

        public override string ToString()
        {
            return $"{{ Row: {Row}, Column: {Column}, Id: {Id} }}";
        }

        public static Seat Decode(string input)
        {
            return new Seat
            {
                Column = DecodeColumn(input.AsSpan().Slice(7)),
                Row = DecodeRow(input.AsSpan().Slice(0, 7)),
            };
        }

        private static int DecodeRow(ReadOnlySpan<char> input)
        {
            Span<int> rows = stackalloc int[128];
            for (int i = 0; i < rows.Length; ++i)
            {
                rows[i] = i;
            }

            int seat = WalkArray(rows, input);

            return seat;
        }

        private static int DecodeColumn(ReadOnlySpan<char> input)
        {
            Span<int> rows = stackalloc int[8];
            for (int i = 0; i < rows.Length; ++i)
            {
                rows[i] = i;
            }

            Span<char> normalizedInput = stackalloc char[input.Length];
            for (int i = 0; i < input.Length; ++i)
            {
                normalizedInput[i] = input[i] == 'R' ? 'B' : 'F';
            }

            return WalkArray(rows, normalizedInput);
        }

        private static int WalkArray(ReadOnlySpan<int> array, ReadOnlySpan<char> input)
        {
            int start = 0;
            int end = array.Length - 1;

            // Kinda binary search
            foreach (var action in input)
            {
                if (action == 'F')
                {
                    int current = (int)Math.Floor((start + end) / 2D);

                    // Keep lower half.
                    end = current;
                }
                else
                {
                    int current = (int)Math.Ceiling((start + end) / 2D);

                    // Keep upper half.
                    start = current;
                }
            }

            Debug.Assert(end == start);

            return end;
        }
    }
}
