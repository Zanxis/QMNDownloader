using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QMNDownloader.Helpers
{
    internal static partial class FileHelper
    {
        private static readonly string[] suffixes = ["B", "KB", "MB", "GB", "TB"];
        internal static readonly char[] separator = ['/'];

        internal static string GetSafeDirectoryName(string url)
        {
            return string.Join("_", GetLastSegmentFromUrl(url).Split(Path.GetInvalidFileNameChars()));
        }

        static string GetLastSegmentFromUrl(string url)
        {
            Uri uri = new(url);
            string[] segments = uri.AbsolutePath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return segments.Length > 0 ? segments[^1] : string.Empty;
        }

        internal static string GenerateHash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        internal static string FormatFileSize(long bytes)
        {
            double size = bytes;
            int suffixIndex = 0;

            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                size /= 1024;
                suffixIndex++;
            }

            return $"{size:0.##} {suffixes[suffixIndex]}";
        }
    }
}
