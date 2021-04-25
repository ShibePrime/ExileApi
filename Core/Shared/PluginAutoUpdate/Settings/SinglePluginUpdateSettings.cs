using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using System;
using System.Linq;
using SharpDX;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Attributes;

namespace ExileCore.Shared.PluginAutoUpdate.Settings
{
    public class SinglePluginUpdateSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public TextNode Name { get; set; } = new TextNode();
        public TextNode SourceUrl { get; set; } = new TextNode();
        public TextNode CommitShaCurrent { get; set; } = new TextNode();
        
        [IgnoreMenu]
        public string CommitShaLatest { get; set; } = "";
        [IgnoreMenu]
        public bool CommitShaCurrentIsValid => CommitShaCurrent?.Value?.Length == 40 || CommitShaCurrent?.Value?.Length == 0;
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

            bool update = false;
            if (!Enable && CommitShaCurrent != CommitShaLatest && CommitShaLatest != "") 
                update = ImGui.Button("Update");

            ImGui.SameLine();
            ImGui.Indent(160);

            bool delete = ImGui.Button("Delete");

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


            // handle buttons
            if (update)
            {
                CommitShaCurrent.Value = CommitShaLatest;
            }
            if (delete)
            {
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }

            //ImGui.SameLine();
            //ImGui.Indent(344);
            //if (ImGui.Button("Delete"))
            //{
            //    DeleteRequested?.Invoke(this, EventArgs.Empty);
            //}
            //ImGui.Unindent(344);


            //ImGui.Indent(20);
            //string sourceUrl = SourceUrl?.Value ?? "";
            //ImGui.InputText(UniqueName + "SourceUrl", ref sourceUrl, 200);
            //ChangeSourceUrlValue(sourceUrl);
            //ImGui.Unindent(20);

            //string commitId = CommitId?.Value ?? "";
            //if (Enable)
            //{
            //    ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
            //    ImGui.InputText(UniqueName + "CommitId", ref commitId, 100, ImGuiInputTextFlags.ReadOnly);
            //    ImGui.PopStyleVar();
            //}
            //else
            //{
            //    ImGui.InputText(UniqueName + "CommitId", ref commitId, 100);
            //}
            //CommitId.Value = commitId;

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
        }


    }
}
