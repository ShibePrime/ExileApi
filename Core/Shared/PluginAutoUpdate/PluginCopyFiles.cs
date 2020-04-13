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
        public static List<string> DependenciesDirectoryNames => new List<string> { "libs", "Libs", "lib", "Lib" };

        public static void CopySettings(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            CopyFolder(
                sourceDirectory,
                compiledDirectory,
                SettingsDirectoryNames,
                "_new"
            );
        }

        public static void CopyDependencies(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            CopyFolder(
                sourceDirectory,
                compiledDirectory,
                DependenciesDirectoryNames
            );
        }

        private static void CopyFolder(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory, List<string> possibleFolderName, string suffixIfExists = "")
        {
            var sourceSettings = GetDirectoryByNames(sourceDirectory, possibleFolderName);
            if (sourceSettings == null) return;
            compiledDirectory.Create();
            var compiledSettings = GetDirectoryByNames(compiledDirectory, possibleFolderName);
            string targetName = sourceSettings.Name;
            if (compiledSettings != null && suffixIfExists != "")
            {
                targetName = targetName + suffixIfExists;
            }

            var target = new DirectoryInfo(Path.Combine(compiledDirectory.FullName, targetName));
            CopyAll(sourceSettings, target);
        }

        public static DirectoryInfo GetDirectoryByNames(DirectoryInfo directory, List<string> names)
        {
            var csprojPath = directory.GetFiles($"{directory.Name}.csproj", SearchOption.AllDirectories).FirstOrDefault();
            if (csprojPath == null) return null;

            var result = csprojPath.Directory
                .GetDirectories()
                .FirstOrDefault(d => names.Contains(d.Name));
            return result;
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
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
