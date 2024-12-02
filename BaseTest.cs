using System.Diagnostics;

namespace AdventOfCode24
{
    public class BaseTest
    {
        public void TimedTest(Action action)
        {
            var watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedTicks} ticks");
        }
    }
}
