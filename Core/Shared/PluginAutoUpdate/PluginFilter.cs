using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginFilter
    {
        public List<DirectoryInfo> GetSourcePluginsToCompile(string sourceDirectoryName, string compiledDirectoryName)
        {
            var sourceDirectories = new DirectoryInfo(sourceDirectoryName).GetDirectories();
            var compiledDirectories = new DirectoryInfo(compiledDirectoryName).GetDirectories();
            List<DirectoryInfo> toCompile = new List<DirectoryInfo>();
            foreach (var sourceDirectory in sourceDirectories)
            {
                var compiledDirectory = compiledDirectories.Where(c => c.Name == sourceDirectory.Name).FirstOrDefault();
                if (compiledDirectory == null)
                {
                    toCompile.Add(sourceDirectory);
                    continue;
                }
                var compileSettingsDirectory = PluginCopyFiles.GetSettingsDirectory(compiledDirectory);
                var latestCompileChange = LatestChangeInDirectory(compiledDirectory, compileSettingsDirectory);
                var latestSourceChange = LatestChangeInDirectory(sourceDirectory);
                if (latestSourceChange > latestCompileChange)
                {
                    toCompile.Add(sourceDirectory);
                    continue;
                }
            }
            return toCompile;
        }

        private DateTime LatestChangeInDirectory(DirectoryInfo directory, DirectoryInfo exclude = null)
        {
            directory.Refresh();
            var files = directory.GetFiles("*", SearchOption.AllDirectories);
            var lastWriteTime = DateTime.MinValue;
            foreach (var file in files)
            {
                if (exclude != null && IsChildOfDirectory(file, exclude)) continue;

                if (file.LastWriteTime > lastWriteTime)
                {
                    lastWriteTime = file.LastWriteTime;
                }
            }
            return lastWriteTime;
        }

        private bool IsChildOfDirectory(FileInfo file, DirectoryInfo directory)
        {
            return directory.GetFiles(file.Name, SearchOption.AllDirectories).Any();
        }
    }
}
