using QMNDownloader.Helpers;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QMNDownloader.Services
{
    internal static class DirectoryService
    {
        public static (string blogDirectory, string imageDirectory, string videoDirectory) SetupDirectories(string blogUrl)
        {
            Console.WriteLine("Creating default directories...");

            string baseDirectory = Directory.GetCurrentDirectory();
            string blogDirectory = Path.Combine(baseDirectory, "downloads", FileHelper.GetSafeDirectoryName(blogUrl));
            string imageDirectory = Path.Combine(blogDirectory, "images");
            string videoDirectory = Path.Combine(blogDirectory, "videos");

            _ = Directory.CreateDirectory(blogDirectory);
            _ = Directory.CreateDirectory(imageDirectory);
            _ = Directory.CreateDirectory(videoDirectory);

            ConsoleHelpers.WriteLineColor($"[ • ] Directory `{blogDirectory}` created successfully.", ConsoleColor.Cyan);
            ConsoleHelpers.WriteLineColor($"[ • ] Directory `{imageDirectory}` created successfully.", ConsoleColor.Cyan);
            ConsoleHelpers.WriteLineColor($"[ • ] Directory `{videoDirectory}` created successfully.", ConsoleColor.Cyan);

            return (blogDirectory, imageDirectory, videoDirectory);
        }

        public static async Task SaveSummaryAsync(StringBuilder summary, string blogDirectory)
        {
            string summaryFilePath = Path.Combine(blogDirectory, "summary.md");
            await File.WriteAllTextAsync(summaryFilePath, summary.ToString());
            Console.WriteLine($"Summary saved to: {summaryFilePath}");
        }
    }
}
