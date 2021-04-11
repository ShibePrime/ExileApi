using System.Collections.Generic;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;

namespace ExileCore.PoEMemory
{
    public readonly struct FileInformation
    {
        public FileInformation(long ptr, int changeCount)
        {
            Ptr = ptr;
            ChangeCount = changeCount;
        }

        public long Ptr { get; }
        public int ChangeCount { get; }
    }

    public class FilesFromMemory
    {
        private readonly IMemory _mem;

        public FilesFromMemory(IMemory memory)
        {
            _mem = memory;
        }

        public Dictionary<string, FileInformation> GetAllFiles()
        {
            var files = new Dictionary<string, FileInformation>();
            var fileRootAddress = _mem.AddressOfProcess + _mem.BaseOffsets[OffsetsName.FileRoot];
            for (int rbIndex = 0; rbIndex < 16; rbIndex++)
            {
                var fileRootBlock = _mem.Read<FileRootBlock>(fileRootAddress + rbIndex * 0x28);
                for (int bIndex = 0; bIndex < (fileRootBlock.Capacity + 1) / 8; bIndex++)
                {
                    var basePtr = fileRootBlock.FileNodesPtr + bIndex * 0xc8;
                    var hasValues = _mem.ReadBytes(basePtr, 8);
                    for (int index = 0; index < 8; index++)
                    {
                        if (hasValues[index] == 0xFF) continue;
                         
                        var fileEntryPtr = _mem.Read<long>(basePtr + 8 + index * 0x18 + 8);
                        var fileInfo = _mem.Read<GameOffsets.FileInformation>(fileEntryPtr);
                        var key = _mem.ReadStringU(fileInfo.String);
                        files[key] = new FileInformation(fileEntryPtr, fileInfo.AreaCount);
                    }                        
                }
            }
            return files;
        }
    }
}
