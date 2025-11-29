using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace premaker_lang_server
{
    internal class TextDocumentHandler : CompletionHandlerBase
    {
        protected override CompletionRegistrationOptions CreateRegistrationOptions(
            CompletionCapability capability, ClientCapabilities clientCapabilities
            )
            => new CompletionRegistrationOptions 
            { 
                TriggerCharacters = new[] { "." },
                DocumentSelector = new TextDocumentSelector(
                    new TextDocumentFilter 
                    { 
                        Pattern = "**/*.lua"
                    })
            };

        public override Task<CompletionList> Handle(
            CompletionParams request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CompletionList(new[]
            {
                new CompletionItem { Label = "HelloWorld", Kind = CompletionItemKind.Text }
            }));
        }

        public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
    }
}

