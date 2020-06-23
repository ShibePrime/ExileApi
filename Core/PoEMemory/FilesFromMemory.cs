using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using MoreLinq;

namespace ExileCore.PoEMemory
{
    public struct FileInformation
    {
        public FileInformation(long ptr, int changeCount, int test1, int test2)
        {
            Ptr = ptr;
            ChangeCount = changeCount;
            Test1 = test1;
            Test2 = test2;
        }

        public long Ptr { get; }
        public int ChangeCount { get; }
        public int Test1 { get; }
        public int Test2 { get; }
    }

    public class FilesFromMemory
    {
        private readonly IMemory mem;

        public FilesFromMemory(IMemory memory)
        {
            mem = memory;
        }

        public Dictionary<string, FileInformation> GetAllFiles()
        {
            var files = new ConcurrentDictionary<string, FileInformation>();
            var fileRoot = mem.AddressOfProcess + mem.BaseOffsets[OffsetsName.FileRoot];

            Parallel.For(0, 128, i =>
            {
                var addr = fileRoot + i * 0x40;
                var fileChunkStruct = mem.Read<FilesOffsets>(addr);

                ReadDictionary(fileChunkStruct.ListPtr, files);
            });

            return files.ToDictionary();
        }

        public void ReadDictionary(long head, ConcurrentDictionary<string, FileInformation> dictionary)
        {
            var node = mem.Read<FileNode>(head);

            var headLong = head;

            // first node in list does not contain file information
            node = mem.Read<FileNode>(node.Next);

            var maxCount = 1000;    // not seeing more than that amount of files in one bucket

            while (headLong != node.Next)
            {
                if (0 == --maxCount)
                    return;

                var advancedInformation = mem.Read<GameOffsets.FileInformation>(node.Value);
                if (advancedInformation.String.buf == 0) return;

                var key = mem.ReadStringU(node.Key);

                if (dictionary.ContainsKey(key))
                    Core.Logger.Error($"ReadDictionary error. Already contains key: {key}. Value: {node.Value:X}");
                else
                    dictionary[key] = new FileInformation(node.Value, advancedInformation.AreaCount, advancedInformation.Test1, advancedInformation.Test2);

                node = mem.Read<FileNode>(node.Next);
            }
        }
    }
}
