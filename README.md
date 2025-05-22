# [Premaker](https://github.com/ColtMcG1/Premaker)
[![CodeQL](https://github.com/ColtMcG1/Premaker/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/ColtMcG1/Premaker/actions/workflows/github-code-scanning/codeql)

Premaker is a Visual Studio extension that allows you to run Premake project files for your solution. It is designed to be easy to use and integrates seamlessly with Visual Studio.

Premaker also makes it easy to manage your Premake versions. You can specify the version of Premake you want to use, and it will automatically download and install it for you. This makes it easy to keep your Premake installation up to date and ensures that you are always using the latest version.

--- 
## Supported Frameworks

Premaker is available as three buildable projects targeting:
- .NET Framework 4.7.2
- .NET 8
- .NET 9

This ensures compatibility with a wide range of Visual Studio and .NET environments.

--- 
## Standalone Application
### Features
- It allows you to run Premake project files for your solution.
- Use can select the version of Premake you want to use.
- You can change and save the arguments you want to pass to Premake in the options.

### Future Features & Functionality
- It can be used to generate Premake project files.

--- 
## Command Line Interface
### Features
You can use all the same features as the standalone application, but from the command line.

### How To Use It
```bash
premaker [options] [args]
```
### Options
```bash
--package, -p  Specify the package to use (premake5.x-beta0.x)
--version, -v  Specify the version of Premake to use
--list <releases, packages>, -l <releases, packages> List available or installed premake versions
--download, -d   Download premake
--remove, -r    Remove premake
```

### Examples
```bash
premaker --package premake5.0-beta1 vs2022
```
```bash
premaker -v 00000000 vs2022
```
```bash
premaker --list releases
```
--- 
## Visual Studio Extension
### Getting Started
- Click install
- That's it!

### How To Use It
- Once in a solution, you can find the command in the solution menu.
- You can also add the icon to your toolbar: **Tools > Customize > Toolbars > VSPremake**

<table>
    <tbody>
        <tr>
            <td><img width="260" alt="img1" src="https://github.com/user-attachments/assets/644d46e3-6e4a-414c-aaa0-0fcb9e789e46" /></td>
            <td><img width="386" alt="img2" src="https://github.com/user-attachments/assets/b4fd3631-9955-42cc-8757-1e3a7b658cb9" /></td>
        </tr>
    </tbody>
</table>

### Settings
- You can provide arguments to Premake through the extension's settings: **Tools > Options > VSPremake**

[Find it on the Microsoft Marketplace here](https://marketplace.visualstudio.com/items?itemName=ColtonMcGraw.VSPremake)
--- 