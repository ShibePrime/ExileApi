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
        private const string RELEASE_FILENAME = "PoeHelper";
        private const string UPDATE_FOLDER = "update";
        private static string ReleaseFilenameWithExtension => $"{RELEASE_FILENAME}.zip";


        public static void Update(GithubReleaseResponse githubReleaseResponse)
        {
            var releaseZip = githubReleaseResponse.Assets
                .Where(asset => asset.FileName.Equals(ReleaseFilenameWithExtension, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (!releaseZip.FileName.Equals(ReleaseFilenameWithExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                DebugWindow.LogError("Update failed -> Download not possible, release .zip url not found.");
                return;
            }

            var fileLocation = Path.Combine(UPDATE_FOLDER, ReleaseFilenameWithExtension);
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
            startInfo.Arguments = Path.Combine(Application.StartupPath, UPDATE_FOLDER) + " " + ReleaseFilenameWithExtension + " " + Application.ExecutablePath;
            Process.Start(startInfo);
            Application.Exit();
        }      

    }
}
