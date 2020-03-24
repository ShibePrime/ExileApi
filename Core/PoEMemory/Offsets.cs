using System.Collections.Generic;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory
{
    public class Offsets
    {
        public static Offsets Regular = new Offsets { ExeName = "PathOfExile_x64" };
        public static Offsets Korean = new Offsets { ExeName = "Pathofexile_x64_KG" };
        public static Offsets Steam = new Offsets { ExeName = "PathOfExile_x64Steam" };

        /*
            PathOfExile_x64.exe+E06827 - 48 8B 08                 - mov rcx,[rax]
            PathOfExile_x64.exe+E0682A - 48 8D 3D 1F1F4001        - lea rdi,[PathOfExile_x64.exe+2208750]
            PathOfExile_x64.exe+E06831 - 8B 04 0E                 - mov eax,[rsi+rcx]
        */
        //48 8B 08 48 8D 3D ?? ?? ?? ?? 8B 04 0E
        private static readonly Pattern FileRootPattern = new Pattern(new byte[] {
            0x48, 0x8B, 0x08,
            0x48, 0x8D, 0x3D, 0x00, 0x00, 0x00, 0x00,
            0x8B, 0x04, 0x0E,
        }, "xxxxxx????xxx", "File Root", 0xE00000);

        /*
            PathOfExile_x64.exe+13AC79B - F0 0FC1 41 3C         - lock xadd [rcx+3C],eax
            PathOfExile_x64.exe+13AC7A0 - 8B 0D 128EBF00        - mov ecx,[PathOfExile_x64.exe+1FA55B8]
            PathOfExile_x64.exe+13AC7A6 - 48 8B 45 D0           - mov rax,[rbp-30]
        */
        //F0 0F C1 41 3C 8B 0D ?? ?? ?? ?? 48 8B 45 D0
        private static readonly Pattern AreaChangePattern = new Pattern(new byte[] {
            0xF0, 0x00, 0x00, 0x41, 0x3C,
            0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00,
            0x48, 0x8B, 0x45, 0xD0,
        }, "x??xxxx????xxxx", "Area change", 0x900000);

        /*
            PathOfExile_x64.exe+149485 - 48 89 9C 24 80000000  - mov [rsp+00000080],rbx
            PathOfExile_x64.exe+14948D - 48 8B F9              - mov rdi,rcx
            PathOfExile_x64.exe+149490 - 33 ED                 - xor ebp,ebp
            PathOfExile_x64.exe+149492 - 48 39 2D 9F8E0B02     - cmp [PathOfExile_x64.exe+2202338],rbp
        */
        //48 89 9C 24 ?? ?? ?? ?? 48 8B F9 33 ED 48 39 2D ?? ?? ?? ??
        private static readonly Pattern GameStatePattern = new Pattern(new byte[] {
            0x48, 0x89, 0x9C, 0x24, 0x00, 0x00, 0x00, 0x00,
            0x48, 0x8B, 0xF9,
            0x33, 0xED,
            0x48, 0x39, 0x2D, 0x00, 0x00, 0x00, 0x00,
        }, "xxxx????xxxxxxxx????", "Game State", 0x140000);

        public string ExeName { get; private set; }

        public Dictionary<OffsetsName, long> DoPatternScans(IMemory m)
        {
            var result = new Dictionary<OffsetsName, long>();
            var addresses = m.FindPatterns(FileRootPattern, AreaChangePattern, GameStatePattern);
            var baseAddress = m.Process.MainModule.BaseAddress.ToInt64();

            var fileRoot = m.Read<int>(baseAddress + addresses[0] + 6) + addresses[0] + 10;
            var areaChangeCount = m.Read<int>(baseAddress + addresses[1] + 7) + addresses[1] + 11;
            var gameStateOffset = m.Read<int>(baseAddress + addresses[2] + 16) + addresses[2] + 20;

            result.Add(OffsetsName.FileRoot, fileRoot);
            result.Add(OffsetsName.AreaChangeCount, areaChangeCount);
            result.Add(OffsetsName.GameStateOffset, gameStateOffset);

            return result;
        }
    }
}
