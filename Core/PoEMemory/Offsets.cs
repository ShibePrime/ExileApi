using System.Collections.Generic;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;

namespace ExileCore.PoEMemory
{
    public class Offsets
    {
        public static Offsets Regular = new Offsets {IgsOffset = 0, IgsDelta = 0, ExeName = "PathOfExile_x64"};
        public static Offsets Korean = new Offsets {IgsOffset = 0, IgsDelta = 0, ExeName = "Pathofexile_x64_KG"};

        public static Offsets Steam = new Offsets {IgsOffset = 0x28, IgsDelta = 0, ExeName = "PathOfExile_x64Steam"};

        /* FileRoot Pointer
        00007FF6C47EED01  | 48 8D 0D A8 23 7F 00               | lea rcx,qword ptr ds:[7FF6C4FE10B0]        | <--FileRootPtr
        00007FF6C47EED08  | E8 E3 5C 56 FF                     | call pathofexile_x64.7FF6C3D549F0          |
        00007FF6C47EED0D  | 48 8B 3D A4 23 7F 00               | mov rdi,qword ptr ds:[7FF6C4FE10B8]        |
        00007FF6C47EED14  | 48 8B 1F                           | mov rbx,qword ptr ds:[rdi]                 |
        00007FF6C47EED17  | 48 3B DF                           | cmp rbx,rdi                                |
        00007FF6C47EED1A  | 0F 84 26 01 00 00                  | je pathofexile_x64.7FF6C47EEE46            |
        */

        private static readonly Pattern FileRootPattern =
            new Pattern(new byte[]
                {
                    0x48, 0x8b, 0x08,
                    0x4c, 0x8d, 0x35,
                    0x00, 0x00, 0x00, 0x00,
                    0x8b

                }, "xxxxxx????x", "File Root",
                14630000);

        /* Area Change
        00007FF63317CE40 | 48 83 EC 58                    | sub rsp,58                                      |
        00007FF63317CE44 | 4C 8B C1                       | mov r8,rcx                                      |
        00007FF63317CE47 | 41 B9 01 00 00 00              | mov r9d,1                                       |
        00007FF63317CE4D | 48 8B 49 10                    | mov rcx,qword ptr ds:[rcx+10]                   |
        00007FF63317CE51 | 48 89 4C 24 30                 | mov qword ptr ss:[rsp+30],rcx                   |
        00007FF63317CE56 | 48 85 C9                       | test rcx,rcx                                    |
        00007FF63317CE59 | 74 11                          | je pathofexile_x64 - alpha 2.5.7FF63317CE6C     |
        00007FF63317CE5B | 41 8B C1                       | mov eax,r9d                                     |
        00007FF63317CE5E | F0 0F C1 41 54                 | lock xadd dword ptr ds:[rcx+54],eax             |
        00007FF63317CE63 | 8B 05 7B 09 F0 00              | mov eax,dword ptr ds:[<AreaChangeCount>]        |
        00007FF63317CE69 | 89 41 50                       | mov dword ptr ds:[rcx+50],eax                   |
        00007FF63317CE6C | 49 8B 08                       | mov rcx,qword ptr ds:[r8]                       |
        00007FF63317CE6F | 49 8B 40 18                    | mov rax,qword ptr ds:[r8+18]                    |
        */

        private static readonly Pattern AreaChangePattern =
            new Pattern(
                new byte[]
                {
                    0xE8, 
                    0x00, 0x00, 0x00, 0x00, 
                    0xE8, 
                    0x00, 0x00, 0x00, 0x00, 
                    0xFF, 0x05
                }, "x????x????xx", "Area change", 9430000);

        /*
        PathOfExile_x64.exe+118FD9 - 4C 8B 35 48255B01     - mov r14,[PathOfExile_x64.exe+16CB528] { [C6151734A0] }<<here
        PathOfExile_x64.exe+118FE0 - 4D 85 F6              - test r14,r14
        PathOfExile_x64.exe+118FE3 - 0F94 C0               - sete al
        PathOfExile_x64.exe+118FE6 - 84 C0                 - test al,al
        */

        private static readonly Pattern GameStatePattern = new Pattern(
            new byte[]
            {
	            0x48, 0x8b, 0xf9, 0x33, 0xed, 0x48, 0x39, 0x2d , 0x00, 0x00, 0x00, 0x00, 0x0f, 0x85, 0x1a,
            }, "xxxxxxxx????xxx", "Game State", 1240000);

        public long AreaChangeCount { get; private set; }
        public long Base { get; private set; }
        public string ExeName { get; private set; }
        public long FileRoot { get; private set; }
        public int IgsDelta { get; private set; }
        public int IgsOffset { get; private set; }
        public int IgsOffsetDelta => IgsOffset + IgsDelta;
        public long isLoadingScreenOffset { get; private set; }
        public long GameStateOffset { get; private set; }

        public Dictionary<OffsetsName, long> DoPatternScans(IMemory m)
        {
            var array = m.FindPatterns(FileRootPattern, AreaChangePattern, GameStatePattern);

            var result = new Dictionary<OffsetsName, long>();

            //  System.Console.WriteLine("Base Pattern: " + (m.AddressOfProcess + array[0]).ToString("x8"));

            var index = 0;
            var baseAddress = m.Process.MainModule.BaseAddress.ToInt64();

            FileRoot = m.Read<int>(baseAddress + array[index] + 6) + array[index] + 10;
            index++;
            //FileRoot = 0x362CCC0; 3.11.0c

            AreaChangeCount = m.Read<int>(baseAddress + array[index] + 0xC) + array[index] + 0x10;
            index++;
            //AreaChangeCount = 0x336AA88; 3.11.0f


            GameStateOffset = m.Read<int>(baseAddress + array[index] + 8) + array[index] + 12;

            result.Add(OffsetsName.FileRoot, FileRoot);
            result.Add(OffsetsName.AreaChangeCount, AreaChangeCount);
            result.Add(OffsetsName.GameStateOffset, GameStateOffset);
            return result;
        }
    }
}
