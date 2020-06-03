using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExileCore.Shared.VersionChecker
{
    public class AutoUpdate
    {
        private const string ReleaseFilename = "PoeHelper";
        private const string UpdateFolder = "update";
        private static string ReleaseFilenameWithEnding => $"{ReleaseFilename}.zip";


        public static void Update(GithubReleaseResponse githubReleaseResponse)
        {
            var releaseZip = githubReleaseResponse.Assets
                .Where(asset => asset.FileName.Equals(ReleaseFilenameWithEnding, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (!releaseZip.FileName.Equals(ReleaseFilenameWithEnding, StringComparison.InvariantCultureIgnoreCase))
            {
                DebugWindow.LogError("Update failed -> Release .zip file not found.");
                return;
            }

            var fileLocation = Path.Combine(UpdateFolder, ReleaseFilenameWithEnding);
            Download(releaseZip.BrowserDownloadUrl, fileLocation);
            LaunchUpdater();
        }

        private static void Download(string zipUrl, string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                DebugWindow.LogMsg("Update file already present.");
                return;
            }

            DebugWindow.LogMsg("Download update...");
            using (var client = new WebClient())
            {
                client.DownloadFile(zipUrl, fileLocation);
            }
            DebugWindow.LogMsg("Donwload update... done");
        }

        private static void LaunchUpdater()
        {
            var startInfo = new ProcessStartInfo();            
            startInfo.FileName = Path.Combine(Application.StartupPath, "Updater.exe");
            startInfo.Arguments = Path.Combine(Application.StartupPath, UpdateFolder) + " " + ReleaseFilenameWithEnding + " " + Application.ExecutablePath;
            Process.Start(startInfo);
            Application.Exit();
        }      

    }
}
