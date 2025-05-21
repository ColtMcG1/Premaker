using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Premaker
{
    public class PremakePackage
    {
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; } = 0;
        public string Path { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    };

    public static class PremakeManager
    {

        public static async Task CreatePackage(string path, string name, int version, string downloadUrl, IProgress<double>? progress = null)
        {

            //Download the file from the URL
            using var stream = await GitInterface.DownloadFileAsync(downloadUrl, progress);

            // Extract the executable as a byte array (assuming the method returns byte[] or null)
            string exePath = GitInterface.ExtractExecutableFromZipAsync(stream, path, name) ?? string.Empty;

            //Create the package
            var package = new PremakePackage
            {
                Name = name.ToLower(),
                Version = version,
                Path = exePath,
                DownloadUrl = downloadUrl
            };

            // Save the package to the app data location
            var packages = await LoadPackagesAsync(path);
            packages.Add(package);
            await SavePackagesAsync(path, packages);
        }

        public static async Task RemovePackage(string path, string name)
        {
            var packages = await LoadPackagesAsync(path); // Load the existing packages
            var package = packages.FirstOrDefault(p => p.Name == name); // Find the package to remove
            if (package != null)
            {
                if (File.Exists(package.Path))
                { 
                    File.Delete(package.Path); // Delete the package file if it exists
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Package not found",
                        Content = "The package file was not found. It may have been deleted manually.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                }

                packages.Remove(package);
                await SavePackagesAsync(path, packages);
            }
        }

        public static async Task<List<PremakePackage>> LoadPackagesAsync(string path)
        {
            path = Path.Combine(path, "packages.json");
            if (!File.Exists(path))
                return new List<PremakePackage>();

            using var stream = File.OpenRead(path);
            var list = await JsonSerializer.DeserializeAsync<List<PremakePackage>>(stream)
                       ?? new List<PremakePackage>();
            return list;
        }

        public static async Task SavePackagesAsync(string path, IEnumerable<PremakePackage> packages)
        {
            path = Path.Combine(path, "packages.json");
            using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, packages, new JsonSerializerOptions { WriteIndented = true });
        }
    };

}
