import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {
  const disposable = vscode.commands.registerCommand('premaker.helloWorld', () => {
    vscode.window.showInformationMessage('Hello from Premaker!');
  });
  context.subscriptions.push(disposable);
}

export function deactivate() {}
