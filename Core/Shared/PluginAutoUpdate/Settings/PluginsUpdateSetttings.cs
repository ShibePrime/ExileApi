using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
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

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();

            if (ImGui.Button("Add new Plugin"))
            {
                var newPlugin = new SinglePluginUpdateSettings()
                {
                    Enable = new ToggleNode(true),
                    SourceUrl = new TextNode(""),
                    LastUpdated = DateTime.Now
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
