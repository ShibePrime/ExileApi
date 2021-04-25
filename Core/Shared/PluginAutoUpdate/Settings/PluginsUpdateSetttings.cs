using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate.Settings
{
    public class PluginsUpdateSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public TextNode Username { get; set; } = new TextNode("username");
        public TextNode Password { get; set; } = new TextNode("password");
        public List<SinglePluginUpdateSettings> Plugins { get; set; } = new List<SinglePluginUpdateSettings>();
        private List<SinglePluginUpdateSettings> PluginsToDelete { get; set; } = new List<SinglePluginUpdateSettings>();



        private const string UniqueName = "##PluginsUpdateSettings";
        public void Draw()
        {
            var enable = Enable.Value;
            ImGui.Checkbox($"Enable the PluginAutoUpdate mechanismn {UniqueName}", ref enable);
            Enable.Value = enable;

            var duplicatedPlugins = GetDuplicatedPlugins(Plugins);
            if (duplicatedPlugins.Count > 0)
            {
                ImGui.Spacing();
                ImGui.Spacing();

                ImGui.TextColored(Color.Red.ToImguiVec4(), "Error. The following plugins are duplicated: ");
                foreach (var plugin in duplicatedPlugins)
                {
                    ImGui.TextColored(Color.Red.ToImguiVec4(), $"{plugin.Name.Value} -> {plugin.SourceUrl.Value}");
                }
            }

            var pluginsWithInvalidCommit = Plugins.Where(p => !p.CommitShaCurrentIsValid);
            if (pluginsWithInvalidCommit.Count() > 0)
            {
                ImGui.Spacing();
                ImGui.Spacing();
                ImGui.TextColored(Color.Red.ToImguiVec4(), "Error. The following plugins have an invlaid commit sha: ");
            }
            foreach (var plugin in pluginsWithInvalidCommit)
            {
                ImGui.TextColored(Color.Red.ToImguiVec4(), $"{plugin.Name.Value} -> {plugin.CommitShaCurrent?.Value}");
            }


            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();

            if (ImGui.Button("Add new Plugin"))
            {
                var newPlugin = new SinglePluginUpdateSettings()
                {
                    Enable = new ToggleNode(true),
                    Name = new TextNode("new Plugin"),
                    SourceUrl = new TextNode(""),
                    CommitShaCurrent = new TextNode(""),
                    CommitShaLatest = "",
                };
                Plugins.Reverse();
                Plugins.Add(newPlugin);
                Plugins.Reverse();
            }

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();

            Plugins.RemoveAll(s => PluginsToDelete.Contains(s));
            PluginsToDelete.Clear();

            foreach (var plugin in Plugins)
            {
                plugin.DeleteRequested += OnDeleteRequested;
                plugin.Draw();
            }
        }

        private List<SinglePluginUpdateSettings> GetDuplicatedPlugins(List<SinglePluginUpdateSettings> plugins) 
        {
            var result = new List<SinglePluginUpdateSettings>();
            var allPluginNames = Plugins.Select(p => p.Name.Value);
            foreach (var plugin in Plugins)
            {
                if (allPluginNames.Where(s => s == plugin.Name.Value).Count() <= 1) continue;

                result.Add(plugin);
            }
            return result;
        }

        private void OnDeleteRequested(object sender, EventArgs eventArgs)
        {
            if (!(sender is SinglePluginUpdateSettings))
            {
                DebugWindow.LogError("PluginsUpdateSettings -> invalid event received");
                return;
            }

            PluginsToDelete.Add(sender as SinglePluginUpdateSettings);
        }
    }
}
