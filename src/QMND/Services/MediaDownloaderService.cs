using QMNDownloader.Helpers;
using QMNDownloader.Media;

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QMNDownloader.Services
{
    internal static class MediaDownloaderService
    {
        public static async Task<StringBuilder> DownloadMediaFilesAsync(HttpClient httpClient, string url, string[] mediaUrls, (string blogDirectory, string imageDirectory, string videoDirectory) directories)
        {
            StringBuilder summary = new();
            BuildSummaryHeader(summary, url, directories, mediaUrls.Length);

            int imageCount = 0, videoCount = 0, otherCount = 0;
            long totalSize = 0;

            foreach (string mediaUrl in mediaUrls)
            {
                (string filePath, long fileSize) = await MediaDownloader.DownloadMediaAsync(httpClient, mediaUrl, directories.imageDirectory, directories.videoDirectory);
                AppendMediaInfo(summary, mediaUrl, filePath, fileSize);

                totalSize += fileSize;
                CategorizeMedia(mediaUrl, ref imageCount, ref videoCount, ref otherCount);

                ConsoleHelpers.WriteLineColor($"[ ✓ ] File `{Path.GetFileName(filePath)}` downloaded successfully.", ConsoleColor.Green);
            }

            AppendSummaryStats(summary, imageCount, videoCount, otherCount, totalSize);
            return summary;
        }

        private static void BuildSummaryHeader(StringBuilder summary, string url, (string blogDirectory, string imageDirectory, string videoDirectory) directories, int mediaCount)
        {
            _ = summary.AppendLine("# Download Summary")
                   .AppendLine($"**URL**: `{url}`")
                   .AppendLine($"**Download Directory**: `{directories.blogDirectory}`")
                   .AppendLine($"## Downloaded Files ({mediaCount})")
                   .AppendLine();
        }

        private static void AppendMediaInfo(StringBuilder summary, string mediaUrl, string filePath, long fileSize)
        {
            string fileName = Path.GetFileName(filePath);
            _ = summary.AppendLine($"### {fileName} ({FileHelper.FormatFileSize(fileSize)})")
                   .AppendLine($"- **URL**: [{mediaUrl}]({mediaUrl})")
                   .AppendLine($"- **Path**: [{filePath}]({filePath})")
                   .AppendLine();
        }

        private static void CategorizeMedia(string mediaUrl, ref int imageCount, ref int videoCount, ref int otherCount)
        {
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
        }

        private static void AppendSummaryStats(StringBuilder summary, int imageCount, int videoCount, int otherCount, long totalSize)
        {
            _ = summary.AppendLine("## Statistics")
                   .AppendLine($"- **Images**: {imageCount}")
                   .AppendLine($"- **Videos**: {videoCount}")
                   .AppendLine($"- **Others**: {otherCount}")
                   .AppendLine($"- **Total Size**: {FileHelper.FormatFileSize(totalSize)}");
        }
    }
}
