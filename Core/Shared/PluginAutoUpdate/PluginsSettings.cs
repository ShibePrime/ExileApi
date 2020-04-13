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
        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        [Menu("Github credentials, optional!")]
        public string Username { get; set; } = new TextNode();
        public string Password { get; set; } = new TextNode();
        public List<PluginUpdateSettings> Plugins { get; set; }
    }

    public class PluginUpdateSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public string Name { get; set; }
        public string SourceUrl { get; set; }
    }
}
