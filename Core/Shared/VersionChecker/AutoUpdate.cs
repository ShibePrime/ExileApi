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
        private const string RELEASE_FOLDER = "PoeHelper";
        private const string UPDATE_FOLDER = "update";
        private const string UPDATER_EXECUTEABLE = "Updater.exe";
        private static string releaseFileName => $"{RELEASE_FOLDER}.zip";
        public bool IsDownloading { get; private set; }
        public bool IsReadyToUpdate { get; private set; }

        public AutoUpdate()
        {
            IsDownloading = false;
            IsReadyToUpdate = false;
        }

        public void PrepareUpdate(GithubReleaseResponse latestVersionResponse)
        {
            IsDownloading = true;
            var releaseZip = latestVersionResponse.Assets
                .Where(asset => asset.FileName.Equals(releaseFileName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (!releaseZip.FileName.Equals(releaseFileName, StringComparison.InvariantCultureIgnoreCase))
            {
                DebugWindow.LogError("AutoUpdate -> Download failed, release .zip url not found.");
                IsDownloading = false;
                return;
            }

            var fileLocation = Path.Combine(UPDATE_FOLDER, releaseFileName);

            if (UseExistingRelease(latestVersionResponse))
            {
                IsReadyToUpdate = true;
            }
            else
            {
                Download(releaseZip.BrowserDownloadUrl, fileLocation);
                Unzip(fileLocation);
                IsReadyToUpdate = true;
            }
            IsDownloading = false;
        }

        private bool UseExistingRelease(GithubReleaseResponse latestVersionResponse)
        {
            var unzippedDir = new DirectoryInfo(Path.Combine(UPDATE_FOLDER, RELEASE_FOLDER));            
            if (!unzippedDir.Exists) return false;

            var localVersion = VersionChecker.LoadLocalVersion(Path.Combine(unzippedDir.FullName, VersionChecker.VERSION_FILE_NAME));
            if (localVersion == null)
            {
                DebugWindow.LogError($"AutoUpdate -> Cant read local version");
                CleanUpdateFolder();
                return false;
            }

            var latestVersion = VersionChecker.ConvertStringToVersionJson(latestVersionResponse.VersionString);
            if (latestVersion == null)
            {
                DebugWindow.LogError($"AutoUpdate -> Cant read latest version, {latestVersionResponse.VersionString}");
                CleanUpdateFolder();
                return false;
            }

            var versionComparison = VersionChecker.VersionComparison(localVersion.Value, latestVersion.Value);
            if (versionComparison.IsUpdateAvailable())
            {
                DebugWindow.LogError($"AutoUpdate -> Current update folder is outdated, delete and start update process again");
                CleanUpdateFolder();
                return false;
            }

            return true;
        }

        private static void CleanUpdateFolder()
        {
            var directory = new DirectoryInfo(UPDATE_FOLDER);

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }

        private void Download(string zipUrl, string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                DebugWindow.LogMsg("AutoUpdate -> Update .zip file already exists");
                return;
            }

            var dir = Path.GetDirectoryName(fileLocation);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            DebugWindow.LogMsg("AutoUpdate -> Download update...");
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(zipUrl, fileLocation);
                }
                catch (Exception e)
                {
                    DebugWindow.LogError($"AutoUpdate -> Download update... failed");
                    DebugWindow.LogError($"AutoUpdate -> {e.Message}");
                }
            }
            DebugWindow.LogMsg("AutoUpdate -> Donwload update... done");
        }

        private string Unzip(string fileLocation)
        {
            var resultFolder = Path.Combine(UPDATE_FOLDER, RELEASE_FOLDER);
            try
            {
                DebugWindow.LogMsg($"AutoUpdate -> Extract .zip...");
                ZipFile.ExtractToDirectory(fileLocation, resultFolder);
                DebugWindow.LogMsg($"AutoUpdate -> Extract .zip... done");
            }
            catch
            {
                DebugWindow.LogError($"AutoUpdate -> Extract .zip... failed");
                try
                {
                    DebugWindow.LogMsg("AutoUpdate -> Delete existing .zip file...");
                    File.Delete(fileLocation);
                    DebugWindow.LogMsg("AutoUpdate -> Delete existing .zip file... done");

                    DebugWindow.LogMsg("AutoUpdate -> Delete already unpacked data...");
                    CleanUpdateFolder();
                    DebugWindow.LogMsg("AutoUpdate -> Delete already unpacked data... done");
                }
                catch (Exception e)
                {
                    DebugWindow.LogError("AutoUpdate -> Delete exisiting .zip file or already unpacked data... failed");
                    DebugWindow.LogMsg($"AutoUpdate -> You need to delete the content of the folder \"{UPDATE_FOLDER}\" manually ");
                    DebugWindow.LogDebug($"AutoUpdate -> {e.Message}");
                }
            }
            
            return resultFolder;
        }

        public void LaunchUpdater()
        {
            var startInfo = new ProcessStartInfo();            
            startInfo.FileName = Path.Combine(Application.StartupPath, UPDATER_EXECUTEABLE);
            startInfo.Arguments = $"\"{Path.Combine(Application.StartupPath, UPDATE_FOLDER)}\" \"{RELEASE_FOLDER}\" \"{Application.ExecutablePath}\"";
            Process.Start(startInfo);
            Application.Exit();
        }      

    }
}
