using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Group> groups;
            using (var stream = File.OpenRead("input.txt"))
            {
                using var reader = new StreamReader(stream);
                groups = await ReadGroupsAsync(reader);
            }

            Console.WriteLine("Questions anyone answered yes: {0}", groups.Sum(x => x.CountUniqueAnswers()));
            Console.WriteLine("Questions all in group answered yes: {0}", groups.Sum(x => x.CountConsensusAnswers()));
        }

        static async Task<List<Group>> ReadGroupsAsync(TextReader reader)
        {
            var currentGroup = new Group();
            var groups = new List<Group> { currentGroup };

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line == string.Empty)
                {
                    currentGroup = new Group();
                    groups.Add(currentGroup);
                    continue;
                }

                currentGroup.AddAnswer(line);
            }

            return groups;
        }
    }

    public class Group
    {
        private readonly List<string> _answers;

        public Group()
        {
            _answers = new List<string>();
        }

        public void AddAnswer(string input)
        {
            _answers.Add(input);
        }

        public int CountUniqueAnswers()
        {
            return _answers.Aggregate(string.Concat).Distinct().Count();
        }

        public int CountConsensusAnswers()
        {
            var answers = _answers.Aggregate(string.Concat).Distinct();

            int count = answers.Count(x => _answers.All(y => y.Contains(x)));

            return count;
        }
    }

}
