using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using System;
using System.Linq;
using SharpDX;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Attributes;
using Newtonsoft.Json;

namespace ExileCore.Shared.PluginAutoUpdate.Settings
{
    public class SinglePluginUpdateSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public TextNode Name { get; set; } = new TextNode();
        public TextNode SourceUrl { get; set; } = new TextNode();
        public TextNode CommitShaCurrent { get; set; } = new TextNode();
        
        [JsonIgnore]
        public string CommitShaLatest { get; set; } = "";
        [JsonIgnore]
        public bool CommitShaCurrentIsValid => CommitShaCurrent?.Value?.Length == 40 || CommitShaCurrent?.Value?.Length == 0;
        [JsonIgnore]
        public bool UpdateAvailable => CommitShaLatest != "" && CommitShaCurrent?.Value != CommitShaLatest;
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
            ImGui.TextColored(Color.Green.ToImguiVec4(), Name);

            ImGui.Indent(30);

            var enable = Enable.Value;
            ImGui.Checkbox($"Autoupdate{UniqueName}", ref enable);
            Enable.Value = enable;

            ImGui.SameLine();
            ImGui.Indent(160);

            if (!Enable && CommitShaCurrent != CommitShaLatest && CommitShaLatest != "")
            {
                if(ImGui.Button($"Update{UniqueName}"))
                {
                    CommitShaCurrent.Value = CommitShaLatest;
                }
            }

            ImGui.SameLine();
            ImGui.Indent(160);

            if(ImGui.Button($"Delete{UniqueName}"))
            {
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }

            ImGui.Unindent(320);

            string sourceUrl = SourceUrl?.Value ?? "";
            ImGui.InputText(UniqueName + "SourceUrl", ref sourceUrl, 200);
            ChangeSourceUrlValue(sourceUrl);

            string commitId = CommitShaCurrent?.Value ?? "";
            if (Enable)
            {
                ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.3f);
                ImGui.InputText(UniqueName + "CommitId", ref commitId, 40, ImGuiInputTextFlags.ReadOnly);
                ImGui.PopStyleVar();
            }
            else
            {
                ImGui.InputText(UniqueName + "CommitId", ref commitId, 100);
            }
            CommitShaCurrent.Value = commitId;

            ImGui.Unindent(30);

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
        }
    }
}
