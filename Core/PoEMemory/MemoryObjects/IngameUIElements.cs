using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects.Metamorph;
using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class IngameUIElements : Element
    {
        private Element _BetrayalWindow;
        private readonly CachedValue<IngameUElementsOffsets> _cachedValue;
        private Element _CraftBench;
        private Cursor _cursor;
        private SubterraneanChart _DelveWindow;
        private IncursionWindow _IncursionWindow;
        private Map _map;
        private Element _purchaseWindow;
        private Element _SynthesisWindow;
        private Element _UnveilWindow;
        private Element _ZanaMissionChoice;
        private readonly CachedValue<List<QuestState>> _cachedQuestStates;


        public IngameUIElements()
        {
            _cachedValue = new FrameCache<IngameUElementsOffsets>(() => M.Read<IngameUElementsOffsets>(Address));
            _cachedQuestStates = new TimeCache<List<QuestState>>(GenerateQuestStates, 1000);
        }

        public IngameUElementsOffsets IngameUIElementsStruct => _cachedValue.Value;
        public GameUi GameUI => GetObject<GameUi>(IngameUIElementsStruct.GameUI);
        public SellWindow SellWindow => GetObject<SellWindow>(IngameUIElementsStruct.SellWindow);
        public TradeWindow TradeWindow => GetObject<TradeWindow>(IngameUIElementsStruct.TradeWindow);
        public NpcDialog NpcDialog => GetObject<NpcDialog>(IngameUIElementsStruct.NpcDialog);
        public BanditDialog BanditDialog => GetObject<BanditDialog>(IngameUIElementsStruct.BanditDialog);
        public Element PurchaseWindow => _purchaseWindow ??= GetObject<Element>(IngameUIElementsStruct.PurchaseWindow);
        public SubterraneanChart DelveWindow => _DelveWindow ??= GetObject<SubterraneanChart>(IngameUIElementsStruct.DelveWindow);
        public SkillBarElement SkillBar => GetObject<SkillBarElement>(IngameUIElementsStruct.SkillBar);
        public SkillBarElement HiddenSkillBar => GetObject<SkillBarElement>(IngameUIElementsStruct.HiddenSkillBar);
        public ChatElement ChatBoxRoot => GetObject<ChatElement>(IngameUIElementsStruct.ChatPanel);
        [Obsolete("Use ChatBoxRoot?.MessageBox instead")]
        public Element ChatBox => ChatBoxRoot?.MessageBox;
        [Obsolete("Use ChatBoxRoot?.MessageBox?.Children.Select(x => x.Text).ToList() instead")]
        public IList<string> ChatMessages => ChatBox?.Children.Select(x => x.Text).ToList();
        public Element QuestTracker => GetObject<Element>(IngameUIElementsStruct.QuestTracker);
        public QuestRewardWindow QuestRewardWindow => GetObject<QuestRewardWindow>(IngameUIElementsStruct.QuestRewardWindow);
        public Element OpenLeftPanel => GetObject<Element>(IngameUIElementsStruct.OpenLeftPanel);
        public Element OpenRightPanel => GetObject<Element>(IngameUIElementsStruct.OpenRightPanel);
        public StashElement StashElement => GetObject<StashElement>(IngameUIElementsStruct.StashElement);
        public InventoryElement InventoryPanel => GetObject<InventoryElement>(IngameUIElementsStruct.InventoryPanel);
        public Element TreePanel => GetChildAtIndex(24);
        public Element PVPTreePanel => GetChildAtIndex(25);
        public Element AtlasPanel => GetObject<Element>(IngameUIElementsStruct.AtlasPanel);
        public Element Atlas => AtlasPanel; // Required to fit with TehCheats Api, Random Feature uses this field.
        public Map Map => _map ??= GetObject<Map>(IngameUIElementsStruct.Map);
        public ItemsOnGroundLabelElement ItemsOnGroundLabelElement => GetObject<ItemsOnGroundLabelElement>(IngameUIElementsStruct.itemsOnGroundLabelRoot);
        public IList<LabelOnGround> ItemsOnGroundLabels => ItemsOnGroundLabelElement.LabelsOnGround;
        public IList<LabelOnGround> ItemsOnGroundLabelsVisible => ItemsOnGroundLabelElement.LabelsOnGround.Where(x => x.Address != 0 && x.IsVisible).ToList();
        public GemLvlUpPanel GemLvlUpPanel => GetObject<GemLvlUpPanel>(IngameUIElementsStruct.GemLvlUpPanel);
        public Element InvitesPanel => GetObject<Element>(IngameUIElementsStruct.InvitesPanel);
        public ItemOnGroundTooltip ItemOnGroundTooltip => GetObject<ItemOnGroundTooltip>(IngameUIElementsStruct.ItemOnGroundTooltip);
        public MapStashTabElement MapStashTab => ReadObject<MapStashTabElement>(IngameUIElementsStruct.MapTabWindowStartPtr);
        public Element Sulphit => GetObject<Element>(IngameUIElementsStruct.Map).GetChildAtIndex(0);
        public Cursor Cursor => _cursor ??= GetObject<Cursor>(IngameUIElementsStruct.Mouse);
        public Element BetrayalWindow => _BetrayalWindow ??= GetObject<Element>(IngameUIElementsStruct.BetrayalWindow);
        public Element SyndicatePanel => BetrayalWindow; // Required for TehCheats Api, BroodyHen uses this.
        public Element SyndicateTree => GetObject<Element>(M.Read<long>(BetrayalWindow.Address + 0xA50));
        public Element UnveilWindow => _UnveilWindow ??= GetObject<Element>(IngameUIElementsStruct.UnveilWindow);
        public Element ZanaMissionChoice => _ZanaMissionChoice ??= GetObject<Element>(IngameUIElementsStruct.ZanaMissionChoice);
        public IncursionWindow IncursionWindow => _IncursionWindow ??= GetObject<IncursionWindow>(IngameUIElementsStruct.IncursionWindow);
        public Element SynthesisWindow => _SynthesisWindow ??= GetObject<Element>(IngameUIElementsStruct.SynthesisWindow);
        public Element CraftBench => _CraftBench ??= GetObject<Element>(IngameUIElementsStruct.CraftBenchWindow);
        public bool IsDndEnabled => M.Read<byte>(Address + 0xf92) == 1;
        public string DndMessage => M.ReadStringU(M.Read<long>(Address + 0xf98));
        public WorldMapElement AreaInstanceUi => GetObject<WorldMapElement>(IngameUIElementsStruct.AreaInstanceUi);
        public WorldMapElement WorldMap => GetObject<WorldMapElement>(IngameUIElementsStruct.WorldMap);
        public MetamorphWindowElement MetamorphWindow => GetObject<MetamorphWindowElement>(IngameUIElementsStruct.MetamorphWindow);
        public RitualWindow RitualWindow => GetObject<RitualWindow>(IngameUIElementsStruct.RitualWindow);
        public Element RitualFavourWindow => GetObject<Element>(IngameUIElementsStruct.RitualFavourPanel);
        public Element UltimatumProgressWindow => GetObject<Element>(IngameUIElementsStruct.UltimatumProgressPanel);
        public DelveDarknessElement DelveDarkness => GetObject<DelveDarknessElement>(IngameUIElementsStruct.DelveDarkness);
        public HarvestWindow HarvestWindow => GetObject<HarvestWindow>(IngameUIElementsStruct.HarvestWindow);
        
        public IEnumerable<QuestState> GetUncompletedQuests => GetQuestStates.Where(q => q.QuestStateId != 0);
        public IEnumerable<QuestState> GetCompletedQuests => GetQuestStates.Where(q => q.QuestStateId == 0);
        public List<QuestState> GetQuestStates => _cachedQuestStates?.Value;

        public Element Element248 => GetObject<Element>(M.Read<long>(Address + 0x248));
        public Element Element250 => GetObject<Element>(M.Read<long>(Address + 0x250));
        public Element Element268 => GetObject<Element>(M.Read<long>(Address + 0x268));
        public Element Element270 => GetObject<Element>(M.Read<long>(Address + 0x270));
        public Element Element278 => GetObject<Element>(M.Read<long>(Address + 0x278));
        public Element Element290 => GetObject<Element>(M.Read<long>(Address + 0x290));
        public Element Element2C0 => GetObject<Element>(M.Read<long>(Address + 0x2C0));
        public Element Element2C8 => GetObject<Element>(M.Read<long>(Address + 0x2C8));
        public Element Element2D0 => GetObject<Element>(M.Read<long>(Address + 0x2D0));
        public Element Element2D8 => GetObject<Element>(M.Read<long>(Address + 0x2D8));
        public Element Element2E0 => GetObject<Element>(M.Read<long>(Address + 0x2E0));
        public Element Element2E8 => GetObject<Element>(M.Read<long>(Address + 0x2E8));
        public Element Element2F0 => GetObject<Element>(M.Read<long>(Address + 0x2F0));
        public Element Element2F8 => GetObject<Element>(M.Read<long>(Address + 0x2F8));
        public Element Element300 => GetObject<Element>(M.Read<long>(Address + 0x300));
        public Element Element308 => GetObject<Element>(M.Read<long>(Address + 0x308));
        public Element Element310 => GetObject<Element>(M.Read<long>(Address + 0x310));
        public Element Element318 => GetObject<Element>(M.Read<long>(Address + 0x318));
        public Element Element320 => GetObject<Element>(M.Read<long>(Address + 0x320));
        public Element Element360 => GetObject<Element>(M.Read<long>(Address + 0x360));
        public Element Element3C0 => GetObject<Element>(M.Read<long>(Address + 0x3C0));
        public Element Element3C8 => GetObject<Element>(M.Read<long>(Address + 0x3C8));
        public Element Element3D8 => GetObject<Element>(M.Read<long>(Address + 0x3D8));
        public Element Element3E0 => GetObject<Element>(M.Read<long>(Address + 0x3E0));
        public Element Element3F0 => GetObject<Element>(M.Read<long>(Address + 0x3F0));
        public Element Element3F8 => GetObject<Element>(M.Read<long>(Address + 0x3F8));
        public Element Element458 => GetObject<Element>(M.Read<long>(Address + 0x458));
        public Element Element4B8 => GetObject<Element>(M.Read<long>(Address + 0x4B8));
        public Element Element4C0 => GetObject<Element>(M.Read<long>(Address + 0x4C0));
        public Element Element4D0 => GetObject<Element>(M.Read<long>(Address + 0x4D0));
        public Element Element4D8 => GetObject<Element>(M.Read<long>(Address + 0x4D8));
        public Element Element4E0 => GetObject<Element>(M.Read<long>(Address + 0x4E0));
        public Element Element4E8 => GetObject<Element>(M.Read<long>(Address + 0x4E8));
        public Element Element4F0 => GetObject<Element>(M.Read<long>(Address + 0x4F0));
        public Element Element4F8 => GetObject<Element>(M.Read<long>(Address + 0x4F8));
        public Element Element500 => GetObject<Element>(M.Read<long>(Address + 0x500));
        public Element Element6E0 => GetObject<Element>(M.Read<long>(Address + 0x6E0));
        public Element Element8C0 => GetObject<Element>(M.Read<long>(Address + 0x8C0));
        public Element Element8C8 => GetObject<Element>(M.Read<long>(Address + 0x8C8));
        public Element Element8D0 => GetObject<Element>(M.Read<long>(Address + 0x8D0));
        public Element Element8D8 => GetObject<Element>(M.Read<long>(Address + 0x8D8));
        public Element Element8E0 => GetObject<Element>(M.Read<long>(Address + 0x8E0));
        public Element Element8E8 => GetObject<Element>(M.Read<long>(Address + 0x8E8));
        public Element Element8F0 => GetObject<Element>(M.Read<long>(Address + 0x8F0));
        public Element Element8F8 => GetObject<Element>(M.Read<long>(Address + 0x8F8));
        public Element Element978 => GetObject<Element>(M.Read<long>(Address + 0x978));
        public Element Element980 => GetObject<Element>(M.Read<long>(Address + 0x980));
        public Element Element988 => GetObject<Element>(M.Read<long>(Address + 0x988));
        public Element Element990 => GetObject<Element>(M.Read<long>(Address + 0x990));
        public Element Element9A0 => GetObject<Element>(M.Read<long>(Address + 0x9A0));
        public Element Element9A8 => GetObject<Element>(M.Read<long>(Address + 0x9A8));
        public Element Element9B0 => GetObject<Element>(M.Read<long>(Address + 0x9B0));
        public Element Element9B8 => GetObject<Element>(M.Read<long>(Address + 0x9B8));
        public Element Element9C0 => GetObject<Element>(M.Read<long>(Address + 0x9C0));
        public Element Element9C8 => GetObject<Element>(M.Read<long>(Address + 0x9C8));
        public Element Element9D0 => GetObject<Element>(M.Read<long>(Address + 0x9D0));
        public Element Element9D8 => GetObject<Element>(M.Read<long>(Address + 0x9D8));
        public Element Element9E0 => GetObject<Element>(M.Read<long>(Address + 0x9E0));
        public Element Element9E8 => GetObject<Element>(M.Read<long>(Address + 0x9E8));
        public Element Element9F0 => GetObject<Element>(M.Read<long>(Address + 0x9F0));
        public Element ElementA00 => GetObject<Element>(M.Read<long>(Address + 0xA00));
        public Element ElementA10 => GetObject<Element>(M.Read<long>(Address + 0xA10));
        public Element ElementA38 => GetObject<Element>(M.Read<long>(Address + 0xA38));
        public Element ElementA90 => GetObject<Element>(M.Read<long>(Address + 0xA90));
        public Element ElementAA8 => GetObject<Element>(M.Read<long>(Address + 0xAA8));
        public Element ElementAC0 => GetObject<Element>(M.Read<long>(Address + 0xAC0));
        public Element ElementB40 => GetObject<Element>(M.Read<long>(Address + 0xB40));
        public Element ElementB88 => GetObject<Element>(M.Read<long>(Address + 0xB88));
        
        private List<QuestState> GenerateQuestStates()
        {
            var result = new List<QuestState>();
            /*
             * This is definitly not the most performant way to get the quest.
             * 9 quests are missing (e.g. a10q2). 
             */
            for (long i = 0; i < 0xffff; i += 0x8)
            {
                long pointerToQuest = IngameUIElementsStruct.GetQuests + i;
                var addressOfQuest = M.Read<long>(pointerToQuest);

                var questState = GetObject<QuestState>(addressOfQuest);
                if (questState?.Quest != null)
                {
                    if (result.Where(r => r.Quest?.Id == questState.Quest?.Id).Any()) continue; // skip entries which are already in the list
                    result.Add(questState);
                }
            }

            return result;
        }
    }
}
