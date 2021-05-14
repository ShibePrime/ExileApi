using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Nodes;

namespace ExileCore
{
    public class EntityListWrapper
    {
        private readonly CoreSettings _settings;
        private readonly GameController _gameController;
        private readonly Queue<uint> keysForDelete = new Queue<uint>(24);

        private readonly Coroutine parallelUpdateDictionary;
        private readonly Stack<Entity> Simple = new Stack<Entity>(512);
        private static EntityListWrapper _instance;
        public EntityListWrapper(GameController gameController, CoreSettings settings, MultiThreadManager multiThreadManager)
        {
            _instance = this;
            _gameController = gameController;
            _settings = settings;

            gameController.Area.OnAreaChange += AreaChanged;

            //updateEntity = new Coroutine(RefreshState, new WaitTime(coroutineTimeWait), null, "Update Entity")
            //        {Priority = CoroutinePriority.High, SyncModWork = true};

            var collectEntitiesDebug = new DebugInformation("Collect Entities");

            IEnumerator Test()
            {
                while (true)
                {
                    yield return gameController.IngameState.Data.EntityList.CollectEntities(
                        _settings.ParseServerEntities?.Value ?? false
                        );
                    UpdateEntityCollections();
                    yield return new WaitTime(1000 / settings.EntitiesUpdate);
                    parallelUpdateDictionary.UpdateTicks((uint) (parallelUpdateDictionary.Ticks + 1));

                }
            }

            parallelUpdateDictionary = new Coroutine(Test(), null, "Collect entites") {SyncModWork = true};
            UpdateCondition(1000 / settings.EntitiesUpdate);

            settings.EntitiesUpdate.OnValueChanged += (sender, i) => { UpdateCondition(1000 / i); };

            var enumValues = typeof(EntityType).GetEnumValues();
            ValidEntitiesByType = new Dictionary<EntityType, List<Entity>>(enumValues.Length);

            foreach (EntityType enumValue in enumValues)
            {
                ValidEntitiesByType[enumValue] = new List<Entity>(8);
            }

            PlayerUpdate += (sender, entity) => Entity.Player = entity;
        }

        public IDictionary<uint, Entity> EntityCache => _gameController.Game.IngameState.Data.EntityList.EntityCache;
        public ICollection<Entity> Entities => EntityCache.Values;
        public Entity Player { get; private set; }
        public List<Entity> OnlyValidEntities { get; } = new List<Entity>(500);
        public List<Entity> NotOnlyValidEntities { get; } = new List<Entity>(500);
        public Dictionary<uint, Entity> NotValidDict { get; } = new Dictionary<uint, Entity>(500);
        public Dictionary<EntityType, List<Entity>> ValidEntitiesByType { get; }

        public void StartWork()
        {
            Core.ParallelRunner.Run(parallelUpdateDictionary);
        }

        private void UpdateCondition(int coroutineTimeWait = 50)
        {
            parallelUpdateDictionary.UpdateCondtion(new WaitTime(coroutineTimeWait));
        }

#pragma warning disable CS0067
        public event Action<Entity> EntityAdded;
        public event Action<Entity> EntityAddedAny;
        public event Action<Entity> EntityIgnored;
        public event Action<Entity> EntityRemoved;
#pragma warning restore CS0067

        private void AreaChanged(AreaInstance area)
        {
            try
            {
                var dataLocalPlayer = _gameController.Game.IngameState.Data.LocalPlayer;

                if (Player == null)
                {
                    if (dataLocalPlayer.Path.StartsWith("Meta"))
                    {
                        Player = dataLocalPlayer;
                        Player.IsValid = true;
                        PlayerUpdate?.Invoke(this, Player);
                    }
                }
                else
                {
                    if (Player.Address != dataLocalPlayer.Address)
                    {
                        if (dataLocalPlayer.Path.StartsWith("Meta"))
                        {
                            Player = dataLocalPlayer;
                            Player.IsValid = true;
                            PlayerUpdate?.Invoke(this, Player);
                        }
                    }
                }

                OnlyValidEntities.Clear();
                NotOnlyValidEntities.Clear();

                foreach (var e in ValidEntitiesByType)
                {
                    e.Value.Clear();
                }
            }
            catch (Exception e)
            {
                DebugWindow.LogError($"{nameof(EntityListWrapper)} -> {e}");
            }
        }

        private void UpdateEntityCollections()
        {
            OnlyValidEntities.Clear();
            NotOnlyValidEntities.Clear();
            NotValidDict.Clear();

            foreach (var e in ValidEntitiesByType)
            {
                e.Value.Clear();
            }

            while (keysForDelete.Count > 0)
            {
                var key = keysForDelete.Dequeue();

                if (EntityCache.TryGetValue(key, out var entity))
                {
                    EntityRemoved?.Invoke(entity);
                    EntityCache.Remove(key);
                }
            }

            foreach (var entity in EntityCache)
            {
                var entityValue = entity.Value;

                if (entityValue.IsValid)
                {
                    OnlyValidEntities.Add(entityValue);
                    ValidEntitiesByType[entityValue.Type].Add(entityValue);
                }
                else
                {
                    NotOnlyValidEntities.Add(entityValue);
                    NotValidDict[entityValue.Id] = entityValue;

                }
            }
        }

        public void RefreshState()
        {
            if (_gameController.Area.CurrentArea == null) return;
            if (Player == null || !Player.IsValid) return;

            while (Simple.Count > 0)
            {
                var entity = Simple.Pop();

                if (entity == null)
                {
                    DebugWindow.LogError($"{nameof(EntityListWrapper)}.{nameof(RefreshState)} entity is null. (Very strange).");
                    continue;
                }

                var entityId = entity.Id;
                if (EntityCache.TryGetValue(entityId, out _)) continue;

                if (entityId >= int.MaxValue && !_settings.ParseServerEntities)
                    continue;

                if (entity.Type == EntityType.Error)
                    continue;

                EntityAddedAny?.Invoke(entity);
                if ((int) entity.Type >= 100) EntityAdded?.Invoke(entity);

                EntityCache[entityId] = entity;
            }

            UpdateEntityCollections();
        }

        public event EventHandler<Entity> PlayerUpdate;

        public static Entity GetEntityById(uint id)
        {
            return _instance.EntityCache.TryGetValue(id, out var result) ? result : null;
        }

        public string GetLabelForEntity(Entity entity)
        {
            var hashSet = new HashSet<long>();
            var entityLabelMap = _gameController.Game.IngameState.EntityLabelMap;
            var num = entityLabelMap;

            while (true)
            {
                hashSet.Add(num);
                if (_gameController.Memory.Read<long>(num + 0x10) == entity.Address) break;

                num = _gameController.Memory.Read<long>(num);
                if (hashSet.Contains(num) || num == 0 || num == -1) return null;
            }

            return _gameController.Game.ReadObject<EntityLabel>(num + 0x18).Text;
        }
    }
}
