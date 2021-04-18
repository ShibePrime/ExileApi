using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.Components
{
    public class MinimapIcon : Component
    {
        private FrameCache<MinimapIconOffsets> cachedValue;
        private MinimapIconOffsets MinimapIconOffsets => cachedValue.Value;
        public bool IsVisible => MinimapIconOffsets.IsVisible == 0; //M.Read<byte>(Address + 0x30) == 0;
        public bool IsHide => MinimapIconOffsets.IsHide == 1; //M.Read<byte>(Address + 0x34) == 1;
        public string Name => NativeStringReader.ReadString(MinimapIconOffsets.NamePtr, M); 
        protected override void OnAddressChange() 
        {
            cachedValue = new FrameCache<MinimapIconOffsets>(() => M.Read<MinimapIconOffsets>(Address));
        }
    }
}
