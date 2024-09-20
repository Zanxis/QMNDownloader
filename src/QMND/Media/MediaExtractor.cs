using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QMNDownloader.Media
{
    internal static class MediaExtractor
    {
        internal static async Task<string[]> ExtractMediaUrls(HttpClient httpClient, string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            _ = response.EnsureSuccessStatusCode();
            string pageContent = await response.Content.ReadAsStringAsync();

            HtmlDocument document = new();
            document.LoadHtml(pageContent);

            HtmlNode mediaContainer = document.DocumentNode.SelectSingleNode("//main[@class='content']");
            List<string> mediaUrls = [];

            if (mediaContainer != null)
            {
                HtmlNodeCollection mediaNodes = mediaContainer.SelectNodes(".//img | .//video/source | .//div[contains(@class, 'flowplayer')]");

                if (mediaNodes != null)
                {
                    int index = 0;
                    foreach (HtmlNode mediaNode in mediaNodes)
                    {
                        string mediaUrl = mediaNode.Name switch
                        {
                            "img" => mediaNode.GetAttributeValue("src", null),
                            "source" => mediaNode.GetAttributeValue("src", null),
                            "div" when mediaNode.HasClass("flowplayer") => ExtractVideoUrlFromJson(WebUtility.HtmlDecode(mediaNode.GetAttributeValue("data-item", null))),
                            _ => null
                        };

                        if (!string.IsNullOrEmpty(mediaUrl))
                        {
                            Console.WriteLine($"[ • ] Extracting ({index}): {mediaUrl}");
                            mediaUrls.Add(mediaUrl);
                            index++;
                        }
                    }
                }
            }

            return [.. mediaUrls];
        }

        private static string ExtractVideoUrlFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            try
            {
                json = WebUtility.HtmlDecode(json);
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                if (root.TryGetProperty("sources", out JsonElement sources) && sources.GetArrayLength() > 0)
                {
                    foreach (JsonElement source in sources.EnumerateArray())
                    {
                        if (source.TryGetProperty("src", out JsonElement src) && source.TryGetProperty("type", out JsonElement type) && type.GetString() == "video/mp4")
                        {
                            return src.GetString();
                        }
                    }
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error parsing JSON: {e.Message}");
            }

            return null;
        }
    }
}
