using QMNDownloader.Constants;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QMNDownloader.Helpers
{
    internal static class HttpClientHelper
    {
        internal static HttpClient CreateHttpClient()
        {
            HttpClientHandler handler = new()
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxConnectionsPerServer = 10,
            };

            HttpClient httpClient = new(handler)
            {
                Timeout = TimeSpan.FromSeconds(30),
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("QMN", GeneralConstants.PROGRAM_VERSION.ToString()));
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

            return httpClient;
        }
    }
}
