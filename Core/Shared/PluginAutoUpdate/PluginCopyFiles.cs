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
        public static List<string> DependenciesDirectoryNames => new List<string> { "libs", "Libs", "lib", "Lib", "packages", "Packages" };
        public static List<string> StaticFilesNames => new List<string> { "images", "Images", "img", "Img", "static", "Static" };

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
                DependenciesDirectoryNames,
                "",
                true
            );
        }

        public static void CopyStaticFiles(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            CopyFolder(
                sourceDirectory,
                compiledDirectory,
                StaticFilesNames
            );
        }

        private static void CopyFolder(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory, List<string> possibleFolderName, string suffixIfExists = "", bool putFilesIntoRoot = false)
        {
            var sourceSettings = GetDirectoryByNamesSource(sourceDirectory, possibleFolderName);
            if (sourceSettings == null) return;
            compiledDirectory.Create();
            var compiledSettings = GetDirectoryByNamesCompiled(compiledDirectory, possibleFolderName);
            string targetName = sourceSettings.Name;
            if (compiledSettings != null && suffixIfExists != "")
            {
                targetName = targetName + suffixIfExists;
            }

            string targetPath;
            if (!putFilesIntoRoot)
            {
                targetPath = Path.Combine(compiledDirectory.FullName, targetName);
            }
            else
            {
                targetPath = compiledDirectory.FullName;
            }
            var target = new DirectoryInfo(targetPath);

            CopyAll(sourceSettings, target);
        }

        public static DirectoryInfo GetDirectoryByNamesCompiled(DirectoryInfo directory, List<string> names)
        {
            var result = directory
                .GetDirectories()
                .FirstOrDefault(d => names.Contains(d.Name));
            return result;
        }


        public static DirectoryInfo GetDirectoryByNamesSource(DirectoryInfo directory, List<string> names)
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
