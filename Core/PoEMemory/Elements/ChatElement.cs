using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements
{
    public class ChatElement : Element
    {
        // Structure of Chatbox is
        //Root > [x] ChatBox Root Panel > [0] Toggle Panel     > [0] Channel Bar Panel
        //                                                     > [1] Hide ChatBox Button Panel
        //                              > [1] History Panel    > [0] Feature Placeholder
        //                                                     > [1] Feature Placeholder
        //                                                     > [2] Chat Body Panel  > [0] Feature Placeholder
        //                                                                            > [1] Chat History Panel
        //                                                     > [3] Chat Scroll Panel
        //                              > [2] Target Channel Panel
        //                              > [3] Text Input Panel
        //                              > [4] Autocomplete Panel
        //

        private readonly CachedValue<ChatElementOffsets> _cacheChatElementOffsets;

        public ChatElement()
        {
            _cacheChatElementOffsets = new FrameCache<ChatElementOffsets>(() => Address == 0 ? default : M.Read<ChatElementOffsets>(Address));
        }

        public new bool IsVisibleLocal => (_cacheChatElementOffsets.Value.IsVisibleLocal & 8) == 8;
        public new bool IsVisible => base.IsVisible && IsVisibleLocal;
        public new bool IsHighlighted => false; // chat is never highlighted
        public bool IsActive => _cacheChatElementOffsets.Value.IsActive;

        public Element MessageBox => GetChildAtIndex(1)?.GetChildAtIndex(2)?.GetChildAtIndex(1);
        public long TotalMessageCount => MessageBox?.ChildCount ?? 0;
        public new EntityLabel this[int index]
        {
            get
            {
                if (index < TotalMessageCount)
                    return MessageBox.GetChildAtIndex(index).AsObject<EntityLabel>();

                return null;
            }
        }
    }
}
