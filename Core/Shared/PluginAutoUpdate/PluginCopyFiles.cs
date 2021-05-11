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
        public static List<string> StaticFilesNames => new List<string> { "images", "Images", "img", "Img", "static", "Static", "textures", "Textures", "texture", "Texture" };


        public static List<Task> CopyTxtAndJsonFromRoot(
            DirectoryInfo sourceDirectory, 
            DirectoryInfo compiledDirectory)
        {
            var files = new List<FileInfo>();
            files.AddRange(sourceDirectory.GetFiles("*.txt", SearchOption.TopDirectoryOnly).ToList());
            files.AddRange(sourceDirectory.GetFiles("*.json", SearchOption.TopDirectoryOnly).ToList());

            return CopyListOfFiles(sourceDirectory, compiledDirectory, files, false);
        }

        public static List<Task> CopyTxtAndJsonDefaultFiles(
            DirectoryInfo sourceDirectory, 
            DirectoryInfo compiledDirectory)
        {
            var files = new List<FileInfo>();
            files.AddRange(sourceDirectory.GetFiles("*default*.txt", SearchOption.AllDirectories).ToList());
            files.AddRange(sourceDirectory.GetFiles("*default*.json", SearchOption.AllDirectories).ToList());

            return CopyListOfFiles(sourceDirectory, compiledDirectory, files, true);
        }

        public static List<Task> CopyListOfFiles(
            DirectoryInfo sourceDirectory, 
            DirectoryInfo compiledDirectory,
            List<FileInfo> files,
            bool overwrite)
        {
            var tasks = new List<Task>();

            foreach (var file in files)
            {
                var relativeSourcePath = file.FullName.Remove(0, sourceDirectory.FullName.Length + 1);
                var compiledFilePath = Path.Combine(compiledDirectory.FullName, relativeSourcePath);

                var compiledFile = new FileInfo(compiledFilePath);
                if (compiledFile.Exists && !overwrite) continue;

                var task = Task.Run(() => file.CopyTo(compiledFilePath, overwrite));
                tasks.Add(task);
            }

            return tasks;
        }

        public static List<Task> CopySettings(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            return CopyFolder(
                sourceDirectory,
                compiledDirectory,
                SettingsDirectoryNames,
                "_new"
            );            
        }

        public static List<Task> CopyDependencies(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            return CopyFolder(
                sourceDirectory,
                compiledDirectory,
                DependenciesDirectoryNames,
                "",
                true
            );
        }

        public static List<Task> CopyStaticFiles(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory)
        {
            return CopyFolder(
                sourceDirectory,
                compiledDirectory,
                StaticFilesNames
            );
        }

        private static List<Task> CopyFolder(DirectoryInfo sourceDirectory, DirectoryInfo compiledDirectory, List<string> possibleFolderName, string suffixIfExists = "", bool putFilesIntoRoot = false)
        {
            var sourceFolders = GetDirectoryByNamesSource(sourceDirectory, possibleFolderName);
            if (sourceFolders == null) return null;
            compiledDirectory.Create();
            var compiledFolders = GetDirectoryByNamesCompiled(compiledDirectory, possibleFolderName);

            List<Task> copyTasks = new List<Task>();
            foreach (var sourceFolder in sourceFolders)
            {
                string targetName = sourceFolder.Name;
                if (compiledFolders.Any(c => c.Name.Equals(targetName)) && suffixIfExists != "")
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

                copyTasks.Add(Task.Run(() => CopyAll(sourceFolder, target)));
            }
            return copyTasks;
        }

        public static DirectoryInfo[] GetDirectoryByNamesCompiled(DirectoryInfo directory, List<string> names)
        {
            var result = directory
                .GetDirectories()
                .Where(d => names.Contains(d.Name))
                .ToArray();
            return result;
        }


        public static DirectoryInfo[] GetDirectoryByNamesSource(DirectoryInfo directory, List<string> names)
        {
            var csprojPath = directory.GetFiles($"{directory.Name}.csproj", SearchOption.AllDirectories).FirstOrDefault();
            if (csprojPath == null)
            {
                csprojPath = directory.GetFiles("*.csproj", SearchOption.AllDirectories).FirstOrDefault();
            }
            if (csprojPath == null) return null;

            var result = csprojPath.Directory
                .GetDirectories()
                .Where(d => names.Contains(d.Name))
                .ToArray();
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
