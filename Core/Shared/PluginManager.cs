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
using ExileCore.Shared.PluginAutoUpdate.Settings;
using JM.LinqFaster;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using MoreLinq.Extensions;
using SharpDX;
using SharpDX.Direct3D11;

namespace ExileCore.Shared
{
    public class PluginManager
    {
        private const string PluginsDirectory = "Plugins";
        private const string CompiledPluginsDirectory = "Compiled";
        private const string SourcePluginsDirectory = "Source";
        private string AutoPluginUpdateSettingsPath => Path.Combine(PluginsDirectory, "updateSettings.json");
        private string AutoPluginUpdateSettingsPathDump => Path.Combine("config", "dumpUpdateSettings.json");
        private readonly GameController _gameController;
        private readonly Graphics _graphics;
        private readonly MultiThreadManager _multiThreadManager;
        private readonly SettingsContainer _settingsContainer;

        private Dictionary<string, string> Directories { get; }
        public bool AllPluginsLoaded { get; private set; }
        public string RootDirectory { get; }
        public List<PluginWrapper> Plugins { get; private set; }

        public PluginManager(
            GameController gameController, 
            Graphics graphics, 
            MultiThreadManager multiThreadManager,
            SettingsContainer settingsContainer
            )
        {
            _gameController = gameController;
            _graphics = graphics;
            _multiThreadManager = multiThreadManager;
            _settingsContainer = settingsContainer;

            Plugins = new List<PluginWrapper>();
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
                    DebugWindow.LogMsg($"PluginManager -> {directory.Value} doesn't exists, but don't worry i created it for you.");
                    Directory.CreateDirectory(directory.Value);
                }
            }

            Task.Run(() => LoadPlugins(gameController));
        }

        private void LoadPlugins(GameController gameController)
        {
            var pluginLoader = new PluginLoader(_gameController, _graphics, this);

            var pluginUpdateSettings = _settingsContainer.PluginsUpdateSettings;
            var loadPluginTasks = new List<Task<List<PluginWrapper>>>();
            var stopLoadFromCompiledDirectory = new List<string>();

            // check for changes in the updateSettings. Delete changed repositories, to make sure all changes are acted upon
            if (pluginUpdateSettings.Enable)
            {
                var file = new FileInfo(AutoPluginUpdateSettingsPathDump);
                PluginsUpdateSettings dumpPluginUpdateSettings = null;
                if (file.Exists)
                {
                    dumpPluginUpdateSettings = SettingsContainer.LoadSettingFile<PluginsUpdateSettings>(AutoPluginUpdateSettingsPathDump);
                }

                RemoveChangedPlugins(pluginUpdateSettings, dumpPluginUpdateSettings);
                SettingsContainer.SaveSettingFile(AutoPluginUpdateSettingsPathDump, pluginUpdateSettings);
                try
                {
                    var pluginAutoUpdates = RunPluginAutoUpdate(pluginLoader, pluginUpdateSettings);
                    loadPluginTasks.AddRange(pluginAutoUpdates);

                    stopLoadFromCompiledDirectory = pluginUpdateSettings
                        .Plugins
                        .Select(p => p.Name?.Value)
                        .ToList();
                }
                catch (Exception e)
                {
                    DebugWindow.LogError("PluginManager -> AutoUpdate failed, load all compiled plugins.");
                    DebugWindow.LogError($"PluginManager -> {e.Message}");
                    stopLoadFromCompiledDirectory = new List<string>();
                }
            }

            loadPluginTasks.AddRange(LoadCompiledDirPlugins(pluginLoader, stopLoadFromCompiledDirectory));

            Task.WaitAll(loadPluginTasks?.ToArray());

            Plugins = loadPluginTasks
                .Where(t => t.Result != null)
                .SelectMany(t => t.Result)
                .OrderBy(x => x.Order)
                .ThenByDescending(x => x.CanBeMultiThreading)
                .ThenBy(x => x.Name)
                .ToList();

            AddPluginInfoToDevTree();

            InitialisePlugins(gameController);

            AreaOnOnAreaChange(gameController.Area.CurrentArea);
            AllPluginsLoaded = true;
        }

        private void RemoveChangedPlugins(PluginsUpdateSettings settings, PluginsUpdateSettings dumpSettings)
        {
            if (dumpSettings == null) return;

            foreach (var plugin in settings.Plugins)
            {
                var fittingPluginFromDump = dumpSettings.Plugins
                    .Where(d => d.Name?.Value == plugin.Name?.Value)
                    .Where(d => d.SourceUrl?.Value == plugin.SourceUrl?.Value)
                    .FirstOrDefault();
                // unchanged entry
                if (fittingPluginFromDump != null) continue;

                try
                {
                    DebugWindow.LogMsg($"PluginManager -> Remove old files from {plugin.Name?.Value}");
                    DeleteFilesFromPlugin(plugin.Name?.Value);
                } 
                catch (Exception e)
                {
                    DebugWindow.LogError($"PluginManager -> Remove old files from {plugin.Name?.Value} failed");
                    DebugWindow.LogDebug($"PluginManager -> {e.Message}");
                }
            }
        }

        private void DeleteFilesFromPlugin(string pluginName)
        {
            var sourceDirectory = new DirectoryInfo(Path.Combine(Directories[PluginsDirectory], SourcePluginsDirectory));
            var compiledDirectory = new DirectoryInfo(Path.Combine(Directories[PluginsDirectory], CompiledPluginsDirectory));

            var directoriesToDelete = new List<DirectoryInfo>();
            directoriesToDelete.AddRange(sourceDirectory.GetDirectories(pluginName, SearchOption.TopDirectoryOnly));
            directoriesToDelete.AddRange(compiledDirectory.GetDirectories(pluginName, SearchOption.TopDirectoryOnly));

            foreach (var directory in directoriesToDelete)
            {
                foreach (var f in directory.GetFiles("*", SearchOption.AllDirectories))
                {
                    f.Attributes = FileAttributes.Normal;
                    f.Delete();
                }
                directory.Delete(true);
            }
        }

        private List<Task<List<PluginWrapper>>> RunPluginAutoUpdate(
            PluginLoader pluginLoader, 
            PluginsUpdateSettings pluginsUpdateSettings)
        {
            var pluginUpdater = new PluginUpdater(
                pluginsUpdateSettings,
                RootDirectory,
                Directories[CompiledPluginsDirectory],
                Directories[SourcePluginsDirectory],
                pluginLoader
                );

            var pluginTasks = pluginUpdater.UpdateAndLoadAllAsync();
            return pluginTasks;
        }

        private List<Task<List<PluginWrapper>>> LoadCompiledDirPlugins(
            PluginLoader pluginLoader,
            List<string> excludedPluginNames)
        {
            if (excludedPluginNames == null) excludedPluginNames = new List<string>();

            var compiledDirectories = new DirectoryInfo(Directories[CompiledPluginsDirectory])
                .GetDirectories()
                .Where(di => !excludedPluginNames.Any(
                    excludedName => excludedName.Equals(di.Name, StringComparison.InvariantCultureIgnoreCase)
                    )
                );

            var loadTasks = new List<Task<List<PluginWrapper>>>();
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
