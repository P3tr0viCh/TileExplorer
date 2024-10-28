using P3tr0viCh.Utils;
using System;
using System.IO;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static string AssemblyNameAndVersion()
        {
            var assemblyDecorator = new Misc.AssemblyDecorator();

            return $"{assemblyDecorator.Assembly.GetName().Name}/{assemblyDecorator.VersionString()}";
        }

        public static void DirectoryCreate(string path)
        {
            DebugWrite.Line($"{path}");

            Directory.CreateDirectory(path);
        }

        public static void FileCopy(string sourceFileName, string destFileName)
        {
            DebugWrite.Line($"{sourceFileName} > {destFileName}");

            File.Copy(sourceFileName, destFileName, true);
        }

        public static double LinearInterpolate(double x, double x1, double y1, double x2, double y2)
        {
            if (x1 == x2 && y1 == y2)
            {
                return y1;
            }

            var a = (y2 - y1) / (x2 - x1);

            var b = y1 - a * x1;

            return a * x + b;
        }

        public static bool DoubleEquals(double x, double y, double tolerance)
        {
            var diff = Math.Abs(x - y);
            return diff <= tolerance ||
                   diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
        }

        public static double DoubleFloorToEpsilon(double value, double epsilon)
        {
            return Math.Floor(value / epsilon) * epsilon;
        }

        public static string GetMonthName(int month)
        {
            return new DateTime(1981, month, 1).ToString("MMMM");
        }

        public static string[] GetMonthNames()
        {
            var result = new string[12];
            
            for (var month = 0; month < 12; month++)
            {
                result[month] = GetMonthName(month + 1);
            }

            return result;
        }
    }
}