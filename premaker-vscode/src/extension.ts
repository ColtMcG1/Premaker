import * as vscode from 'vscode';
import { run_premake_file_command } from './commands';

export function activate(context: vscode.ExtensionContext) {
  const hello = vscode.commands.registerCommand('premaker.helloWorld', () => {
    vscode.window.showInformationMessage('Hello from Premaker!');
  });
  context.subscriptions.push(hello);

  // Register the premake run command (appears in explorer/context for premake files)
  const runCmd = run_premake_file_command();
  context.subscriptions.push(runCmd);
}

export function deactivate() {}
