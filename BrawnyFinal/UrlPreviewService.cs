using System.Text.RegularExpressions;
using BrawnyFinal.Models;

namespace BrawnyFinal;

public class UrlPreviewService
{
    private readonly HttpClient _client;

    public UrlPreviewService(HttpClient client)
    {
        _client = client;
    }

    public async Task<UrlPreviewModel?> GetPreviewAsync(string url)
    {
        try
        {
            string? html;

            try
            {
                html = await _client.GetStringAsync(url);
            }
            catch
            {
            }

            try
            {
                html = await _client.GetStringAsync($"https://corsproxy.io/?url={url}");
            }
            catch (Exception ex)
            {
                Console.Error.Write(ex.Message);
                return null;
            }

            return ParseHtml(html, url);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return null;
        }
    }

    private UrlPreviewModel ParseHtml(string html, string url)
    {
        string? GetMetaTagContent(string pattern)
        {
            var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }
        
        return new UrlPreviewModel
        {
            Title = GetMetaTagContent("<meta property=\"og:title\" content=\"(.*?)\"") ?? 
                    GetMetaTagContent("<title>(.*?)</title>") ?? "No Title",
            Description = GetMetaTagContent("<meta property=\"og:description\" content=\"(.*?)\"") ?? 
                          GetMetaTagContent("<meta name=\"description\" content=\"(.*?)\"") ?? "No Description",
            ImageUrl = GetMetaTagContent("<meta property=\"og:image\" content=\"(.*?)\""),
            Url = url
        };
    }
}