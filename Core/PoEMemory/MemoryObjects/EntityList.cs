using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExileCore.Shared;
using GameOffsets;
using JM.LinqFaster;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class EntityList : RemoteMemoryObject
    {
        public ConcurrentDictionary<uint, Entity> EntityCache { get; } 
        private readonly Queue<uint> _entityIdsToDelete = new Queue<uint>(128);

        private readonly List<long> _entityListAddresses = new List<long>(2048);
        private readonly List<long> _entityAddresses = new List<long>(2048);
        private ConcurrentBag<long> _entityIds = new ConcurrentBag<long>();

        public EntityList()
        {
            EntityCache = new ConcurrentDictionary<uint, Entity>(8, 2048);
        }

        public IEnumerator CollectEntities(
            bool parseServerEntities,
            ParallelOptions parallelOptions,
            Action<Entity> entityRemoved,
            Action<Entity> entityAdded)
        {
            if (Address == 0)
            {
                if(pTheGame.InGame) {
                    DebugWindow.LogError($"{nameof(EntityList)} -> Address is 0;");
                }
                yield return new WaitTime(100);
            }
            _entityListAddresses.Clear();
            _entityAddresses.Clear();
            _entityIds = new ConcurrentBag<long>();

            var address = M.Read<long>(Address + 0x8);
            _entityListAddresses.Add(address);

            var node = M.Read<EntityListOffsets>(address);
            GatherEntityAddressesRecursive(node);

            Parallel.ForEach(_entityAddresses, parallelOptions, entityAddress =>
            {
                var entityId = M.Read<uint>(entityAddress + 0x60);
                if (entityId <= 0) return;
                if (entityId >= int.MaxValue && !parseServerEntities) return;
                _entityIds.Add(entityId);
                if (EntityCache.ContainsKey(entityId))
                {
                    EntityCache[entityId].IsValid = true;
                    return;
                }

                var entity = GetObject<Entity>(entityAddress);
                if (!EntityCache.TryAdd(entityId, entity))
                {
                    DebugWindow.LogError($"Unable to add entity to list, id: {entityId}");
                }
                entityAdded?.Invoke(entity);
            });

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
                    continue;
                };
                entityRemoved?.Invoke(removedEntity);
            }
        }

        private void GatherEntityIdsToDelete()
        {
            var entityIdsSnapshot = _entityIds.ToArray();
            foreach (var entity in EntityCache)
            {
                if (entityIdsSnapshot.ContainsF(entity.Key))
                {
                    entity.Value.IsValid = true;
                }
                else
                {
                    entity.Value.IsValid = false;
                    _entityIdsToDelete.Enqueue(entity.Key);
                    continue;
                }

                if (entity.Value.DistancePlayer > 1000)
                {
                    _entityIdsToDelete.Enqueue(entity.Key);
                    continue;
                }
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
