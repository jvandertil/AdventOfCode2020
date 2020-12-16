using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day08
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var instructions = new[] {
            //    "nop +0",
            //    "acc +1",
            //    "jmp +4",
            //    "acc +3",
            //    "jmp -3",
            //    "acc -99",
            //    "acc +1",
            //    "jmp -4",
            //    "acc +6",
            //};

            var instructions = await File.ReadAllLinesAsync("input.txt");

            Console.WriteLine("Value of accumulator before loop: {0}", StupidGameEmulator.CalculateAccUntilLoop(instructions));
            BruteInfiniteLoop(instructions);
        }

        static void BruteInfiniteLoop(string[] instructions)
        {
            var nopToJmp = BruteInfiniteLoopNopToJmp(instructions);
            if (nopToJmp is not null)
            {
                Console.WriteLine(StupidGameEmulator.CalculateAccUntilLoop(nopToJmp));
                return;
            }

            var jmpToNop = BruteInfiniteLoopJmpToNop(instructions);
            Console.WriteLine(StupidGameEmulator.CalculateAccUntilLoop(jmpToNop));
        }

        static string[] BruteInfiniteLoopNopToJmp(string[] instructions)
        {
            bool found = false;
            int pointer = 0;
            string[] copyInstructions = null;

            while (!found && (pointer < instructions.Length))
            {
                copyInstructions = instructions.ToArray();

                if (copyInstructions[pointer].StartsWith("nop"))
                {
                    copyInstructions[pointer] = copyInstructions[pointer].Replace("nop", "jmp");
                    found = !StupidGameEmulator.ProgramHasInfiniteLoop(copyInstructions);
                }

                pointer++;
            }

            return found ? copyInstructions : null;
        }

        static string[] BruteInfiniteLoopJmpToNop(string[] instructions)
        {
            bool found = false;
            int pointer = 0;
            string[] copyInstructions = null;

            while (!found && (pointer < instructions.Length))
            {
                copyInstructions = instructions.ToArray();

                if (copyInstructions[pointer].StartsWith("jmp"))
                {
                    copyInstructions[pointer] = copyInstructions[pointer].Replace("jmp", "nop");
                    found = !StupidGameEmulator.ProgramHasInfiniteLoop(copyInstructions);
                }

                pointer++;
            }

            return found ? copyInstructions : null;
        }
    }

    public class StupidGameEmulator
    {
        public static bool ProgramHasInfiniteLoop(string[] instructions)
        {
            int pointer = 0;
            HashSet<int> instructionsProcessed = new HashSet<int>();
            while (!instructionsProcessed.Contains(pointer)
                   && pointer < instructions.Length)
            {
                string currentInstruction = instructions[pointer];
                instructionsProcessed.Add(pointer);

                if (currentInstruction.StartsWith("nop"))
                {
                    pointer++;
                }
                else if (currentInstruction.StartsWith("jmp"))
                {
                    pointer += int.Parse(currentInstruction.Substring(4), NumberStyles.Integer | NumberStyles.AllowLeadingSign);
                }
                else if (currentInstruction.StartsWith("acc"))
                {
                    pointer++;
                }
            }

            return pointer < instructions.Length;
        }

        public static int CalculateAccUntilLoop(string[] instructions)
        {
            HashSet<int> instructionsProcessed = new HashSet<int>();

            int accumulator = 0;

            int pointer = 0;
            while (!instructionsProcessed.Contains(pointer)
                   && pointer < instructions.Length)
            {
                string currentInstruction = instructions[pointer];
                instructionsProcessed.Add(pointer);

                if (currentInstruction.StartsWith("nop"))
                {
                    pointer++;
                }
                else if (currentInstruction.StartsWith("jmp"))
                {
                    pointer += int.Parse(currentInstruction.Substring(4), NumberStyles.Integer | NumberStyles.AllowLeadingSign);
                }
                else if (currentInstruction.StartsWith("acc"))
                {
                    accumulator += int.Parse(currentInstruction.Substring(4), NumberStyles.Integer | NumberStyles.AllowLeadingSign);
                    pointer++;
                }
            }

            return accumulator;
        }
    }

}
