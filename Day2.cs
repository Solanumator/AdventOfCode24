using AdventOfCode24.Util;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode24
{
    [TestFixture]
    public class Day2 : BaseTest
    {
        private List<List<int>> lines = new List<List<int>>();

        [OneTimeSetUp]
        public void Setup()
        {
            var input = LoadFile.LoadInput(2, 1);
            foreach (var item in input)
            {
                lines.Add(item.Split(' ').Select(x => int.Parse(x)).ToList());
            }
        }

        [Test]
        public void Part1()
        {
            this.TimedTest(() =>
            {
                var safeReports = 0;

                foreach (var l in this.lines)
                {
                    safeReports += this.IsSafe(l) ? 1 : 0;
                }

                Console.WriteLine(safeReports);
            });
        }

        [Test]
        public void Part2()
        {
            // Expecting 343
            this.TimedTest(() =>
            {
                var safeReports = 0;

                foreach (var l in this.lines)
                {
                    var isSafe = this.IsSafeBruteForce(l);
                    safeReports += isSafe ? 1 : 0;
                }

                Console.WriteLine(safeReports);
            });
        }

        public bool IsSafeBruteForce(List<int> rowValues)
        {
            for (int i = 0; i < rowValues.Count; i++)
            {
                var listToTest = new List<int>(rowValues);
                listToTest.RemoveAt(i);
                if (this.IsSafe(listToTest))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSafe(List<int> rowValues)
        {
            var isAsc = true;
            var isDesc = true;

            for (int i = 1; i < rowValues.Count; i++)
            {
                // Ensure gaps are between 1 and 3
                var diff = Math.Abs(rowValues[i] - rowValues[i - 1]);
                if (diff < 1 || diff > 3)
                {
                    return false;
                }

                // Ensure ascending or descending
                if (rowValues[i] < rowValues[i - 1])
                {
                    isAsc = false;
                }
                else if (rowValues[i] > rowValues[i - 1])
                {
                    isDesc = false;
                }
                else
                {
                    // Return early if duplicates exist
                    return false;
                }

                // We can return early if both are false
                if (!isAsc && !isDesc)
                {
                    return false;
                }
            }

            return isAsc || isDesc;
        }
    }
}
