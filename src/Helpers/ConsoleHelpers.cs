using System;

namespace QMNDownloader.Helpers
{
    internal static class ConsoleHelpers
    {
        internal static void WriteLineColor(string value, ConsoleColor color)
        {
            SetForegroundColor(color);
            Console.WriteLine(value);
            Console.ResetColor();
        }

        internal static void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
