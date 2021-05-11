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
        public Element TreePanel => GetObject<Element>(IngameUIElementsStruct.TreePanel);
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
        public DelveDarknessElement DelveDarkness => GetObject<DelveDarknessElement>(IngameUIElementsStruct.DelveDarkness);
        public HarvestWindow HarvestWindow => GetObject<HarvestWindow>(IngameUIElementsStruct.HarvestWindow);

        public IEnumerable<QuestState> GetUncompletedQuests => GetQuestStates.Where(q => q.QuestStateId != 0);
        public IEnumerable<QuestState> GetCompletedQuests => GetQuestStates.Where(q => q.QuestStateId == 0);
        public List<QuestState> GetQuestStates => _cachedQuestStates?.Value;

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
