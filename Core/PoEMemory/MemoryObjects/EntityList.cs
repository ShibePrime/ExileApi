using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using GameOffsets;
using JM.LinqFaster;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class EntityList : RemoteMemoryObject
    {
        public ConcurrentDictionary<uint, Entity> EntityCache { get; } 
        private readonly Queue<uint> _entityIdsToDelete = new Queue<uint>(128);

        private readonly List<long> _entityListAddresses = new List<long>();
        private readonly List<long> _entityAddresses = new List<long>();
        private readonly List<long> _entityIds = new List<long>();

        public EntityList()
        {
            EntityCache = new ConcurrentDictionary<uint, Entity>(8, 2048);
        }

        public IEnumerator CollectEntities(
            bool parseServerEntities,
            Action<Entity> entityRemoved)
        {
            if (Address == 0)
            {
                DebugWindow.LogError($"{nameof(EntityList)} -> Address is 0;");
                yield return new WaitTime(100);
            }
            _entityListAddresses.Clear();
            _entityAddresses.Clear();
            _entityIds.Clear();

            var address = M.Read<long>(Address + 0x8);
            _entityListAddresses.Add(address);

            var node = M.Read<EntityListOffsets>(address);
            GatherEntityAddressesRecursive(node);

            foreach (var entityAddress in _entityAddresses)
            {
                var entityId = M.Read<uint>(entityAddress + 0x60);
                if (entityId <= 0) continue;
                if (entityId >= int.MaxValue && !parseServerEntities) continue;
                _entityIds.Add(entityId);
                if (EntityCache.ContainsKey(entityId))
                {
                    EntityCache[entityId].IsValid = true;
                    continue;
                }

                var entity = GetObject<Entity>(entityAddress);
                if(!EntityCache.TryAdd(entityId, entity))
                {
                    DebugWindow.LogError($"Unable to add entity to list, id: {entityId}");
                }
            }

            GatherEntityIdsToDelete();
            RemoveEntities(entityRemoved);
        }

        private void RemoveEntities(Action<Entity> entityRemoved)
        {
            while (_entityIdsToDelete.Count > 0)
            {
                var entityIdToDelete = _entityIdsToDelete.Dequeue();
                if (!EntityCache.TryRemove(entityIdToDelete, out Entity removedEntity))
                {
                    DebugWindow.LogError($"Unable to remove entity from list, id: {entityIdToDelete}");
                    return;
                };
                entityRemoved?.Invoke(removedEntity);
            }
        }

        private void GatherEntityIdsToDelete()
        {
            foreach (var entity in EntityCache)
            {
                if (_entityIds.Contains(entity.Key))
                {
                    entity.Value.IsValid = true;
                }
                else
                {
                    _entityIdsToDelete.Enqueue(entity.Key);
                }

                if (entity.Value.DistancePlayer > 1000) _entityIdsToDelete.Enqueue(entity.Key);
            }
        }

        private void GatherEntityAddressesRecursive(EntityListOffsets node)
        {
            _entityAddresses.Add(node.Entity);

            if (!_entityListAddresses.Contains(node.FirstAddr))
            {
                _entityListAddresses.Add(node.FirstAddr);
                GatherEntityAddressesRecursive(M.Read<EntityListOffsets>(node.FirstAddr));
            }

            if (!_entityListAddresses.Contains(node.SecondAddr))
            {
                _entityListAddresses.Add(node.SecondAddr);
                GatherEntityAddressesRecursive(M.Read<EntityListOffsets>(node.SecondAddr));
            }
        }
    }
}
