using AdventOfCode24.Util;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode24.Days
{
    [TestFixture]
    public partial class Day4 : BaseTest
    {
        private List<string> lines = new List<string>();
        private int[,] grid;
        private int RowCount;
        private int ColCount;

        [OneTimeSetUp]
        public void Setup()
        {
            this.lines = LoadFile.LoadInput(4, 1).ToList();
            this.grid = new int[this.lines.Count, this.lines[0].Length];

            // Populate the grid, changing the letters of XMAS into numbers (1,2,3,4)
            // and everything else as 0
            for (int row = 0; row < this.lines.Count; row++)
            {
                for (int c = 0; c < this.lines[row].Length; c++)
                {
                    this.grid[row, c] = this.ToNum(this.lines[row][c]);
                }
            }

            this.RowCount = this.grid.GetLength(0);
            this.ColCount = this.grid.GetLength(1);
        }

        [Test]
        public void Part1()
        {
            this.TimedTest(() =>
            {
                var res = 0;
                for (int row = 0; row < this.grid.GetLength(0); row++)
                {
                    for (int col = 0; col < this.grid.GetLength(1); col++)
                    {
                        if (grid[row,col] == 1)
                        {
                            res += this.CheckAllDirections(row, col);
                        }
                    }
                }

                Console.WriteLine(res);
            });
        }

        [Test]
        public void Part2()
        {
            this.TimedTest(() =>
            {
                var res = 0;
                var found = new List<(Direction direction, int y, int x)>();
                for (int row = 0; row < this.grid.GetLength(0); row++)
                {
                    for (int col = 0; col < this.grid.GetLength(1); col++)
                    {
                        // In this case, we check A's for surrounding characters
                        if (grid[row, col] == 2)
                        {
                            found.AddRange(this.GetAllMasInstances(row, col));
                        }
                    }
                }

                // Loop each result and check which ones cross
                foreach (var result in found)
                {
                    var middleX = result.direction.GetNeighbourVector().x;
                    var middleY = result.direction.GetNeighbourVector().y;

                    // Check these directions, from the right place..
                    var resToCheck = result.direction.GetCrossingVectors(middleX, middleY);

                    foreach (var toCheck in resToCheck)
                    {
                        res += found.Exists(x => x.direction == toCheck.direction && x.x == toCheck.col && x.y == toCheck.row) ? 1 : 0;
                    }
                }

                Console.WriteLine(res);
            });
        }

        // Checks every direction for XMAS and returns the number of directions that contain the full word
        private int CheckAllDirections(int row, int col) => ((Direction[])Enum.GetValues(typeof(Direction))).Sum(dir => this.CheckThisDirection(dir, 1, row, col) ? 1 : 0);

        private IEnumerable<(Direction dir, int row, int col)> GetAllMasInstances(int row, int col)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (this.CheckThisDirection(dir, 2, row, col))
                {
                    yield return (dir, row, col);
                }

            }
        }

        // Checks the next cell in a given direciton for the next letter, where current is the index of the next letter.
        private bool CheckThisDirection(Direction direction, int current, int row, int col)
        {
            // If current is 4, we've found the word and are done.
            if (current == 4)
            {
                return true;
            }

            // Increment to next letter
            var next = current + 1;

            var nextPosition = this.GetRelativePosition(row, col, direction);

            if (nextPosition.valid && grid[nextPosition.row, nextPosition.col] == next)
            {
                // Recursively keep checking this direction until
                // we either get a full match or incorrect char
                return CheckThisDirection(direction, next, nextPosition.row, nextPosition.col);
            }

            return false;
        }

        private (bool valid, int row, int col) GetRelativePosition(int row, int col, Direction direction)
        {
            var movement = direction.GetNeighbourVector();
            var newCol = col + movement.x;
            var newRow = row + movement.y;

            if (newCol < 0 || newCol >= grid.GetLength(1) || newRow < 0 || newRow >= grid.GetLength(0))
            {
                return (false, 0, 0);
            }

            return (true, newRow, newCol);
        }

        private int ToNum(char c)
        {
            switch (c)
            {
                case 'X':
                    return 1;
                case 'M':
                    return 2;
                case 'A':
                    return 3;
                case 'S':
                    return 4;
                default:
                    return 0;
            }
        }
    }

    public static class DirectionHelpers
    {
        public static Direction Opposite(this Direction dir) =>
            dir switch
            {
                Direction.UpLeft => Direction.DownRight,
                Direction.Up => Direction.Down,
                Direction.UpRight => Direction.DownLeft,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.DownLeft => Direction.UpRight,
                Direction.Down => Direction.Up,
                Direction.DownRight => Direction.UpLeft,
                _ => throw new NotImplementedException(),
            };

        public static Direction Crossing(this Direction dir) =>
            dir switch
            {
                Direction.UpLeft => Direction.DownLeft,
                Direction.Up => Direction.Left,
                Direction.UpRight => Direction.DownRight,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                Direction.DownLeft => Direction.DownRight,
                Direction.Down => Direction.Right,
                Direction.DownRight => Direction.UpRight,
                _ => throw new NotImplementedException(),
            };

        public static List<(Direction direction, int row, int col)> GetCrossingVectors(this Direction dir, int x, int y)
        {
            var res = new List<(Direction direction, int row, int col)>();
            var perpDirection = dir.Crossing();
            var oppDirection = perpDirection.Opposite();

            // Start position for perpDirection
            var perpVector = perpDirection.GetNeighbourVector();
            res.Add((oppDirection, x + perpVector.x, y + perpVector.y));

            var oppVector = oppDirection.GetNeighbourVector();
            res.Add((perpDirection, x + oppVector.x, y + perpVector.y));

            return res;
        }

        public static (int y, int x) GetNeighbourVector(this Direction dir) =>
            dir switch
            {
                Direction.UpLeft => (-1, -1),
                Direction.Up => (-1, 0),
                Direction.UpRight => (-1, +1),
                Direction.Left => (0, -1),
                Direction.Right => (0, 1),
                Direction.DownLeft => (1, -1),
                Direction.Down => (1, 0),
                Direction.DownRight => (1, 1),
                _ => (0, 0),
            };
    }

    public enum Direction
    {
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight,
    }
}
