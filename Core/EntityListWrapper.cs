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
        public event Action<Entity> EntityAdded;
        public event Action<Entity> EntityRemoved;
        public event EventHandler<Entity> PlayerUpdate;

        private readonly CoreSettings _settings;
        private readonly GameController _gameController;
        private readonly Coroutine _parallelUpdateDictionary;
        private static EntityListWrapper _instance;

        public Entity Player { get; private set; }
        public IDictionary<uint, Entity> EntityCache => _gameController.Game.IngameState.Data.EntityList.EntityCache;
        public ICollection<Entity> Entities => EntityCache.Values;
        public IDictionary<EntityType, List<Entity>> ValidEntitiesByType { get; private set; }
        public IDictionary<uint, Entity> OnlyValidEntitiesWithId { get; private set; }
        public ICollection<Entity> OnlyValidEntities => OnlyValidEntitiesWithId.Values;
        public IDictionary<uint, Entity> NotValidEntitiesWithId { get; private set; }
        public ICollection<Entity> NotValidEntities => NotValidEntitiesWithId.Values;

        [Obsolete("Use 'EntityAdded' instead. Going to be removed in 3.15")]
        public event Action<Entity> EntityAddedAny;
        [Obsolete("Going to be removed in 3.15")]
        public event Action<Entity> EntityIgnored;
        [Obsolete("Use 'NotValidEntitiesWithId' instead. Going to be removed in 3.15")]
        public Dictionary<uint, Entity> NotValidDict => (Dictionary<uint, Entity>)NotValidEntitiesWithId;
        [Obsolete("Going to be removed in 3.15")]
        public List<Entity> NotOnlyValidEntities { get; private set; } = new List<Entity>(500);


        public EntityListWrapper(GameController gameController, CoreSettings settings, MultiThreadManager multiThreadManager)
        {
            _instance = this;
            _gameController = gameController;
            _settings = settings;
            _gameController.Area.OnAreaChange += AreaChanged;
            _parallelUpdateDictionary = new Coroutine(CollectEntities(), null, "Collect Entities") {SyncModWork = true};

            _settings.EntitiesUpdate.OnValueChanged += (sender, i) => { UpdateCondition(1000 / i); };
            UpdateCondition(1000 / _settings.EntitiesUpdate);

            PlayerUpdate += (sender, entity) => Entity.Player = entity;

            InitializeCollections();
        }

        private IEnumerator CollectEntities()
        {
            while (true)
            {
                yield return _gameController.IngameState.Data.EntityList.CollectEntities(
                    _settings.ParseServerEntities?.Value ?? false,
                    EntityRemoved,
                    EntityAdded
                    );
                UpdateEntityCollections();
                yield return new WaitTime(1000 / _settings.EntitiesUpdate);
                _parallelUpdateDictionary.UpdateTicks((uint)(_parallelUpdateDictionary.Ticks + 1));

            }
        }

        public void StartWork()
        {
            Core.ParallelRunner.Run(_parallelUpdateDictionary);
        }

        private void UpdateCondition(int coroutineTimeWait = 50)
        {
            _parallelUpdateDictionary.UpdateCondtion(new WaitTime(coroutineTimeWait));
        }

        private void AreaChanged(AreaInstance area)
        {
            try
            {
                ClearGeneratedColletions();
                EntityCache.Clear();

                var dataLocalPlayer = _gameController.Game.IngameState.Data.LocalPlayer;

                if (Player != null && Player.Address == dataLocalPlayer.Address) return;
                if (!dataLocalPlayer.Path.StartsWith("Meta")) return;
             
                Player = dataLocalPlayer;
                Player.IsValid = true;
                PlayerUpdate?.Invoke(this, Player);
            }
            catch (Exception e)
            {
                DebugWindow.LogError($"EntityListWrapper.AreaChanged -> {e}");
            }
        }

        private void InitializeCollections()
        {
            var enumValues = typeof(EntityType).GetEnumValues();
            ValidEntitiesByType = new Dictionary<EntityType, List<Entity>>(enumValues.Length);
            OnlyValidEntitiesWithId = new Dictionary<uint, Entity>(2048);
            NotValidEntitiesWithId = new Dictionary<uint, Entity>(2048);

            foreach (EntityType enumValue in enumValues)
            {
                ValidEntitiesByType[enumValue] = new List<Entity>(8);
            }
        }

        private void UpdateEntityCollections()
        {
            ClearGeneratedColletions();

            foreach (var entity in EntityCache)
            {
                if (entity.Value == null) continue;
                if (entity.Value.IsValid)
                {
                    OnlyValidEntitiesWithId.Add(entity);
                    ValidEntitiesByType[entity.Value.Type].Add(entity.Value);
                }
                else
                {
                    NotValidEntitiesWithId.Add(entity);
                }
            }
        }

        private void ClearGeneratedColletions()
        {
            OnlyValidEntitiesWithId?.Clear();
            NotValidEntitiesWithId?.Clear();

            foreach (var e in ValidEntitiesByType)
            {
                e.Value?.Clear();
            }
        }

        public static Entity GetEntityById(uint id)
        {
            return _instance.EntityCache.TryGetValue(id, out var result) ? result : null;
        }

        // TODO why is this here?
        [Obsolete("I dont see any reason to keep this, probably removed in 3.15")]
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
