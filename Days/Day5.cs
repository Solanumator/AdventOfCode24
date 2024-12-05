using AdventOfCode24.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode24.Days
{
    public class Day5 : BaseTest
    {
        public List<(int num1, int num2)> Rules = new List<(int num1, int num2)>();
        public List<List<int>> Lists = new List<List<int>>();

        [OneTimeSetUp]
        public void Setup()
        {
            var lines = LoadFile.LoadInput(5, 1).ToList();

            // Rules first
            var rawRules = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToList();
            foreach (var rule in rawRules)
            {
                var split = rule.Split('|');
                this.Rules.Add((int.Parse(split[0]), int.Parse(split[1])));
            }

            // Lists
            this.Lists = lines
                .SkipWhile(x => !string.IsNullOrWhiteSpace(x))
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => x.Select(c => int.Parse(c)).ToList())
                .ToList();
        }

        [Test]
        public void Part1_Linq()
        {
            var res = 0;

            foreach (List<int> test in this.Lists)
            {
                // Get rules where both numbers exist
                var rulesToApply = this.Rules.Where(x => test.Contains(x.num1) && test.Contains(x.num2)).ToList();

                // Check that the index of the first number is less than the index of the second, for each applicable rule
                var matches = rulesToApply.TrueForAll(rule => test.IndexOf(rule.num1) < test.IndexOf(rule.num2));

                if (matches)
                {
                    // Will truncate decimal to get middle
                    var middleNumber = test[(test.Count / 2)];

                    // Get middle and add
                    res += middleNumber;
                }
            }

            Console.WriteLine(res);
        }

        [Test]
        public void Part2_Linq()
        {
            var res = 0;

            foreach (List<int> test in this.Lists)
            {
                // Get rules where both numbers exist
                var rulesToApply = this.Rules.Where(x => test.IndexOf(x.num1) >= 0 && test.IndexOf(x.num2) >= 0).ToList();

                // Check that the index of the first number is less than the index of the second, for each applicable rule
                var matches = this.Check(rulesToApply, test);

                if (!matches)
                {
                    // Sort using custom IComparer
                    test.Sort(new Day5Sort(rulesToApply));

                    res += test[test.Count / 2];
                }
            }

            Console.WriteLine(res);
        }

        private bool Check(List<(int num1, int num2)> rulesToApply, List<int> test) =>
            rulesToApply.TrueForAll(rule => test.IndexOf(rule.num1) < test.IndexOf(rule.num2));
    }

    public class Day5Sort(List<(int num1, int num2)> rules) : IComparer<int>
    {
        public int Compare(int num1, int num2)
        {
            // Doesnt matter if this sorts backwards or forwards
            // as the middle number will be the same.
            if (rules.Contains((num1, num2))) return 1;
            if (rules.Contains((num2, num1))) return -1;
            return 0;
        }
    }
}
