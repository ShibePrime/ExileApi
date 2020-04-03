using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginSourceDownloader
    {
        private string SourceDirectory { get; }
        private PluginsSettings PluginsSettings { get; }

        public PluginSourceDownloader(string sourceDirectory, PluginsSettings pluginsSettings)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            PluginsSettings = pluginsSettings ?? throw new ArgumentNullException(nameof(pluginsSettings));
        }

        public void UpdateAll()
        {
            foreach (var plugin in PluginsSettings.Plugins)
            {
                Update(plugin);                
            }
        }

        public void Update(string pluginName)
        {
            var plugin = PluginsSettings.Plugins.FirstOrDefault(p => p.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase));
            if (plugin == null) return;
            Update(plugin);
        }

        private void Update(PluginSettings plugin)
        {
            if (!plugin.Enable)
            {
                DebugWindow.LogMsg($"{plugin.Name}: Update disabled in settings!");
                return;
            }
            var sw = Stopwatch.StartNew();
            DebugWindow.LogMsg($"{plugin.Name}: Start update.");
            var repositoryPath = Path.Combine(SourceDirectory, plugin.Name);
            Repository repository = null;

            try
            {
                repository = new Repository(repositoryPath);
                if (repository == null) throw new Exception();
            }
            catch
            {
                DebugWindow.LogMsg($"{plugin.Name}: No valid repository at: {repositoryPath}. Starting to clone...");
                try
                {
                    Clone(plugin.SourceUrl, repositoryPath);
                    sw.Stop();
                    DebugWindow.LogMsg($"{plugin.Name}: Clone successful in {sw.ElapsedMilliseconds} ms.");
                    return;
                }
                catch
                {
                    DebugWindow.LogMsg($"{plugin.Name}: Clone failed. Skipped!");
                    return;
                }
            }

            try
            {
                Pull(repository);
                sw.Stop();
                DebugWindow.LogMsg($"{plugin.Name}: Update successful in {sw.ElapsedMilliseconds} ms.");
            }
            catch
            {
                DebugWindow.LogMsg($"{plugin.Name}: Update failed. Skipped!");
            }
        }

        private void Clone(string url, string path)
        {
            Repository.Clone(url, path);
        }

        private void Pull(Repository repository)
        {
            var options = new PullOptions();
            options.FetchOptions = new FetchOptions();
            var signature = new Signature(new Identity("ExileApi", ""), DateTimeOffset.Now);
            Commands.Pull(repository, signature, options);
        }
    }
}
