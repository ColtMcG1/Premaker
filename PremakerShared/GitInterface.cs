using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Premaker
{
    public class GitHubRelease
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("assets_url")]
        public string AssetsUrl { get; set; } = string.Empty;

        [JsonPropertyName("upload_url")]
        public string UploadUrl { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonPropertyName("target_commitish")]
        public string TargetCommitish { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("draft")]
        public bool Draft { get; set; }

        [JsonPropertyName("prerelease")]
        public bool Prerelease { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime PublishedAt { get; set; }

        [JsonPropertyName("author")]
        public GitHubUser Author { get; set; } = new();

        [JsonPropertyName("assets")]
        public List<GitHubAsset> Assets { get; set; } = new();



        public bool IsDownloaded { get; set; } = false;
    }

    public class GitHubUser
    {
        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;
    }

    public class GitHubAsset
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("download_count")]
        public int DownloadCount { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; } = string.Empty;
    }


    public static class GitInterface
    {
        public static async Task<List<GitHubRelease>> GetGitHubReleasesAsync(string owner, string repo)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Premaker"); // GitHub requires a User-Agent

            var url = $"https://api.github.com/repos/{owner}/{repo}/releases";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tags = JsonSerializer.Deserialize<List<GitHubRelease>>(json);

            return tags ?? new List<GitHubRelease>();
        }
        public static async Task<Stream> DownloadFileAsync(string url, IProgress<double>? progress = null)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Premaker"); // GitHub requires a User-Agent
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var total = response.Content.Headers.ContentLength ?? -1L;
            var stream = await response.Content.ReadAsStreamAsync();
            var memStream = new MemoryStream();
            var buffer = new byte[81920];
            long read = 0;
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await memStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                read += bytesRead;
                if (total > 0 && progress != null)
                {
                    progress.Report((double)read / total);
                }
            }
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
        public static string? ExtractExecutableFromZipAsync(Stream zipStream, string outputDirectory, string executableName)
        {
            // Create dir
            string location = Path.Combine(outputDirectory, executableName);
            Directory.CreateDirectory(location);

            //Unzip and extract item(s). (Should only be ONE file, unless Premake changes there release file structure in the future)
            string entryPath = string.Empty;
            using (var innerZip = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (var entry in innerZip.Entries)
                {
                    entryPath = Path.Combine(location, entry.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(entryPath)!);
                    entry.ExtractToFile(entryPath, overwrite: true);
                }
            }

            //Return the path to the extracted file.
            return entryPath;
        }

    };
}