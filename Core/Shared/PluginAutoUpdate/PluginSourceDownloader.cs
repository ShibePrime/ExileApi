using LibGit2Sharp;
using LibGit2Sharp.Handlers;
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
        private PluginsUpdateSettings PluginsUpdateSettings { get; }

        public PluginSourceDownloader(string sourceDirectory, PluginsUpdateSettings pluginsUpdateSettings)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
            PluginsUpdateSettings = pluginsUpdateSettings ?? throw new ArgumentNullException(nameof(pluginsUpdateSettings));
        }

        public void Update(PluginUpdateSettings plugin)
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
                    DebugWindow.LogError($"{plugin.Name}: Clone failed. Skipped!");
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
                DebugWindow.LogError($"{plugin.Name}: Update failed. Skipped!");
            }
        }

        private void Clone(string url, string path)
        {
            var fetchOptions = GetFetchOptions();
            var cloneOptions = new CloneOptions
            {
                CredentialsProvider = fetchOptions.CredentialsProvider
            };
            Repository.Clone(url, path, cloneOptions);
        }

        private void Pull(Repository repository)
        {
            var options = new PullOptions
            {
                FetchOptions = GetFetchOptions()
            };
            var signature = new Signature(new Identity("ExileApi", "nomail"), DateTimeOffset.Now);
            Commands.Pull(repository, signature, options);
        }

        private FetchOptions GetFetchOptions()
        {
            var fetchOptions = new FetchOptions
            {
                CredentialsProvider = new CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = PluginsUpdateSettings.Username,
                        Password = PluginsUpdateSettings.Password,
                    }
                )
            };
            return fetchOptions;
        }
    }
}
