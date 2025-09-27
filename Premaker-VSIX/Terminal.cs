using EnvDTE;
using Microsoft.Build.Framework.XamlTypes;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Premaker
{
    internal static class Terminal
    {
        private static string premakeExecutableLocation = string.Empty;
        private static AsyncPackage _package;
        public static void Install(AsyncPackage package)
        {

            string tempFolder = Path.GetTempPath(); 
            string premakeExePath = Path.Combine(tempFolder, "premakemanager.exe"); // Write the premake binary to the file
            premakeExecutableLocation = premakeExePath;
            if (!File.Exists(premakeExePath))
            {
                File.WriteAllBytes(premakeExePath, Properties.Resources.premakemanager);
            }
            _package = package;

        }

        public static void Uninstall()
        {
            if (!string.IsNullOrEmpty(premakeExecutableLocation) && !File.Exists(premakeExecutableLocation))
            {
                File.Delete(premakeExecutableLocation);
            }
        }
        public static async Task Run(string arguments = "", bool pane = false)
        {
            try
            {
                if (pane)
                {
                    await RunToPaneAsync(premakeExecutableLocation, arguments, _package);
                }
                else
                {
                    await RunNoPaneAsync(premakeExecutableLocation,arguments,_package);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Premaker - Error", MessageBoxButtons.OK);
            }
        }

        private static async Task RunNoPaneAsync(string premakeExecutableLocation,string arguments, AsyncPackage package)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(_package.DisposalToken);

                IVsSolution solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
                solution.GetSolutionInfo(out string solutionDirectory, out _, out _);

                var start = new ProcessStartInfo()
                {
                    FileName = premakeExecutableLocation,
                    WorkingDirectory = solutionDirectory,
                    CreateNoWindow = false,
                    LoadUserProfile = true,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false,
                    Arguments = arguments,
                    UseShellExecute = false
                };

                var process = new System.Diagnostics.Process();
                process.StartInfo = start;
                process.Start();
                await Task.Run(() => process.WaitForExit());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Premaker - Error", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private static async Task RunToPaneAsync(string premakeExecutableLocation, string arguments, AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null) return;

            arguments.ThrowIfNullOrEmpty("arguments can not be empty when using output pane");

            var paneGuid = new Guid("519B77C0-1DCC-4561-AB6C-3181A1B75A6C");
            
            outputWindow.CreatePane(paneGuid, "premakemanager", 1, 0);
            outputWindow.GetPane(paneGuid, out IVsOutputWindowPane pane);
            pane.Activate();
            pane.Clear();

            IVsSolution solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
            solution.GetSolutionInfo(out string solutionDirectory, out _, out _);

            var proc = new System.Diagnostics.Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = premakeExecutableLocation;
            proc.StartInfo.WorkingDirectory = solutionDirectory;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            proc.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;


            proc.OutputDataReceived += (o, args) =>
            {
#pragma warning disable VSTHRD010
                if (args.Data != null)
                    _ = pane.OutputStringThreadSafe(args.Data + "\n");
#pragma warning restore VSTHRD010
            };

            proc.ErrorDataReceived += (o, args) =>
            {
#pragma warning disable VSTHRD010
                if (args.Data != null)
                    _ = pane.OutputStringThreadSafe(args.Data + "\n");
#pragma warning restore VSTHRD010
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            
            await Task.Run(() => proc.WaitForExit());
        }
    }
}
