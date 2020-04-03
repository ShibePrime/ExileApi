using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Shared.PluginAutoUpdate
{
    public class PluginsSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public List<PluginSettings> Plugins { get; set; }
    }

    public class PluginSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public string Name { get; set; }
        public string SourceUrl { get; set; }
    }
}
