using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Continue.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Continue", "Continue for Visual Studio", "0.0.1")]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]

    public sealed class ContinuePackage : AsyncPackage
    {
        public const string PackageGuidString = "72f4ce0c-6689-4e79-b6d3-7e2930c2faba";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            string? assemblyDir = Path.GetDirectoryName(typeof(ContinuePackage).Assembly.Location);
            string nodeScript = Path.Combine(assemblyDir ?? string.Empty, "node", "out", "index.js");
            string output = "";
            try
            {
                var psi = new ProcessStartInfo("node", nodeScript)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var proc = Process.Start(psi);
                if (proc != null)
                {
                    output = await proc.StandardOutput.ReadToEndAsync();
                    await proc.WaitForExitAsync();
                }
            }
            catch (Exception ex)
            {
                output = $"Error running core script: {ex.Message}";
            }

            VsShellUtilities.ShowMessageBox(
                this,
                string.IsNullOrWhiteSpace(output) ? "Continue Visual Studio extension loaded" : output.Trim(),
                "Continue",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            VsShellUtilities.ShowMessageBox(
                this,
                $"Continue cargada.\nNode dijo: {output}",
                "Continue for Visual Studio",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

        }
    }
}
