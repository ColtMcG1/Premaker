import * as vscode from "vscode";
import * as path from "path";

export function run_premake_file_command(
  defaultPath?: string
): vscode.Disposable {
  return vscode.commands.registerCommand("premaker.runScript", (uri?: vscode.Uri) => {
    const filePath = uri?.fsPath ?? defaultPath ?? vscode.window.activeTextEditor?.document.uri.fsPath;
    if (!filePath) {
      vscode.window.showErrorMessage('No premake file selected to run.');
      return;
    }

    const filename = path.basename(filePath).toLowerCase();
    if (filename !== 'premake.lua' && filename !== 'premake5.lua') {
      const proceed = 'Run anyway';
      vscode.window.showWarningMessage(`Selected file (${filename}) does not look like a premake script.`, proceed).then(sel => {
        if (sel === proceed) {
          runInTerminal(filePath);
        }
      });
      return;
    }

    runInTerminal(filePath);
  });
}

function runInTerminal(filePath: string) {
  const terminalName = 'Premake';
  const terminal = vscode.window.terminals.find(t => t.name === terminalName) ?? vscode.window.createTerminal(terminalName);
  terminal.show(true);
  // Read user settings for executable and args
  const cfg = vscode.workspace.getConfiguration('premaker');
  const exe = cfg.get<string>('executable', 'premake5');
  const fileArg = cfg.get<string>('fileArg', '--file');
  const extraArgs = cfg.get<string>('extraArgs', '').trim();

  // Build the command safely
  const escapedPath = filePath.replace(/"/g, '\\"');
  const parts: string[] = [];
  parts.push(exe);
  if (fileArg && fileArg.length > 0) {
    parts.push(fileArg);
    parts.push(`"${escapedPath}"`);
  } else {
    // positional
    parts.push(`"${escapedPath}"`);
  }
  if (extraArgs) {
    parts.push(extraArgs);
  }

  const cmd = parts.join(' ');
  terminal.sendText(cmd);
  vscode.window.showInformationMessage(`Running premake: ${path.basename(filePath)}`);
}
