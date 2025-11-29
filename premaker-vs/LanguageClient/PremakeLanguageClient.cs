using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using System.Composition;

namespace Premaker.VSIX.LanguageClient
{
    [Export(typeof(ILanguageClient))]
    [ContentType("lua")]
    internal class PremakeLanguageClient : ILanguageClient
    {
        public string Name => "Premake Language Server";

        public IEnumerable<string> ConfigurationSections => null;

        public object InitializationOptions => null;

        public IEnumerable<string> FilesToWatch => null;

        public event AsyncEventHandler<EventArgs> StartAsync;
        public event AsyncEventHandler<EventArgs> StopAsync;

        public async Task<Connection> ActivateAsync(CancellationToken token)
        {
            var extensionFolder = Path.GetDirectoryName(typeof(PremakeLanguageClient).Assembly.Location);
            var serverPath = Path.Combine(extensionFolder ?? ".", "PremakeLanguageServer.exe");

            var psi = new ProcessStartInfo(serverPath)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = Process.Start(psi)
                ?? throw new InvalidOperationException("Failed to start language server process.");

            _ = Task.Run(async () =>
            {
                try
                {
                    using (var sr = process.StandardError)
                    {
                        string line;
                        while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) != null)
                        {
                            // TODO: route to Output Window pane
                        }
                    }
                }
                catch { }
            });

            return new Connection(process.StandardOutput.BaseStream, process.StandardInput.BaseStream);
        }

        public Task OnLoadedAsync()
        {
            return Task.CompletedTask;
        }

        // Called when server initialization fails. Return an InitializationFailureContext to control notification.
        public Task<InitializationFailureContext> OnServerInitializeFailedAsync(ILanguageClientInitializationInfo initializationState)
        {
            // Return null to indicate default behavior; older C# doesn't support nullable syntax in signature.
            return Task.FromResult<InitializationFailureContext>(null);
        }

        public Task OnServerInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public bool ShowNotificationOnInitializeFailed => false;
    }
}
