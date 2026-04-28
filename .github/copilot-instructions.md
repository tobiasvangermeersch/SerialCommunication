# Copilot instructions for SerialCommunication

## Build, test, and lint commands

- Visual Studio: open SerialCommunication.csproj (or add the project to a .sln) and build/debug from the IDE (VS 2017+).
- MSBuild (command line):
  - Debug: msbuild "SerialCommunication.csproj" /p:Configuration=Debug
  - Release: msbuild "SerialCommunication.csproj" /p:Configuration=Release
- dotnet CLI (when SDK installed and compatible):
  - Build: dotnet build "SerialCommunication.csproj" /p:Configuration=Debug
- Run the app (after building):
  - PowerShell: Start-Process .\bin\Debug\SerialCommunication.exe
  - Or run the executable from Explorer.

- Tests: This repo currently contains no test projects. When a test project (e.g. SerialCommunication.Tests) is added:
  - Run full suite: dotnet test <TestProjectPath>
  - Run a single test (dotnet): dotnet test <TestProj> --filter "FullyQualifiedName=Namespace.ClassName.MethodName"
  - Run a single test (vstest): vstest.console.exe <path-to-test-dll> /Tests:Namespace.ClassName.MethodName
  - Visual Studio: use Test Explorer to run individual tests.

- Linting/analysis: No analyzer or EditorConfig found. To enable linting, add Roslyn analyzers (NuGet) or an .editorconfig and run build to surface warnings.

## High-level architecture

- Project type: Windows Forms desktop application targeting .NET Framework 4.7.2 (old-style csproj).
- Entry point: Program.Main (Program.cs) — constructs and runs Form1.
- UI surface: Form1 is a partial class split across Form1.Designer.cs (auto-generated layout) and Form1.cs (event handlers, runtime logic).
- Serial I/O: Uses System.IO.Ports. Port enumeration is performed at startup (SerialPort.GetPortNames()). Connection logic should live in Form1.cs (see buttonConnect_Click placeholder).
- Resources: Images live under Resources/ and are embedded via the csproj.
- Output artifacts: bin\Debug\SerialCommunication.exe (Debug) and bin\Release\ (Release).

## Key conventions (repo-specific)

- Language: UI labels and many identifiers are Dutch (e.g., Poort, Verbonden, Instellingen, Oefening). Search/source readers should expect Dutch strings in UI code.
- Designer workflow: Do not manually edit Form1.Designer.cs. Add controls and layout changes using the WinForms designer; place runtime code in Form1.cs.
- Event handlers: Named using the control_event pattern (e.g., buttonConnect_Click). Add business logic in those handlers or in helper classes called from them.
- Serial implementation: Keep SerialPort configuration and read/write logic in Form1.cs; consider extracting protocol parsing to separate classes for testability if unit tests are added.
- Adding tests: Create a separate test project (prefer MSTest/xUnit/NUnit), reference the main project, and use dotnet test or vstest.console.exe in CI.
- Project format: Old MSBuild (non-SDK) — open or build with msbuild or Visual Studio. There is no .sln in the repository root.

## AI assistant config files

- No assistant configs detected (CLAUDE.md, AGENTS.md, .cursorrules, .windsurfrules, CONVENTIONS.md, etc.).

---

Update this file when a solution, test project, or CI is added so build/test/lint commands stay accurate.
