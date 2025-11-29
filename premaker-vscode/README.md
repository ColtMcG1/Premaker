# Premaker VS Code Extension (scaffold)

This folder contains a minimal TypeScript scaffold for a VS Code extension.

Quick start

1. From `premaker-vscode` run:

```powershell
npm install
npm run compile
```

2. Open this repo in VS Code and launch the `Run Extension` debug configuration.

3. Run the command palette and execute `Premaker: Hello World` to see a sample notification.

To package: `npm run package` (requires `vsce` installed as devDependency).

Settings

- `premaker.executable` (string): executable used to run premake scripts (default: `premake5`).
- `premaker.fileArg` (string): argument to pass the script file (default: `--file`). Leave empty to pass the file path positionally.
- `premaker.extraArgs` (string): any extra arguments to append to the premake command.
