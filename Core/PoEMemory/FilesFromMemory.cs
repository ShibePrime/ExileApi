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
        private readonly IMemory mem;

        public FilesFromMemory(IMemory memory)
        {
            mem = memory;
        }

        public Dictionary<string, FileInformation> GetAllFiles()
        {
            var files = new ConcurrentDictionary<string, FileInformation>();
            var fileRoot = mem.AddressOfProcess + mem.BaseOffsets[OffsetsName.FileRoot];

            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 128;
            Parallel.For(0, 128, parallelOptions, i =>
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
                {
                    // Ignore those errors for now, there seems to be no pattern to them
                    //DebugWindow.LogError($"FilesFromMemory -> ReadDictionary error. Already contains key: {key}. Value: {node.Value:X}");
                }
                else
                {
                    dictionary[key] = new FileInformation(node.Value, advancedInformation.AreaCount);
                }

                node = mem.Read<FileNode>(node.Next);
            }
        }
    }
}
