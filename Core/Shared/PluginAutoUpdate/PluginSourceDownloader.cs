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

        public PluginSourceDownloader(string sourceDirectory)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
        }

        public void Update(PluginSettings plugin)
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
            Repository.Clone(url, path);
        }

        private void Pull(Repository repository)
        {
            var options = new PullOptions
            {
                FetchOptions = new FetchOptions()
            };
            var signature = new Signature(new Identity("ExileApi", "nomail"), DateTimeOffset.Now);
            Commands.Pull(repository, signature, options);
        }
    }
}
