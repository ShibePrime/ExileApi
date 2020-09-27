using ExileCore.Shared.PluginAutoUpdate.Settings;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using SharpDX;
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

        public void Update(SinglePluginUpdateSettings plugin)
        {
            if (!plugin.Enable)
            {
                DebugWindow.LogMsg($"{plugin.Name}: Update disabled in settings!");
                return;
            }
            var sw = Stopwatch.StartNew();
            DebugWindow.LogDebug($"{plugin.Name}: Start update.");
            var repositoryPath = Path.Combine(SourceDirectory, plugin.Name);

            if (!Repository.IsValid(repositoryPath))
            {
                DebugWindow.LogMsg($"{plugin.Name}: No valid repository at: {repositoryPath}. Starting to clone...");
                try
                {
                    Clone(plugin.SourceUrl?.Value, repositoryPath);
                    sw.Stop();
                    DebugWindow.LogMsg($"{plugin.Name}: Clone successful in {sw.ElapsedMilliseconds} ms, from {plugin.SourceUrl?.Value}.", 5, Color.Green);
                    return;
                }
                catch (Exception e)
                {
                    DebugWindow.LogError($"{plugin.Name} -> Clone failed. Make sure the folder Plugins/Source/{plugin.Name} does not exist. Skipped!");
                    DebugWindow.LogDebug($"{plugin.Name} -> {e.Message}");
                    return;
                }
            }

            var repository = new Repository(repositoryPath);

            try
            {
                var status = Pull(repository);
                sw.Stop();
                if (status == MergeStatus.UpToDate)
                {
                    DebugWindow.LogMsg($"{plugin.Name}: Already up to date, checked in {sw.ElapsedMilliseconds} ms.");
                    return;
                }
                else if (status == MergeStatus.FastForward || status == MergeStatus.NonFastForward)
                {
                    DebugWindow.LogMsg($"{plugin.Name}: Update successful in {sw.ElapsedMilliseconds} ms.", 5, Color.Green);
                    return;
                }
                else
                {
                    throw new Exception(status.ToString());
                }
            }
            catch (Exception e)
            {
                DebugWindow.LogError($"{plugin.Name}: Update failed. Skipped!");
                DebugWindow.LogDebug($"{plugin.Name} -> {e.Message}");
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

        private MergeStatus Pull(Repository repository)
        {
            var options = new PullOptions
            {
                FetchOptions = GetFetchOptions()
            };
            var signature = new Signature(new Identity("ExileApi", "nomail"), DateTimeOffset.Now);
            var result = Commands.Pull(repository, signature, options);
            return result.Status;
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
                        Password = PluginsUpdateSettings.Password?.Value,
                    }
                )
            };
            return fetchOptions;
        }
    }
}
