using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace Premaker
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PremakerPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    public sealed class PremakerPackage : AsyncPackage
    {
        /// <summary>
        /// PremakerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "5fca1048-f29c-4275-8ae0-f63e174fcf7d";

        /// <summary>
        /// Options that the user can control
        /// </summary>
     


        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            Terminal.Install(this);
            await PremakeCommand.InitializeAsync(this);
            await TerminalCommand.InitializeAsync(this);
            await Commands.Configure.InitializeAsync(this);
            await Commands.version.ListReleases.InitializeAsync(this);
            await Commands.version.ListInstalled.InitializeAsync(this);
            await Commands.version.Set.InitializeAsync(this);
            await Commands.config.ConfigVersion.InitializeAsync(this);
            await Commands.config.ConfigView.InitializeAsync(this);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Terminal.Uninstall();
            }

            // Always call base
            base.Dispose(disposing);
        }
        #endregion
    }

    ///TODO: Add save and load for options
}
