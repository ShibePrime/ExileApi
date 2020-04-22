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
        public bool ShouldCompilePlugin(DirectoryInfo SourceDirectory, DirectoryInfo CompiledDirectory)
        {
            if (!SourceDirectory.Exists) return false;
            if (!CompiledDirectory.Exists) return true;

            var compileSettingsDirectory = PluginCopyFiles.GetDirectoryByNamesCompiled(CompiledDirectory, PluginCopyFiles.SettingsDirectoryNames);
            var latestCompileChange = LatestChangeInDirectory(CompiledDirectory, compileSettingsDirectory);
            var latestSourceChange = LatestChangeInDirectory(SourceDirectory);

            if (latestSourceChange > latestCompileChange)
            {
                return true;
            }
            return false;
        }

        private DateTime LatestChangeInDirectory(DirectoryInfo directory, DirectoryInfo[] excludes = null)
        {
            directory.Refresh();
            var files = directory.GetFiles("*", SearchOption.AllDirectories);
            var lastWriteTime = DateTime.MinValue;
            foreach (var file in files)
            {
                if (excludes != null && excludes.Any(exclude => IsChildOfDirectory(file, exclude))) continue;

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
