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

        private readonly CachedValue<ChatElementOffsets> _ChatElement;

        public ChatElement()
        {
            _ChatElement = new FrameCache<ChatElementOffsets>(() => Address == 0 ? default : M.Read<ChatElementOffsets>(Address));
        }


        public new bool IsVisibleLocal => GetChildAtIndex(0).IsVisibleLocal;
        public new bool IsVisible => GetChildAtIndex(0).IsVisible;
        public new bool IsHighlighted => false; // chat is never highlighted
        public bool IsActive => _ChatElement.Value.IsActive;

        public Element MessageBox => GetChildAtIndex(1)?.GetChildAtIndex(2)?.GetChildAtIndex(1);
        public long TotalMessageCount => MessageBox?.ChildCount ?? 0;
        public new EntityLabel this[int index] => index < TotalMessageCount ? MessageBox.GetChildAtIndex(index).AsObject<EntityLabel>() : null;
    }
}
