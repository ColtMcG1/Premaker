using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Microsoft.UI;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Premaker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private static readonly string _appName = "Premaker";
        //private static readonly string _appVersion = "1.0.0";
        //private static readonly string _appAuthor = "Colton McGraw";
        //private static readonly string _appDescription = "A simple application to create premake files.";
        //private static readonly string _appLicense = "MIT License";
        //private static readonly string _appLicenseUrl = "https://opensource.org/licenses/MIT";
        private static readonly string _appDataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appName);

        private ObservableCollection<GitHubRelease> Releases = new();
        private ObservableCollection<PremakePackage> Packages = new();

        public MainWindow()
        {
            this.InitializeComponent();

            // Create the app data folder if it doesn't exist
            if (!Directory.Exists(_appDataLocation))
                Directory.CreateDirectory(_appDataLocation);

            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Set the icon (path is relative to the executable)
            appWindow.SetIcon("Assets\\favicon.ico");

            // Initalize the ItemSources for the ListViews
            ReleasesListView.ItemsSource = Releases;
            PackagesListView.ItemsSource = Packages;

            //Discard releases to silence compiler. We don't care about the result.
            _ = LoadGitReleasesAsync();  
            _ = LoadPackagesAsync();

        }

        /// <summary>
        /// Load the releases from the GitHub repository.
        /// </summary>
        /// <returns></returns>
        private async Task LoadGitReleasesAsync()
        {
            var releases = await GitInterface.GetGitHubReleasesAsync("premake", "premake-core");
            //releases.AddRange(await GitInterface.GetGitHubReleasesAsync("premake", "premake-4.x")); Does nothing since this repo has no releases
            //releases.AddRange(await GitInterface.GetGitHubReleasesAsync("premake", "premake-3.x")); Does nothing since this repo has no releases

            Releases.Clear();

            foreach (var release in releases)
            {
                if(Packages.Any(w => w.Name == release.Name))
                {
                    release.IsDownloaded = true;
                }

                Releases.Add(release);
            }
        }

        /// <summary>
        /// Load the packages from the app data location.
        /// </summary>
        /// <returns></returns>
        private async Task LoadPackagesAsync()
        {
            var loaded = await PremakeManager.LoadPackagesAsync(_appDataLocation);
            Packages.Clear();
            foreach (var pkg in loaded)
                Packages.Add(pkg);
        }

        /// <summary>
        /// Save the packages to the app data location.
        /// </summary>
        /// <returns></returns>
        private async Task SavePackagesAsync()
        {
            await PremakeManager.SavePackagesAsync(_appDataLocation, Packages);
        }

        /// <summary>
        /// Download the selected package and create a new package.
        /// </summary>
        /// <param name="url">URL of Git repo</param>
        /// <param name="path">Path to store data</param>
        /// <param name="name">Name of package</param>
        /// <param name="version">Package version</param>
        /// <param name="downloadUrl">URL of ZIP ball</param>
        /// <returns></returns>
        private async Task DownloadAndCreatePackageAsync(string path, string name, int version, string downloadUrl)
        {
            var progress = new Progress<double>(value =>
            {
                DownloadProgressBar.Value = value;
            });

            await PremakeManager.CreatePackage(path, name, version, downloadUrl, progress);
            DownloadProgressBar.Value = 0; // Reset after done
        }

        /// <summary>
        /// Event handler for the AddPackage button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddPackage_Click(object sender, RoutedEventArgs e)
        {
            //Get the selected item from the ReleasesListView
            if (ReleasesListView.SelectedItem is GitHubRelease selected)
            {
                // Check if the selected version is already in the packages list
                if (Packages.Any(w => w.Name == selected.Name))
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Package already exists",
                        Content = "A package with this name already exists. Please choose a different name.",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                    return;
                }

                try
                {
                    // Check if the selected release has any Windows assets
                    var windowsAsset = selected.Assets.FirstOrDefault(w => w.Name.Contains("windows"));
                    if (windowsAsset == null)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "No suitable asset found",
                            Content = "The selected release does not contain a downloadable asset for Windows.",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };
                        await dialog.ShowAsync();
                        return;
                    }

                    // Download the selected package and create a new package
                    await DownloadAndCreatePackageAsync(_appDataLocation, selected.Name, selected.Id, windowsAsset.BrowserDownloadUrl);

                    // Reload the packages list
                    await LoadPackagesAsync();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Unable to download selected package.",
                        Content = ex.Message,
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }

            _ = LoadGitReleasesAsync();
            _ = LoadPackagesAsync();
        }

        /// <summary>
        /// Event handler for the RemovePackage button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemovePackage_Click(object sender, RoutedEventArgs e)
        {
            if (PackagesListView.SelectedItem is PremakePackage selected)
            {
                Packages.Remove(selected);
                await SavePackagesAsync();
            }

            _ = LoadGitReleasesAsync();
            _ = LoadPackagesAsync();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //Discard releases to silence compiler. We don't care about the result.
            _ = LoadGitReleasesAsync();
            _ = LoadPackagesAsync();

            await Task.CompletedTask;
        }

    }
}
