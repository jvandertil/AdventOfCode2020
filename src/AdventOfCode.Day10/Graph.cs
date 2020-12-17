using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day10
{
    public class Graph
    {
        private readonly Dictionary<int, Vertex> _vertices;

        public Graph()
        {
            _vertices = new Dictionary<int, Vertex>();
        }

        public void AddVertex(int value)
        {
            _vertices.Add(value, new Vertex(value));
        }

        public void ConnectVertices(int start, int end)
        {
            _vertices[end].AddIncoming(_vertices[start]);
        }

        public long CalculatePaths(int start, int end)
        {
            Dictionary<int, long> paths = new();

            Stack<Vertex> stack = new();
            stack.Push(_vertices[end]);

            while (stack.TryPeek(out var current))
            {
                if (current.Value == start)
                {
                    paths[current.Value] = 1;
                    stack.Pop();
                }
                else
                {
                    if (current.Incoming.All(x => paths.ContainsKey(x.Value)))
                    {
                        paths[current.Value] = current.Incoming.Sum(x => paths[x.Value]);
                        stack.Pop();
                    }
                    else
                    {
                        foreach (var incomingVertex in current.Incoming)
                        {
                            if (!paths.ContainsKey(incomingVertex.Value))
                            {
                                stack.Push(incomingVertex);
                            }
                        }
                    }
                }
            }

            return paths[end];
        }

        private class Vertex
        {
            private readonly List<Vertex> _incoming;

            public IReadOnlyList<Vertex> Incoming => _incoming;

            public int Value { get; }

            public Vertex(int value)
            {
                _incoming = new();

                Value = value;
            }

            public void AddIncoming(Vertex vertex)
            {
                _incoming.Add(vertex);
            }

            public override string ToString()
            {
                return $"{{ {Value}, {string.Join(",", _incoming.Select(x => x.Value))}}}";
            }
        }
    }
}
