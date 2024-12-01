using AdventOfCode24.Util;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace AdventOfCode24
{
    public class Tests : BaseTest
    {
        private List<int> left = new List<int>();
        private List<int> right = new List<int>();

        [OneTimeSetUp]
        public void Setup()
        {
            var input = LoadFile.LoadInput(1, 1);

            foreach (var line in input)
            {
                // Split by one or more spaces
                string[] res = Regex.Split(line, @"\s+");
                this.left.Add(int.Parse(res[0]));
                this.right.Add(int.Parse(res[1]));

                // Order the lists
                this.left.Sort();
                this.right.Sort();
            }
        }

        [Test]
        public void Part1()
        {
            this.TimedTest(() =>
            {
                var result = 0;

                // Process elements to get result
                for (int i = 0; i < left.Count; i++)
                {
                    result += Math.Abs(left[i] - right[i]);
                }

                Console.WriteLine(result);
            });
        }

        [Test]
        public void Part2()
        {
            this.TimedTest(() =>
            {
                var rightCounts = this.right.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

                var total = this.left.Sum(val => val * (rightCounts.ContainsKey(val) ? rightCounts[val] : 0));
                Console.WriteLine(total);
            });
        }

        [Test]
        public void Part2NoLinq()
        {
            this.TimedTest(() =>
            {
                var total = 0;
                var rightCounts = new Dictionary<int, int>();

                foreach (var val in right)
                {
                    if (rightCounts.ContainsKey(val))
                    {
                        rightCounts[val]++;
                    }
                    else
                    {
                        rightCounts[val] = 1;
                    }
                }


                // Calculate total based on counts
                foreach (var val in left)
                {
                    if (rightCounts.ContainsKey(val))
                    {
                        total += val * rightCounts[val];
                    }
                }

                Console.WriteLine(total);
            });
        }
    }
}