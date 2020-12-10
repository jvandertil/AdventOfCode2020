using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Day03
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var grid = await LoadGridFromFileAsync("input.txt");
            //var grid = LoadTestGrid();

            Console.WriteLine("Number of trees (3, 1): " + CountTrees(grid, 3, 1));

            int[] slopeCount = {
                CountTrees(grid, 1, 1),
                CountTrees(grid, 3, 1),
                CountTrees(grid, 5, 1),
                CountTrees(grid, 7, 1),
                CountTrees(grid, 1, 2),
            };

            long product = 1;
            foreach (var count in slopeCount)
            {
                product *= count;
            }

            Console.WriteLine("Product of tree counts: " + product);
        }

        static int CountTrees(Grid grid, int right, int down)
        {
            var counter = new TreeCounter(grid, (right, down));

            return counter.CountTrees();
        }

        static Grid LoadTestGrid()
        {
            return new Grid(@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#".Split("\r\n"));
        }

        static async Task<Grid> LoadGridFromFileAsync(string path)
        {
            var grid = await File.ReadAllLinesAsync(path);

            return new Grid(grid);
        }
    }

    public class TreeCounter
    {
        private readonly Grid _grid;
        private readonly (int right, int down) _stepSize;

        private int _currentX;
        private int _currentY;

        public TreeCounter(Grid grid, (int right, int down) stepSize)
        {
            _grid = grid;
            _stepSize = stepSize;

            _currentX = 0;
            _currentY = 0;
        }

        public int CountTrees()
        {
            int count = 0;

            while (Move())
            {
                if (_grid.GetContents(_currentX, _currentY) == '#')
                {
                    count++;
                }
            }

            return count;
        }

        private bool Move()
        {
            _currentX += _stepSize.right;
            _currentY += _stepSize.down;

            return _grid.IsInGrid(_currentX, _currentY);
        }
    }

    public class Grid
    {
        private readonly string[] _grid;
        private readonly int _gridWidth;

        public Grid(string[] grid)
        {
            _grid = grid;
            _gridWidth = _grid[0].Length;
        }

        public bool IsInGrid(int x, int y)
        {
            if (y >= _grid.Length || y < 0)
            {
                return false;
            }

            return x >= 0;
        }

        public char GetContents(int x, int y)
        {
            return _grid[y][x % _gridWidth];
        }
    }
}
