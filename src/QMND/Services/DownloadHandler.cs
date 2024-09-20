using QMNDownloader.Helpers;
using QMNDownloader.Media;

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QMNDownloader.Services
{
    internal static class DownloadHandler
    {
        public static async Task HandleDownloadAsync(string blogUrl)
        {
            Stopwatch runtimeStopwatch = new();
            runtimeStopwatch.Start();

            try
            {
                Console.WriteLine("Initializing tool...");
                (string blogDirectory, string imageDirectory, string videoDirectory) directories = DirectoryService.SetupDirectories(blogUrl);

                using HttpClient httpClient = HttpClientHelper.CreateHttpClient();
                ConsoleHelpers.WriteLineColor(" [done]", ConsoleColor.Green);

                Console.WriteLine("Extracting media URLs for download...");
                string[] mediaUrls = await MediaExtractor.ExtractMediaUrls(httpClient, blogUrl);
                ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);

                if (mediaUrls.Length > 0)
                {
                    Console.WriteLine("Downloading media files...");
                    StringBuilder summary = await MediaDownloaderService.DownloadMediaFilesAsync(httpClient, blogUrl, mediaUrls, directories);
                    ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);

                    await DirectoryService.SaveSummaryAsync(summary, directories.blogDirectory);
                }
                else
                {
                    Console.WriteLine("No media found.");
                }

                DisplayStats(runtimeStopwatch, blogUrl, directories.blogDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                runtimeStopwatch.Stop();
            }
        }

        private static void DisplayStats(Stopwatch runtimeStopwatch, string blogUrl, string blogDirectory)
        {
            Console.WriteLine(new string('=', 32));
            ConsoleHelpers.WriteLineColor("All operations completed successfully!", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("[ STATS ]");
            Console.WriteLine($"[ • ] Runtime: {runtimeStopwatch.Elapsed.TotalSeconds}s");
            Console.WriteLine($"[ • ] URL: {blogUrl}");
            Console.WriteLine($"[ • ] Output Directory: {blogDirectory}");
            Console.WriteLine(new string('=', 32));
        }
    }
}
