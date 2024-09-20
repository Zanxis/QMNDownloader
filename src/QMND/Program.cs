using Figgle;

using QMNDownloader.Constants;
using QMNDownloader.Helpers;
using QMNDownloader.Media;

using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QMNDownloader
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            SetupConsole();

            Console.WriteLine(FiggleFonts.Standard.Render(GeneralConstants.PROGRAM_NAME));
            Console.WriteLine($"{GeneralConstants.PROGRAM_NAME} {GeneralConstants.PROGRAM_VERSION} - (c) {GeneralConstants.PROGRAM_AUTHOR}");
            Console.WriteLine(new string('-', 32));

            RootCommand rootCommand = CreateRootCommand();

            return await rootCommand.InvokeAsync(args);
        }

        private static void SetupConsole()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
        }

        private static RootCommand CreateRootCommand()
        {
            Option<string> blogUrlOption = new("--url", "The URL of the respective website blog page that will be downloaded (full URL).")
            {
                IsRequired = true,
            };

            RootCommand rootCommand = new(GeneralConstants.PROGRAM_DESCRIPTION);
            rootCommand.AddOption(blogUrlOption);
            rootCommand.SetHandler(HandleDownloadAsync, blogUrlOption);

            return rootCommand;
        }

        private static async Task HandleDownloadAsync(string blogUrl)
        {
            Stopwatch runtimeStopwatch = new();
            runtimeStopwatch.Start();

            try
            {
                Console.WriteLine("Initializing tool...");

                (string blogDirectory, string imageDirectory, string videoDirectory) directories = SetupDirectories(blogUrl);

                Console.Write("Creating HttpClient for communication...");
                using HttpClient httpClient = HttpClientHelper.CreateHttpClient();
                ConsoleHelpers.WriteLineColor(" [done]", ConsoleColor.Green);

                Console.WriteLine("Extracting media URLs for download...");
                string[] mediaUrls = await MediaExtractor.ExtractMediaUrls(httpClient, blogUrl);
                ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);

                if (mediaUrls != null && mediaUrls.Length > 0)
                {
                    Console.WriteLine("Downloading extracted media files...");
                    StringBuilder summary = await DownloadMediaFilesAsync(httpClient, blogUrl, mediaUrls, directories);
                    ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);

                    Console.WriteLine("Creating and saving summary markdown file in output directory...");
                    await SaveSummaryAsync(summary, directories.blogDirectory);
                    ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);
                }
                else
                {
                    Console.WriteLine("No media found.");
                    return;
                }

                runtimeStopwatch.Stop();

                Console.WriteLine(new string('=', 32));
                ConsoleHelpers.WriteLineColor("All operations completed successfully!", ConsoleColor.Yellow);
                Console.WriteLine();
                Console.WriteLine("[ STATS ]");
                Console.WriteLine();
                Console.WriteLine($"[ • ] Runtime: {runtimeStopwatch.Elapsed.TotalSeconds}s");
                Console.WriteLine($"[ • ] URL: {blogUrl}");
                Console.WriteLine($"[ • ] Output Directory: {directories.blogDirectory}");
                Console.WriteLine();
                Console.WriteLine(new string('=', 32));
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

        private static (string blogDirectory, string imageDirectory, string videoDirectory) SetupDirectories(string blogUrl)
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
            ConsoleHelpers.WriteLineColor("[done]", ConsoleColor.Green);

            return (blogDirectory, imageDirectory, videoDirectory);
        }

        private static async Task<StringBuilder> DownloadMediaFilesAsync(HttpClient httpClient, string url, string[] mediaUrls, (string blogDirectory, string imageDirectory, string videoDirectory) directories)
        {
            DateTime startTime = DateTime.Now;
            StringBuilder summary = new();
            _ = summary.AppendLine("# Download Summary");
            _ = summary.AppendLine();
            _ = summary.AppendLine("## General Information");
            _ = summary.AppendLine();
            _ = summary.AppendLine($"**URL**: `{url}`");
            _ = summary.AppendLine($"**Download Directory**: `{directories.blogDirectory}`");
            _ = summary.AppendLine($"**Download Start Time**: `{startTime}`");
            _ = summary.AppendLine();
            _ = summary.AppendLine($"## Downloaded Files ({mediaUrls.Length})");
            _ = summary.AppendLine();

            int imageCount = 0, videoCount = 0, otherCount = 0;
            long totalSize = 0;

            foreach (string mediaUrl in mediaUrls)
            {
                (string filePath, long fileSize) = await MediaDownloader.DownloadMediaAsync(httpClient, mediaUrl, directories.imageDirectory, directories.videoDirectory);
                string fileName = Path.GetFileName(filePath);

                _ = summary.AppendLine($"### {fileName} ({FileHelper.FormatFileSize(fileSize)})");
                _ = summary.AppendLine();
                _ = summary.AppendLine($"- **URL**: [{mediaUrl}]({mediaUrl});");
                _ = summary.AppendLine($"- **Path:** [{filePath}]({filePath}).");
                _ = summary.AppendLine();

                totalSize += fileSize;
                if (mediaUrl.EndsWith(".jpg") || mediaUrl.EndsWith(".png"))
                {
                    imageCount++;
                }
                else if (mediaUrl.EndsWith(".mp4"))
                {
                    videoCount++;
                }
                else
                {
                    otherCount++;
                }

                ConsoleHelpers.WriteLineColor($"[ ✓ ] File `{fileName}` has been successfully downloaded and saved to `{filePath}`...", ConsoleColor.Green);
            }

            DateTime endTime = DateTime.Now;
            _ = summary.AppendLine($"**Download End Time**: `{endTime}`");
            _ = summary.AppendLine($"**Total Time**: `{(endTime - startTime).TotalMinutes} minutes`");
            _ = summary.AppendLine();
            _ = summary.AppendLine("## Statistics");
            _ = summary.AppendLine();
            _ = summary.AppendLine($"- **Images**: {imageCount}");
            _ = summary.AppendLine($"- **Videos**: {videoCount}");
            _ = summary.AppendLine($"- **Others**: {otherCount}");
            _ = summary.AppendLine($"- **Total Size**: {FileHelper.FormatFileSize(totalSize)}");

            return summary;
        }

        private static async Task SaveSummaryAsync(StringBuilder summary, string blogDirectory)
        {
            string summaryFilePath = Path.Combine(blogDirectory, "summary.md");
            await File.WriteAllTextAsync(summaryFilePath, summary.ToString());
            Console.WriteLine($"Summary saved to: {summaryFilePath}");
        }
    }
}
