using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;

namespace ExileCore.PoEMemory.MemoryObjects
{

    public class WorldData : RemoteMemoryObject
    {
        private static readonly int CameraOffset = Extensions.GetOffset<WorldDataOffsets>(nameof(WorldDataOffsets.CameraStruct));
        private readonly CachedValue<WorldDataOffsets> _worldData;
        private readonly CachedValue<AreaTemplate> _CurrentArea;
        private readonly CachedValue<WorldArea> _CurrentWorldArea;
        private readonly CachedValue<Camera> _camera;

        public WorldData()
        {
            _worldData = new FrameCache<WorldDataOffsets>(() => M.Read<WorldDataOffsets>(Address));
            _CurrentArea = new AreaCache<AreaTemplate>(() => GetObject<AreaTemplate>(M.Read<WorldAreaDetailsOffsets>(_worldData.Value.WorldAreaDetails).AreaTemplate));
            _CurrentWorldArea = new AreaCache<WorldArea>(() => TheGame.Files.WorldAreas.GetByAddress(CurrentArea.Address));
            _camera = new AreaCache<Camera>(() => GetObject<Camera>(Address + CameraOffset));
        }

        public Camera Camera => _camera.Value;
        public AreaTemplate CurrentArea => _CurrentArea.Value;
        public WorldArea CurrentWorldArea => _CurrentWorldArea.Value;
    }

};
