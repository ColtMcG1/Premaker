using CommandLine;
using Premaker;
using System.Diagnostics;

class PremakeCLI
{
    public class Options
    {
        [Option('v', "version", Required = false, HelpText = "Package version to use.")]
        public string Version { get; set; } = string.Empty;

        [Option('p', "package", Required = false, HelpText = "Package name to use.")]
        public string PackageName { get; set; } = string.Empty;

        [Option('d', "download", Required = false, HelpText = "Download the package.")]
        public bool Download { get; set; }

        [Option('r', "remove", Required = false, HelpText = "Remove the package.")]
        public bool Remove { get; set; }

        [Option('l', "list", Required = false, HelpText = "List all packages.")]
        public string List { get; set; } = string.Empty;

        [Value(0, HelpText = "Arguments to pass to the program.")]
        public IEnumerable<string> ExtraArgs { get; set; } = Enumerable.Empty<string>();
    }

    static string appDataLocation = Environment.GetEnvironmentVariable("APPDATA") ?? string.Empty;
    static string packagesFilePath = Path.Combine(appDataLocation, "Premaker");

    static async Task<List<GitHubRelease>> GetReleasesAsync() =>
        await GitInterface.GetGitHubReleasesAsync("premake", "premake-core");

    static async Task<List<PremakePackage>> GetPackagesAsync() =>
        await PremakeManager.LoadPackagesAsync(packagesFilePath);

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
               .WithParsed<Options>(opts => RunOptions(opts).Wait())
               .WithNotParsed<Options>(HandleParseError);

    }

    static async Task RunOptions(Options opts)
    {

        if (opts.Download)
        {
            await DownloadPackage(opts);
        }
        else if (opts.Remove)
        {
            var packages = await GetPackagesAsync();
            var package = packages.FirstOrDefault(p => p.Name == opts.PackageName);
            if (package != null)
            {
                await PremakeManager.RemovePackage(packagesFilePath, opts.PackageName);
                Console.WriteLine($"Removed {opts.PackageName}.");
            }
            else
            {
                Console.WriteLine($"Package {opts.PackageName} not found.");
            }
        }
        else if (opts.List.Length > 0)
        {
            await ListItems(opts);
        }
        else
        {
            var packages = await GetPackagesAsync();
            var package = packages.FirstOrDefault(p => p.Name == opts.PackageName || p.Version.ToString() == opts.Version.ToString()) ?? packages.MaxBy(p => p.Version); // Get the latest version if no specific version or name is provided
            var extraArgs = opts.ExtraArgs != null ? string.Join(" ", opts.ExtraArgs) : ""; // Join the extra arguments into a single string
            if (string.IsNullOrEmpty(package?.Path)) // Check if the package path is empty
            {
                Console.WriteLine($"Package {opts.PackageName} not found.");
            }
            else
            {
                Console.WriteLine($"Running {package.Name}\nVersion {package.Version}");
                Run(package?.Path ?? string.Empty, extraArgs);
            }
        }
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
        foreach (var error in errs)
        {
            Console.WriteLine(error.ToString());
        }
    }

    static async Task DownloadPackage(Options opts)
    {
        var releases = await GetReleasesAsync();
        var release = releases.FirstOrDefault(r => r.Name == opts.PackageName || r.Id.ToString() == opts.Version.ToString());
        if (release != null)
        {
            if ((await GetPackagesAsync()).Any(w => w.Name.ToLower() == release.Name.ToLower()))
            {
                Console.WriteLine($"Package {opts.PackageName} already exists. Please choose a different name.");
                return;
            }

            var windowsAsset = release.Assets.FirstOrDefault(w => w.Name.Contains("windows"));
            if (windowsAsset == null)
            {
                Console.WriteLine($"The selected release does not contain a downloadable asset for Windows.");
                return;
            }

            var progress = new Progress<double>(p =>
            {
                int percent = (int)(p * 100);
                Console.Write($"\rDownloading {release.Name}: {percent}%");

            });

            await PremakeManager.CreatePackage(packagesFilePath, release.Name, release.Id, windowsAsset.BrowserDownloadUrl, progress);
            Console.WriteLine("\nDownloaded {0}.", release.Name);
        }
        else
        {
            Console.WriteLine($"Release {opts.Version ?? opts.PackageName} not found.");
        }
    }


    static async Task ListItems(Options opts)
    {
        if (opts.List == "releases")
        {
            var releases = await GetReleasesAsync();

            // Find the max length for name and id for padding
            int nameWidth = releases.Any() ? releases.Max(r => r.Name.Length) : 10;
            int idWidth = releases.Any() ? releases.Max(r => r.Id.ToString().Length) : 5;

            // Header
            Console.WriteLine($"{"Name".PadRight(nameWidth)}   {"Id".PadRight(idWidth)}");
            Console.WriteLine(new string('-', nameWidth + idWidth + 3));

            foreach (var release in releases)
            {
                Console.WriteLine($"{release.Name.PadRight(nameWidth)}   {release.Id.ToString().PadRight(idWidth)}");
            }
        }
        else if (opts.List == "packages")
        {
            var packages = await GetPackagesAsync();

            int nameWidth = packages.Any() ? packages.Max(p => p.Name.Length) : 10;
            int versionWidth = packages.Any() ? packages.Max(p => p.Version.ToString().Length) : 7;

            // Header
            Console.WriteLine($"{"Name".PadRight(nameWidth)}   {"Version".PadRight(versionWidth)}");
            Console.WriteLine(new string('-', nameWidth + versionWidth + 3));

            foreach (var package in packages)
            {
                Console.WriteLine($"{package.Name.PadRight(nameWidth)}   {package.Version.ToString().PadRight(versionWidth)}");
            }
        }
        else
        {
            Console.WriteLine("Invalid list option. Use 'releases' or 'packages'.");
        }
    }

    static void Run(string exePath, string arguments = "")
    {
        Console.WriteLine("--------------------------------");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine(output);
        if (!string.IsNullOrEmpty(error))
            Console.WriteLine("Error: " + error);
    }
}
