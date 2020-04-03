using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginUpdater
    {
        private string PluginsDirectory { get; }
        private string CompiledPluginsDirectory { get; }
        private string SourcePluginsDirectory { get; }

        public PluginUpdater(string pluginsDirectory, string compiledPluginsDirectory, string sourcePluginsDirectory)
        {
            PluginsDirectory = pluginsDirectory ?? throw new ArgumentNullException(nameof(pluginsDirectory));
            CompiledPluginsDirectory = compiledPluginsDirectory ?? throw new ArgumentNullException(nameof(compiledPluginsDirectory));
            SourcePluginsDirectory = sourcePluginsDirectory ?? throw new ArgumentNullException(nameof(sourcePluginsDirectory));
        }

        public void DownloadSource()
        {
            var pluginUpdateSettingsPath = Path.Combine(PluginsDirectory, "updateSettings.json");
            var pluginUpdateSettings = SettingsContainer.LoadSettingFile<PluginsSettings>(pluginUpdateSettingsPath);
            if (pluginUpdateSettings == null)
            {
                DebugWindow.LogMsg($"{pluginUpdateSettingsPath} doesn't exists. Plugins wont be updated!");
                return;
            }
            if (!pluginUpdateSettings.Enable)
            {
                DebugWindow.LogMsg($"Plugin Auto Update is deactivated!");
                return;
            }
            var pluginSourceDownloader = new PluginSourceDownloader(SourcePluginsDirectory, pluginUpdateSettings);
            pluginSourceDownloader.UpdateAll();
        }

        public List<DirectoryInfo> GetSourcePluginsToCompile()
        {
            var pluginFilter = new PluginFilter();
            var pluginsToCompile = pluginFilter.GetSourcePluginsToCompile(
                SourcePluginsDirectory,
                CompiledPluginsDirectory
            );
            return pluginsToCompile;
        }

        public void CopySettings(List<DirectoryInfo> sourceDirectories)
        {
            foreach (var plugin in sourceDirectories)
            {
                var compiledDirectory = new DirectoryInfo(GetOutputDirectory(plugin));
                compiledDirectory.Create();
                PluginCopyFiles.CopySettings(
                    plugin,
                    compiledDirectory
                );
            }
        }

        public string GetOutputDirectory(DirectoryInfo source)
        {
            return Path.Combine(CompiledPluginsDirectory, source.Name);
        }
    }
}
