using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.PluginAutoUpdate;
using JM.LinqFaster;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using MoreLinq.Extensions;
using SharpDX;

namespace ExileCore.Shared
{
    public class PluginManager
    {
        private const string PluginsDirectory = "Plugins";
        private const string CompiledPluginsDirectory = "Compiled";
        private const string SourcePluginsDirectory = "Source";
        private readonly GameController _gameController;
        private readonly Graphics _graphics;
        private readonly MultiThreadManager _multiThreadManager;
        private Dictionary<string, string> Directories { get; }
        private  Dictionary<string, Stopwatch> PluginLoadTime { get; } = new Dictionary<string, Stopwatch>();

        public bool AllPluginsLoaded { get; }
        public string RootDirectory { get; }
        public List<PluginWrapper> Plugins { get; } = new List<PluginWrapper>();

        public PluginManager(GameController gameController, Graphics graphics, MultiThreadManager multiThreadManager)
        {
            _gameController = gameController;
            _graphics = graphics;
            _multiThreadManager = multiThreadManager;
            Directories = new Dictionary<string, string>();
            RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directories[PluginsDirectory] = Path.Combine(RootDirectory, PluginsDirectory);

            Directories[CompiledPluginsDirectory] =
                Path.Combine(Directories[PluginsDirectory], CompiledPluginsDirectory);

            Directories[SourcePluginsDirectory] = Path.Combine(Directories[PluginsDirectory], SourcePluginsDirectory);
            _gameController.EntityListWrapper.EntityAdded += EntityListWrapperOnEntityAdded;
            _gameController.EntityListWrapper.EntityRemoved += EntityListWrapperOnEntityRemoved;
            _gameController.EntityListWrapper.EntityAddedAny += EntityListWrapperOnEntityAddedAny;
            _gameController.EntityListWrapper.EntityIgnored += EntityListWrapperOnEntityIgnored;
            _gameController.Area.OnAreaChange += AreaOnOnAreaChange;


            foreach (var directory in Directories)
            {
                if (!Directory.Exists(directory.Value))
                {
                    DebugWindow.LogMsg($"{directory.Value} doesn't exists, but don't worry i created it for you.");
                    Directory.CreateDirectory(directory.Value);
                }
            }

            var pluginLoader = new PluginLoader(_gameController, _graphics, this);

            var pluginUpdateSettingsPath = Path.Combine(PluginsDirectory, "updateSettings.json");
            var pluginUpdateSettings = SettingsContainer.LoadSettingFile<PluginsUpdateSettings>(pluginUpdateSettingsPath);

            List<Task> loadPluginTasks = new List<Task>();
            if (pluginUpdateSettings != null)
            {
                loadPluginTasks.AddRange(RunPluginAutoUpdate(pluginLoader, pluginUpdateSettings));
            }
            loadPluginTasks.AddRange(LoadCompiledDirPlugins(pluginLoader, pluginUpdateSettings));

            Task.WaitAll(loadPluginTasks?.ToArray());

            Plugins = Plugins
                .OrderBy(x => x.Order)
                .ThenByDescending(x => x.CanBeMultiThreading)
                .ThenBy(x => x.Name)
                .ToList();
            AddPluginInfoToDevTree();

            InitialisePlugins(gameController);

            AreaOnOnAreaChange(gameController.Area.CurrentArea);
            AllPluginsLoaded = true;
        }

        private List<Task> RunPluginAutoUpdate(PluginLoader pluginLoader, PluginsUpdateSettings pluginsUpdateSettings)
        {
            var pluginUpdater = new PluginUpdater(
                pluginsUpdateSettings,
                Directories[PluginsDirectory],
                Directories[CompiledPluginsDirectory],
                Directories[SourcePluginsDirectory],
                pluginLoader
                );

            var pluginTasks = pluginUpdater.UpdateAndLoadAllAsync();
            var pluginFullyLoadedTasks = new List<Task>();
            foreach (var pluginTask in pluginTasks)
            {
                var task = pluginTask.ContinueWith(delegate (Task<List<PluginWrapper>> pluginWrappers)
                {
                    Plugins.AddRange(pluginWrappers.Result);
                });
                pluginFullyLoadedTasks.Add(task);
            }

            return pluginFullyLoadedTasks;
        }

        private List<Task> LoadCompiledDirPlugins(PluginLoader pluginLoader, PluginsUpdateSettings pluginsUpdateSettings)
        {
            List<string> excludedNames = new List<string>();
            if (pluginsUpdateSettings != null)
            {
                excludedNames.AddRange(pluginsUpdateSettings.Plugins?.Select(p => p.Name));
            }
            var compiledDirectories = new DirectoryInfo(Directories[CompiledPluginsDirectory])
                .GetDirectories()
                .Where(di => !excludedNames.Any(excludedName => excludedName.Equals(di.Name, StringComparison.InvariantCultureIgnoreCase)));

            List<Task> loadTasks = new List<Task>();
            foreach (var compiledDirectory in compiledDirectories)
            {
                loadTasks.Add(Task.Run(() => pluginLoader.Load(compiledDirectory)));
            }
            return loadTasks;
        }

        private void InitialisePlugins(GameController gameController)
        {
            if (_gameController.Settings.CoreSettings.MultiThreadLoadPlugins)
            {
                //Pre init some general objects because with multi threading load they can null sometimes for some plugin
                var ingameStateIngameUi = gameController.IngameState.IngameUi;
                var ingameStateData = gameController.IngameState.Data;
                var ingameStateServerData = gameController.IngameState.ServerData;
                Parallel.ForEach(Plugins, wrapper => wrapper.Initialise(gameController));
            }
            else
            {
                Plugins.ForEach(wrapper => wrapper.Initialise(gameController));
            }
        }

        private void AddPluginInfoToDevTree()
        {
            var devTree = Plugins.FirstOrDefault(x => x.Name.Equals("DevTree"));

            if (devTree != null)
            {
                try
                {
                    var fieldInfo = devTree.Plugin.GetType().GetField("Plugins");
                    List<PluginWrapper> devTreePlugins() => Plugins;
                    fieldInfo.SetValue(devTree.Plugin, (Func<List<PluginWrapper>>)devTreePlugins);
                }
                catch (Exception e)
                {
                    DebugWindow.LogError(e.ToString());
                }

            }
        }

        public void CloseAllPlugins()
        {
            foreach (var plugin in Plugins)
            {
                plugin.Close();
            }
        }

        private void AreaOnOnAreaChange(AreaInstance area)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.IsEnable)
                    plugin.AreaChange(area);
            }
        }

        private void EntityListWrapperOnEntityIgnored(Entity entity)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.IsEnable)
                    plugin.EntityIgnored(entity);
            }
        }

        private void EntityListWrapperOnEntityAddedAny(Entity entity)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.IsEnable)
                    plugin.EntityAddedAny(entity);
            }
        }

        private void EntityListWrapperOnEntityAdded(Entity entity)
        {
            if (_gameController.Settings.CoreSettings.AddedMultiThread && _multiThreadManager.ThreadsCount > 0)
            {
                var listJob = new List<Job>();

                Plugins.WhereF(x => x.IsEnable).Batch(_multiThreadManager.ThreadsCount)
                    .ForEach(wrappers =>
                        listJob.Add(_multiThreadManager.AddJob(() => wrappers.ForEach(x => x.EntityAdded(entity)),
                            "Entity added")));

                _multiThreadManager.Process(this);
                SpinWait.SpinUntil(() => listJob.AllF(x => x.IsCompleted), 500);
            }
            else
            {
                foreach (var plugin in Plugins)
                {
                    if (plugin.IsEnable)
                        plugin.EntityAdded(entity);
                }
            }
        }

        private void EntityListWrapperOnEntityRemoved(Entity entity)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.IsEnable)
                    plugin.EntityRemoved(entity);
            }
        }

        private void LogError(string msg)
        {
            DebugWindow.LogError(msg, 5);
        }
        
        public void ReceivePluginEvent(string eventId, object args, IPlugin owner)
        {
            foreach (var pluginWrapper in Plugins)
            {
                if (pluginWrapper.IsEnable && pluginWrapper.Plugin != owner)
                    pluginWrapper.ReceiveEvent(eventId, args);
            }
        }
    }
}
