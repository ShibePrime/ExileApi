using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExileCore.Shared.Attributes;

namespace ExileCore.Shared.PluginAutoUpdate.Settings
{
    public class SinglePluginUpdateSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public TextNode Name { get; set; } = new TextNode();
        public TextNode SourceUrl { get; set; } = new TextNode();
        [IgnoreMenu]
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        private Random Random { get; } = new Random();
        private string _uniqueName = "";
        private string UniqueName
        {
            get
            {
                if (_uniqueName == "")
                {
                    _uniqueName = $"##{Name?.Value}{Random.Next(0, int.MaxValue)}";
                }
                return _uniqueName;
            }
        }

        public event EventHandler DeleteRequested;

        public void ChangeSourceUrlValue(string value)
        {
            if (SourceUrl?.Value == null) return;
            if (SourceUrl?.Value == value) return;
            SourceUrl.Value = value;
            Name = value.Split('/').Last();
        }

        public void Draw()
        {
            var enable = Enable.Value;
            ImGui.Checkbox($"{Name?.Value}{UniqueName}", ref enable);
            Enable.Value = enable;

            ImGui.SameLine();
            ImGui.Indent(344);
            if (ImGui.Button("Delete"))
            {
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }
            ImGui.Unindent(344);


            ImGui.Indent(20);
            string sourceUrl = SourceUrl.Value;
            ImGui.InputText(UniqueName + "SourceUrl", ref sourceUrl, 200);
            ChangeSourceUrlValue(sourceUrl);
            ImGui.Unindent(20);

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
        }


    }
}
