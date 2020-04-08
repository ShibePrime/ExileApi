using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginCopyFiles
    {
        public static List<string> SettingsDirectoryNames => new List<string> { "settings", "Settings", "config", "Config" };     

        public static void CopySettings(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            var sourceSettings = GetSettingsDirectory(sourceDirectory);
            if (sourceSettings == null) return;
            compiledDirectory.Create();
            var compiledSettings = GetSettingsDirectory(compiledDirectory);
            string targetName = sourceSettings.Name;
            if (compiledSettings != null)
            {
                targetName = targetName + "_new";
            }

            var target = new DirectoryInfo(Path.Combine(compiledDirectory.FullName, targetName));
            CopyAll(sourceSettings, target);
        }

        public static DirectoryInfo GetSettingsDirectory(DirectoryInfo directory)
        {
            var csprojPath = directory.GetFiles($"{directory.Name}.csproj", SearchOption.AllDirectories).FirstOrDefault();
            if (csprojPath == null) return null;

            var result = csprojPath.Directory
                .GetDirectories()
                .FirstOrDefault(d => SettingsDirectoryNames.Contains(d.Name));
            return result;
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
