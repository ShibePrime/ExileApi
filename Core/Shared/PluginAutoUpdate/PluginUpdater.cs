using ExileCore.Shared.PluginAutoUpdate.Settings;
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
                DebugWindow.LogMsg($"PluginUpdater -> Plugin Auto Update is deactivated!");
                return tasks;
            }

            var pluginCompiler = new PluginCompiler();
            foreach (var plugin in PluginsUpdateSettings.Plugins)
            {
                tasks.Add(Task.Run(() => UpdateSinglePlugin(plugin, PluginSourceDownloader, PluginFilter, pluginCompiler, RootDirectory, PluginLoader)));
            }
            return tasks;
        }

        private List<PluginWrapper> UpdateSinglePlugin(
            SinglePluginUpdateSettings plugin, 
            PluginSourceDownloader pluginSourceDownloader, 
            PluginFilter pluginFilter, 
            IPluginCompiler pluginCompiler,
            string rootDirectory,
            PluginLoader pluginLoader
            )
        {

            var sourcePluginDirectory = new DirectoryInfo(Path.Combine(SourcePluginsDirectory, plugin.Name?.Value));
            var compiledPluginDirectory = new DirectoryInfo(Path.Combine(CompiledPluginsDirectory, plugin.Name?.Value));

            try
            {
                pluginSourceDownloader.Update(plugin);

                if (!pluginFilter.ShouldCompilePlugin(sourcePluginDirectory, compiledPluginDirectory)) return null;

                var dependencyTasks = PluginCopyFiles.CopyDependencies(sourcePluginDirectory, compiledPluginDirectory);
                var settingsTasks = PluginCopyFiles.CopySettings(sourcePluginDirectory, compiledPluginDirectory);
                var staticFilesTasks = PluginCopyFiles.CopyStaticFiles(sourcePluginDirectory, compiledPluginDirectory);
                var txtJsonFilesTask = PluginCopyFiles.CopyTxtAndJsonFromRoot(sourcePluginDirectory, compiledPluginDirectory);
                var txtJsonDefaultFilesTask = PluginCopyFiles.CopyTxtAndJsonDefaultFiles(sourcePluginDirectory, compiledPluginDirectory);
                
                if (dependencyTasks != null) Task.WaitAll(dependencyTasks.ToArray());

                var csProjFiles = sourcePluginDirectory
                    .GetFiles("*.csproj", SearchOption.AllDirectories)
                    .Where(f => f.Extension == ".csproj");
                foreach (var csProjFile in csProjFiles)
                {
                    pluginCompiler.CompilePlugin(
                        csProjFile,
                        compiledPluginDirectory.FullName,
                        new DirectoryInfo(rootDirectory)
                        );
                }

                var remainingTasks = new List<Task>();
                remainingTasks.AddRange(settingsTasks);
                remainingTasks.AddRange(staticFilesTasks);
                remainingTasks.AddRange(txtJsonFilesTask);
                remainingTasks.AddRange(txtJsonDefaultFilesTask);

                if (remainingTasks != null) Task.WaitAll(remainingTasks.ToArray());
            } 
            catch (Exception e)
            {
                DebugWindow.LogError($"PluginUpdater -> {plugin} UpdateSinglePlugin failed.");
                DebugWindow.LogDebug($"PluginUpdater -> {e.Message}");
            }

            var pluginWrapper = pluginLoader.Load(compiledPluginDirectory);
            return pluginWrapper;
        }

        public string GetOutputDirectory(DirectoryInfo source)
        {
            return Path.Combine(CompiledPluginsDirectory, source.Name);
        }
    }
}
