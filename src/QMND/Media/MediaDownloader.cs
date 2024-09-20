using QMNDownloader.Helpers;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace QMNDownloader.Media
{
    internal static class MediaDownloader
    {
        internal static async Task<(string filePath, long fileSize)> DownloadMediaAsync(HttpClient httpClient, string mediaUrl, string imageDirectory, string videoDirectory)
        {
            string mediaExtension = Path.GetExtension(mediaUrl);
            string mediaHash = FileHelper.GenerateHash(mediaUrl);
            string mediaFileName = $"{mediaHash}{mediaExtension}";
            string mediaFilePath;

            if (mediaUrl.EndsWith(".jpg") || mediaUrl.EndsWith(".png"))
            {
                mediaFilePath = Path.Combine(imageDirectory, mediaFileName);
            }
            else if (mediaUrl.EndsWith(".mp4"))
            {
                mediaFilePath = Path.Combine(videoDirectory, mediaFileName);
            }
            else
            {
                mediaFilePath = Path.Combine(imageDirectory, mediaFileName); // Default case for other media
            }

            byte[] mediaData = await httpClient.GetByteArrayAsync(mediaUrl);
            await File.WriteAllBytesAsync(mediaFilePath, mediaData);
            long fileSize = mediaData.Length;
            Console.WriteLine($"Media saved: {mediaFilePath} (Size: {fileSize} bytes)");

            return (mediaFilePath, fileSize);
        }
    }
}
