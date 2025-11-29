using System;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;

namespace premaker_lang_server
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                // Allow clean shutdown on Ctrl+C
                // Allow clean shutdown on Ctrl+C
                e.Cancel = true;
                cts.Cancel();
            };

            ILanguageServer? server = null;
            {
                server = await LanguageServer.From(options => options
                    .WithInput(Console.OpenStandardInput())
                    .WithOutput(Console.OpenStandardOutput())
                    .WithHandler<TextDocumentHandler>()
                ).ConfigureAwait(false);

                // Wait for either server exit or cancellation
                var waitForExit = server.WaitForExit;
                var cancellationTask = Task.Run(() => { cts.Token.WaitHandle.WaitOne(); });

                var completed = await Task.WhenAny(waitForExit, cancellationTask).ConfigureAwait(false);

                if (completed == cancellationTask)
                {
                    // Attempt graceful shutdown
                    try
                    {
                        if (server is ILanguageServer ls)
                        {
                            // DisposeAsync if available
                            if (ls is IAsyncDisposable asyncDisp)
                            {
                                await asyncDisp.DisposeAsync().ConfigureAwait(false);
                            }
                            else if (ls is IDisposable disp)
                            {
                                disp.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Language server failed: {ex}");
                        return 1;
                    }
                    finally
                    {
                        // Last-ditch dispose
                        if (server != null)
                        {
                            await DisposeServerAsync(server).ConfigureAwait(false);
                        }
                    }
                }
            }
            return 0;
        }

        private static async Task DisposeServerAsync(ILanguageServer? server)
        {
            if (server == null) return;

            try
            {
                if (server is IAsyncDisposable asyncDisp)
                {
                    await asyncDisp.DisposeAsync().ConfigureAwait(false);
                    return;
                }

                if (server is IDisposable disp)
                {
                    disp.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Best effort logging
                try { Console.Error.WriteLine($"Error disposing server: {ex}"); } catch { }
            }
        }
    }
}
