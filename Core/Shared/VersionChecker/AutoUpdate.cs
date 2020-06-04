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
        private const string UPDATER_EXECUTEABLE = "Updater.exe";
        private static string ReleaseFilenameWithExtension => $"{RELEASE_FILENAME}.zip";
        public bool IsDownloading { get; private set; }
        public bool IsReadyToUpdate { get; private set; }

        public AutoUpdate()
        {
            IsDownloading = false;
            IsReadyToUpdate = false;
        }

        public void PrepareUpdate(GithubReleaseResponse githubReleaseResponse)
        {
            IsDownloading = true;
            var releaseZip = githubReleaseResponse.Assets
                .Where(asset => asset.FileName.Equals(ReleaseFilenameWithExtension, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (!releaseZip.FileName.Equals(ReleaseFilenameWithExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                DebugWindow.LogError("Update failed -> Download not possible, release .zip url not found.");
                IsDownloading = false;
                return;
            }

            var fileLocation = Path.Combine(UPDATE_FOLDER, ReleaseFilenameWithExtension);

            if (FileExists(fileLocation))
            {
                IsReadyToUpdate = true;
            }
            else
            {
                Download(releaseZip.BrowserDownloadUrl, fileLocation);
                IsReadyToUpdate = true;
            }
            IsDownloading = false;
        }

        private bool FileExists(string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                DebugWindow.LogMsg("Update file exists.");
                return true;
            }
            return false;
        }

        private void Download(string zipUrl, string fileLocation)
        {
            DebugWindow.LogMsg("Download update...");
            using (var client = new WebClient())
            {
                client.DownloadFile(zipUrl, fileLocation);
            }
            DebugWindow.LogMsg("Donwload update... done");
        }

        public void LaunchUpdater()
        {
            var startInfo = new ProcessStartInfo();            
            startInfo.FileName = Path.Combine(Application.StartupPath, UPDATER_EXECUTEABLE);
            startInfo.Arguments = Path.Combine(Application.StartupPath, UPDATE_FOLDER) + " " + ReleaseFilenameWithExtension + " " + Application.ExecutablePath;
            Process.Start(startInfo);
            Application.Exit();
        }      

    }
}
