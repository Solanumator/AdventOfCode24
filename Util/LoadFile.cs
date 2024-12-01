namespace AdventOfCode24.Util
{
    public static class LoadFile
    {
        public static string[] LoadInput(int day, int part) => File.ReadAllLines($"Files/{day}_{part}.txt");
    }
}
