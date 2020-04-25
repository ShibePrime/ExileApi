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
        private string CompiledPluginsDirectory { get; }
        private string SourcePluginsDirectory { get; }

        private PluginsUpdateSettings PluginsUpdateSettings { get; }
        private PluginFilter PluginFilter { get; }
        private PluginSourceDownloader PluginSourceDownloader { get; }
        private PluginLoader PluginLoader { get; }


        public PluginUpdater(
            PluginsUpdateSettings pluginUpdateSettings,
            string rootDirectory, 
            string compiledPluginsDirectory, 
            string sourcePluginsDirectory, 
            PluginLoader pluginLoader)
        {
            PluginsUpdateSettings = pluginUpdateSettings ?? throw new ArgumentNullException(nameof(pluginUpdateSettings));
            RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
            CompiledPluginsDirectory = compiledPluginsDirectory ?? throw new ArgumentNullException(nameof(compiledPluginsDirectory));
            SourcePluginsDirectory = sourcePluginsDirectory ?? throw new ArgumentNullException(nameof(sourcePluginsDirectory));

            PluginFilter = new PluginFilter();
            PluginSourceDownloader = new PluginSourceDownloader(SourcePluginsDirectory, pluginUpdateSettings);
            PluginLoader = pluginLoader;
        }

        public List<Task<List<PluginWrapper>>> UpdateAndLoadAllAsync()
        {
            var tasks = new List<Task<List<PluginWrapper>>>();
            if (!PluginsUpdateSettings.Enable)
            {
                DebugWindow.LogMsg($"Plugin Auto Update is deactivated!");
                return tasks;
            }

            var pluginCompiler = new PluginCompiler(new DirectoryInfo(RootDirectory));
            foreach (var plugin in PluginsUpdateSettings.Plugins)
            {
                tasks.Add(Task.Run(() => UpdateSinglePlugin(plugin, PluginSourceDownloader, PluginFilter, pluginCompiler, PluginLoader)));
            }
            return tasks;
        }

        private List<PluginWrapper> UpdateSinglePlugin(
            PluginUpdateSettings plugin, 
            PluginSourceDownloader pluginSourceDownloader, 
            PluginFilter pluginFilter, 
            PluginCompiler pluginCompiler,
            PluginLoader pluginLoader
            )
        {
            pluginSourceDownloader.Update(plugin);
            var sourcePluginDirectory = new DirectoryInfo(Path.Combine(SourcePluginsDirectory, plugin.Name?.Value));
            var compiledPluginDirectory = new DirectoryInfo(Path.Combine(CompiledPluginsDirectory, plugin.Name?.Value));
            if (!pluginFilter.ShouldCompilePlugin(sourcePluginDirectory, compiledPluginDirectory)) return null;

            var dependencyTasks = PluginCopyFiles.CopyDependencies(sourcePluginDirectory, compiledPluginDirectory);
            var settingsTasks = PluginCopyFiles.CopySettings(sourcePluginDirectory, compiledPluginDirectory);
            var staticFilesTasks = PluginCopyFiles.CopyStaticFiles(sourcePluginDirectory, compiledPluginDirectory);

            Task.WaitAll(dependencyTasks?.ToArray());
            var assembly = pluginCompiler.CompilePlugin(sourcePluginDirectory, compiledPluginDirectory.FullName);

            Task.WaitAll(settingsTasks?.ToArray());
            Task.WaitAll(staticFilesTasks?.ToArray());
            var pluginWrapper = pluginLoader.Load(compiledPluginDirectory, assembly);
            return pluginWrapper;
        }

        public string GetOutputDirectory(DirectoryInfo source)
        {
            return Path.Combine(CompiledPluginsDirectory, source.Name);
        }
    }
}
