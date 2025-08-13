# Continue for Visual Studio

This folder contains a minimal Visual Studio extension that exercises shared functionality from the `core` package.

## Development

1. Install **Visual Studio Community 2022** with the *Visual Studio extension development* workload.
2. From this repository root run:
   ```bash
   cd extensions/visualstudio/node
   npm install
   npm run build
   ```
3. Build the VSIX project:
   ```bash
   cd ..
   dotnet build
   ```
4. Open the generated `.vsix` in Visual Studio to verify that the extension loads and shows a confirmation message.

The extension invokes a small Node script that imports code from `core` to ensure the shared functionality is available.
