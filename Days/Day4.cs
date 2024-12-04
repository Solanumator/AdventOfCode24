using AdventOfCode24.Util;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
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

            });
        }

        // Checks every direction for XMAS and returns the number of directions that contain the full word
        private int CheckAllDirections(int row, int col) => ((Direction[])Enum.GetValues(typeof(Direction))).Sum(dir => this.CheckNext(dir, 1, row, col) ? 1 : 0);

        // Checks the next cell in a given direciton for the next letter, where current is the index of the next letter.
        private bool CheckNext(Direction direction, int current, int row, int col)
        {
            if (current == 4)
            {
                return true;
            }

            // Increment to next letter
            var next = current + 1;

            var nextPosition = this.GetRelativePosition(row, col, direction);

            if (nextPosition.valid && grid[nextPosition.row, nextPosition.col] == next)
            {
                return CheckNext(direction, next, nextPosition.row, nextPosition.col);
            }

            return false;
        }

        private (bool valid, int row, int col) GetRelativePosition(int row, int col, Direction direction)
        {
            var newCol = col;
            var newRow = row;
            switch (direction)
            {
                case Direction.UpLeft:
                    newCol = col - 1;
                    newRow = row - 1;
                    break;
                case Direction.Up:
                    newRow = row - 1;
                    break;
                case Direction.UpRight:
                    newRow = row - 1;
                    newCol = col + 1;
                    break;
                case Direction.Left:
                    newCol = col - 1;
                    break;
                case Direction.Right:
                    newCol = col + 1;
                    break;
                case Direction.DownLeft:
                    newCol = col - 1;
                    newRow = row + 1;
                    break;
                case Direction.Down:
                    newRow = row + 1;
                    break;
                case Direction.DownRight:
                    newRow = row + 1;
                    newCol = col + 1;
                    break;
                default:
                    return (false, 0, 0);
            }

            if (newCol < 0 || newCol >= grid.GetLength(1) || newRow < 0 || newRow >= grid.GetLength(0))
            {
                return (false, 0, 0);
            }

            return (true, newRow, newCol);
        }

        private enum Direction
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
}
