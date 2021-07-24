using System.Collections.Generic;
using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
    public class ItemsOnGroundLabelElement : Element
    {
        public Element LabelOnHover
        {
            get
            {
                var readObjectAt = ReadObjectAt<Element>(0x250);
                return readObjectAt.Address == 0 ? null : readObjectAt;
            }
        }

        public Entity ItemOnHover
        {
            get
            {
                var readObjectAt = ReadObjectAt<Entity>(0x258);
                return readObjectAt.Address == 0 ? null : readObjectAt;
            }
        }

        public string ItemOnHoverPath => ItemOnHover != null ? ItemOnHover.Path : "Null";
        public string LabelOnHoverText => LabelOnHover != null ? LabelOnHover.Text : "Null";
        public int CountLabels => M.Read<int>(Address + 0x270);
        public int CountLabels2 => M.Read<int>(Address + 0x2B0);

        public List<LabelOnGround> LabelsOnGround
        {
            get
            {
                var address = M.Read<long>(Address + 0x2A8);

                var result = new List<LabelOnGround>();

                if (address <= 0)
                    return null;

                var limit = 0;

                for (var nextAddress = M.Read<long>(address); nextAddress != address; nextAddress = M.Read<long>(nextAddress))
                {
                    var labelOnGround = GetObject<LabelOnGround>(nextAddress);

                    if (labelOnGround?.Label?.IsValid ?? false)
                        result.Add(labelOnGround);

                    limit++;

                    if (limit > 100000)
                        return null;
                }

                return result;
            }
        }
    }
}
