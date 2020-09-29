using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using MoreLinq;

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
            var files = new ConcurrentDictionary<string, FileInformation>();
            var fileRootAddress = _mem.AddressOfProcess + _mem.BaseOffsets[OffsetsName.FileRoot];
            Parallel.For(0, 128, i => {
                var fileAddress = fileRootAddress + i * 0x40;
                var fileChunk = _mem.Read<FilesOffsets>(fileAddress);
                ReadDictionary(fileChunk.ListPtr, files);
            });
            return files.ToDictionary();
        }

        public void ReadDictionary(long head, ConcurrentDictionary<string, FileInformation> dictionary)
        {
            var node = _mem.Read<FileNode>(head);
            var sw = Stopwatch.StartNew();
            
            while (head != node.Next)
            {
                if (sw.ElapsedMilliseconds > 2000)
                {
                    DebugWindow.LogError($"FilesFromMemory -> ReadDictionary(...) timeout error. Elapsed: {sw.ElapsedMilliseconds}");
                    break;
                }

                var key = _mem.ReadStringU(node.Key);
                
                if (!dictionary.TryGetValue(key, out _))
                {
                    var areaChangeCount = _mem.Read<int>(node.Value + 0x38);
                    dictionary[key] = new FileInformation(node.Value, areaChangeCount);
                }

                node = _mem.Read<FileNode>(node.Next);
            }
        }

        public Dictionary<string, FileInformation> GetAllFilesSync()
        {
            var files = new ConcurrentDictionary<string, FileInformation>();
            var fileRootAddress = _mem.AddressOfProcess + _mem.BaseOffsets[OffsetsName.FileRoot];
            Parallel.For(0, 128, i => {
                var fileAddress = fileRootAddress + i * 0x40;
                var fileChunk = _mem.Read<FilesOffsets>(fileAddress);
                ReadDictionary(fileChunk.ListPtr, files);
            });
            return files.ToDictionary();
        }
    }
}
