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
        private string RootDirectory { get; }
        private string PluginsDirectory { get; }
        private string CompiledPluginsDirectory { get; }
        private string SourcePluginsDirectory { get; }
        private string PluginUpdateSettingsPath => Path.Combine(PluginsDirectory, "updateSettings.json");

        private PluginFilter PluginFilter { get; }
        private PluginSourceDownloader PluginSourceDownloader { get; }


        public PluginUpdater(string rootDirectory, string pluginsDirectory, string compiledPluginsDirectory, string sourcePluginsDirectory)
        {
            RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
            PluginsDirectory = pluginsDirectory ?? throw new ArgumentNullException(nameof(pluginsDirectory));
            CompiledPluginsDirectory = compiledPluginsDirectory ?? throw new ArgumentNullException(nameof(compiledPluginsDirectory));
            SourcePluginsDirectory = sourcePluginsDirectory ?? throw new ArgumentNullException(nameof(sourcePluginsDirectory));

            PluginFilter = new PluginFilter();
            PluginSourceDownloader = new PluginSourceDownloader(SourcePluginsDirectory);
        }

        public List<Task> UpdateAllAsync()
        {
            var pluginUpdateSettings = SettingsContainer.LoadSettingFile<PluginsSettings>(PluginUpdateSettingsPath);
            if (pluginUpdateSettings == null)
            {
                DebugWindow.LogMsg($"{PluginUpdateSettingsPath} doesn't exists. Plugins wont be updated!");
                return null;
            }
            if (!pluginUpdateSettings.Enable)
            {
                DebugWindow.LogMsg($"Plugin Auto Update is deactivated!");
                return null;
            }

            var pluginCompiler = new PluginCompiler(new DirectoryInfo(RootDirectory));
            List<Task> tasks = new List<Task>();
            foreach (var plugin in pluginUpdateSettings.Plugins)
            {
                tasks.Add(Task.Run(() => UpdateSinglePlugin(plugin, PluginSourceDownloader, PluginFilter, pluginCompiler)));
            }
            return tasks;
        }

        private void UpdateSinglePlugin(
            PluginSettings plugin, 
            PluginSourceDownloader pluginSourceDownloader, 
            PluginFilter pluginFilter, 
            PluginCompiler pluginCompiler
            )
        {
            pluginSourceDownloader.Update(plugin);
            var sourcePluginDirectory = new DirectoryInfo(Path.Combine(SourcePluginsDirectory, plugin.Name));
            var compiledPluginDirectory = new DirectoryInfo(Path.Combine(CompiledPluginsDirectory, plugin.Name));
            if (!pluginFilter.ShouldCompilePlugin(sourcePluginDirectory, compiledPluginDirectory)) return;

            PluginCopyFiles.CopySettings(sourcePluginDirectory, compiledPluginDirectory);
            pluginCompiler.CompilePlugin(sourcePluginDirectory, compiledPluginDirectory.FullName);
        }

        public string GetOutputDirectory(DirectoryInfo source)
        {
            return Path.Combine(CompiledPluginsDirectory, source.Name);
        }
    }
}
