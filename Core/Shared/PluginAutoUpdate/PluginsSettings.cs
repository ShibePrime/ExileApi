using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginsUpdateSettings : ISettings
    {
        [Menu("Enable", Tooltip = "(De)activate the whole PluginAutoUpdate mechanismn")]
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        [Menu("Github Username", Tooltip = "optional")]
        public TextNode Username { get; set; } = new TextNode("username");
        [Menu("Github Password", Tooltip = "optional")]
        public TextNode Password { get; set; } = new TextNode("password");
        public List<PluginUpdateSettings> Plugins { get; set; } = new List<PluginUpdateSettings>();
    }

    public class PluginUpdateSettings : ISettings
    {
        [Menu("Enable Plugin", Tooltip = "(De)activate AutoUpdate for this Plugin")]
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public TextNode Name { get; set; } = new TextNode();
        public TextNode SourceUrl { get; set; } = new TextNode();
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
    }
}
