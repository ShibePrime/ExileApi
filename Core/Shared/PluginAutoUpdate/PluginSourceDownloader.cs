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
            DebugWindow.LogMsg($"{plugin.Name?.Value} -> Update... started");

            var repository = CloneHelper(plugin);
            if (repository == null) return;

            PullHelper(plugin, repository);
            CheckOutHelper(plugin, repository);

            DebugWindow.LogMsg($"{plugin.Name?.Value} -> Update... done", 5, Color.Green);
        }

        private Repository CloneHelper(SinglePluginUpdateSettings plugin)
        {
            var repositoryPath = Path.Combine(SourceDirectory, plugin.Name?.Value);

            if (Repository.IsValid(repositoryPath)) return new Repository(repositoryPath);

            DebugWindow.LogMsg($"{plugin.Name?.Value} -> No valid repository at: {repositoryPath}. Clone from {plugin.SourceUrl?.Value}... started");
            try
            {
                Clone(plugin.SourceUrl?.Value, repositoryPath);
                DebugWindow.LogMsg($"{plugin.Name?.Value} -> Clone... done", 5, Color.Green);
                return new Repository(repositoryPath);
            }
            catch (Exception e)
            {
                DebugWindow.LogError($"{plugin.Name?.Value} -> Clone... failed");
                DebugWindow.LogDebug($"{plugin.Name?.Value} -> {e.Message}");
                return null;
            }
        }

        private void PullHelper(SinglePluginUpdateSettings plugin, Repository repository)
        {
            try
            {
                DebugWindow.LogMsg($"{plugin.Name?.Value} -> Checkout master/main branch... started");
                var masterBranch = repository
                    .Branches
                    .Where(b => b.FriendlyName == "master" || b.FriendlyName == "main")
                    .SingleOrDefault();

                if (masterBranch == null)
                {
                    DebugWindow.LogError($"{plugin.Name?.Value} -> master/main branch does not exist");
                    return;
                }

                Commands.Checkout(repository, masterBranch);
                DebugWindow.LogMsg($"{plugin.Name?.Value} -> Checkout master/main branch... done", 5, Color.Green);


                DebugWindow.LogMsg($"{plugin.Name?.Value} -> Pull... started");
                var status = Pull(repository);
                if (status == MergeStatus.UpToDate)
                {
                    DebugWindow.LogMsg($"{plugin.Name?.Value} -> Pull... done, already up to date", 5, Color.Green);
                    return;
                }
                else if (status == MergeStatus.FastForward || status == MergeStatus.NonFastForward)
                {
                    DebugWindow.LogMsg($"{plugin.Name?.Value} -> Pull... done", 5, Color.Green);
                    return;
                }
                else
                {
                    throw new Exception(status.ToString());
                }
            }
            catch (Exception e)
            {
                DebugWindow.LogError($"{plugin.Name?.Value} -> Pull... failed");
                DebugWindow.LogDebug($"{plugin.Name?.Value} -> {e.Message}");
            }
        }

        private void CheckOutHelper(SinglePluginUpdateSettings plugin, Repository repository)
        {
            plugin.CommitShaLatest = repository.Head.Tip.Sha;
            if (!plugin.Enable && plugin.CommitShaCurrentIsValid)
            {
                try
                {
                    DebugWindow.LogMsg($"{plugin.Name?.Value} -> Check out commit {plugin.CommitShaCurrent?.Value}... started");
                    Commands.Checkout(repository, plugin.CommitShaCurrent.Value);
                    DebugWindow.LogMsg($"{plugin.Name?.Value} -> Check out commit {plugin.CommitShaCurrent?.Value}... done", 5, Color.Green);
                }
                catch (Exception e)
                {
                    DebugWindow.LogError($"{plugin.Name?.Value} -> Check out commit {plugin.CommitShaCurrent?.Value}... failed");
                    DebugWindow.LogDebug($"{plugin.Name?.Value} -> {e.Message}");
                }
            }
            plugin.CommitShaCurrent.Value = repository.Head.Tip.Sha;
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
                        Username = PluginsUpdateSettings.Username?.Value,
                        Password = PluginsUpdateSettings.Password?.Value,
                    }
                )
            };
            return fetchOptions;
        }
    }
}
