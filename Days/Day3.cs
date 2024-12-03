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
    public partial class Day3 : BaseTest
    {
        private List<string> lines = new List<string>();
        private bool isEnabled = true;

        [OneTimeSetUp]
        public void Setup() => this.lines = LoadFile.LoadInput(3, 1).ToList();

        [Test]
        public void Part1() =>
            TimedTest(() => Console.WriteLine(this.lines.Sum(x => this.MultiplyRow(x))));

        [Test]
        public void Part1_SingleRegex() =>
            TimedTest(() => Console.WriteLine(this.lines.Sum(x => this.MultiplyRow_SingleRegex(x))));

        [Test]
        public void Part2() =>
            TimedTest(() => Console.WriteLine(this.lines.Sum(x => this.MultiplyRow_Part2(x))));

        private int MultiplyRow(string row)
        {
            var res = 0;

            foreach (Match m in MultRegex().Matches(row))
            {
                var nums = NumbersRegex().Matches(m.Value);
                var num1 = int.Parse(nums[0].Value);
                var num2 = int.Parse(nums[1].Value);

                res += (num1 * num2);
            }

            return res;
        }

        private int MultiplyRow_SingleRegex(string row)
        {
            var res = 0;

            foreach (Match m in AllInOneRegex().Matches(row))
            {
                var num1 = int.Parse(m.Groups["num1"].Value);
                var num2 = int.Parse(m.Groups["num2"].Value);

                res += (num1 * num2);
            }

            return res;
        }

        private int MultiplyRow_Part2(string row)
        {
            var res = 0;

            foreach (Match m in Part2Regex().Matches(row))
            {
                if (m.Groups["dont"].Success)
                {
                    this.isEnabled = false;
                }
                else if (m.Groups["do"].Success)
                {
                    this.isEnabled = true;
                }
                else if (this.isEnabled)
                {
                    var num1 = int.Parse(m.Groups["num1"].Value);
                    var num2 = int.Parse(m.Groups["num2"].Value);

                    res += (num1 * num2);
                }
            }

            return res;
        }

        [GeneratedRegex(@"mul\((?<num1>[0-9]{1,3}),(?<num2>[0-9]{1,3})\)")]
        private static partial Regex AllInOneRegex();

        [GeneratedRegex(@"[0-9]{1,3}")]
        private static partial Regex NumbersRegex();

        [GeneratedRegex(@"mul\([0-9]{1,3},[0-9]{1,3}\)")]
        private static partial Regex MultRegex();

        [GeneratedRegex(@"(?<mul>mul\((?<num1>[0-9]{1,3})),(?<num2>[0-9]{1,3})\)|(?<dont>don't\(\))|(?<do>do\(\))")]
        private static partial Regex Part2Regex();
    }
}
