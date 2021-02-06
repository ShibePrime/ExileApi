using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameUElementsOffsets
    {
        [FieldOffset(0x210)] public long GetQuests;
        [FieldOffset(0x238)] public long GameUI;
        [FieldOffset(0x370)] public long Mouse;
        [FieldOffset(0x378)] public long SkillBar;
        [FieldOffset(0x380)] public long HiddenSkillBar;
        //[FieldOffset(0x398)] public long HeistAlertPanel; // TODO: Verify
        //[FieldOffset(0x3B0)] public long PvpFightAnnouncePanel;
        //[FieldOffset(0x3C8)] public long PvpResultPanel;
        //[FieldOffset(0x3E0)] public long PvpSpectatingPanel;
        //[FieldOffset(0x3F0)] public long EditingHideoutNotifyPanel;
        [FieldOffset(0x410)] public long ChatPanel;
        //[FieldOffset(0x420)] public long BestiaryMissionCompletePanel;
        //[FieldOffset(0x428)] public long BestiaryNewBeastNotifyPanel;
        //[FieldOffset(0x430)] public long BestiaryNewRecipeNotifyPanel;
        [FieldOffset(0x480)] public long QuestTracker;
        //[FieldOffset(0x488)] public long BenchCraftNewRecipeNotifyPanel;
        //[FieldOffset(0x4A8)] public long HideoutUnlockNotifyPanel;
        [FieldOffset(0x4F8)] public long OpenLeftPanel;
        [FieldOffset(0x500)] public long OpenRightPanel;
        //[FieldOffset(0x4F8)] public long OpenLeftPanel2; // Holds same address as above.
        //[FieldOffset(0x500)] public long OpenRightPanel2; // Holds same address as above.
        //[FieldOffset(0x518)] public long MtxShopPanel;
        [FieldOffset(0x530)] public long InventoryPanel;
        [FieldOffset(0x538)] public long StashElement;
        //[FieldOffset(0x538)] public long GuildStashPanel;
        //[FieldOffset(0x540)] public long HideoutStashPanel; // Hideout stash panels from before rework.
        //[FieldOffset(0x548)] public long SocialPanel;
        [FieldOffset(0x550)] public long TreePanel;
        [FieldOffset(0x558)] public long AtlasPanel;
        //[FieldOffset(0x560)] public long CharacterPanel;
        //[FieldOffset(0x568)] public long OptionsPanel;
        //[FieldOffset(0x570)] public long ChallengesPanel;
        //[FieldOffset(0x578)] public long PantheonPanel;
        //[FieldOffset(0x580)] public long EventsPanel;
        [FieldOffset(0x588)] public long WorldMap;
        //[FieldOffset(0x590)] public long MtxPanel;
        //[FieldOffset(0x598)] public long DecorationsPanel;
        //[FieldOffset(0x5A0)] public long HelpPanel;
        [FieldOffset(0x5B8)] public long Map;
        [FieldOffset(0x5c0)] public long itemsOnGroundLabelRoot;
        //[FieldOffset(0x5BB)] public long VisibleHealthBars; // TODO: Check if is only friendly or all bars.
        [FieldOffset(0x5D0)] public long BanditDialog;
        //[FieldOffset(0x630)] public long BuffPanel;
        [FieldOffset(0x648)] public long NpcDialog;
        [FieldOffset(0x650)] public long QuestRewardWindow;
        [FieldOffset(0x668)] public long PurchaseWindow;
        [FieldOffset(0x670)] public long SellWindow;
        [FieldOffset(0x678)] public long TradeWindow;
        //[FieldOffset(0x670)] public long MapReceptacle;
        //[FieldOffset(0x678)] public long PerandusCadiroOfferPanel;
        //[FieldOffset(0x680)] public long LabyrinthDivineFontPanel;
        //[FieldOffset(0x688)] public long TalismanStoneCirclePanel;
        //[FieldOffset(0x690)] public long TrialPlaquePanel;
        //[FieldOffset(0x698)] public long AscendancySelectPanel;
        [FieldOffset(0x6A0)] public long MapDeviceWindow;
        //[FieldOffset(0x6A8)] public long DarkshrinePanel;
        //[FieldOffset(0x6B0)] public long BestiaryCraftPanel;
        //[FieldOffset(0x6B8)] public long LabyrinthPanel;
        //[FieldOffset(0x6C8)] public long GuildTagEditor;
        //[FieldOffset(0x6D0)] public long BroadcastMessagePanel;
        //[FieldOffset(0x6D8)] public long FriendNoteEditorPanel;
        //[FieldOffset(0x6E0)] public long BetaLadderScreen;
        //[FieldOffset(0x6F0)] public long DivinationCardTradeScreen;
        [FieldOffset(0x708)] public long IncursionWindow;
        //[FieldOffset(0x700)] public long IncursionCorruptionAltarPanel;
        //[FieldOffset(0x708)] public long IncursionAltarOfSacrificePanel;
        //[FieldOffset(0x710)] public long IncursionLapidaryLensPanel;
        //[FieldOffset(0x718)] public long NikoSubterraneanChartPanel; // This is the subterrranean chart when talking to niko.
        [FieldOffset(0x720)] public long DelveWindow;  // This is the subterranean chart using in the mine.
		[FieldOffset(0x728)] public long ZanaMissionChoice;
        //[FieldOffset(0x730)] public long SupportGemExamplesPanel;
        //[FieldOffset(0x738)] public long JunSyndicateInvestigationPanel; // this is the murder board when talking to Jun.
        [FieldOffset(0x740)] public long BetrayalWindow; // this is the murder board shown during Betrayal encounter.
        //[FieldOffset(0x748)] public long HelenaHideoutSelectPanel;
        [FieldOffset(0x750)] public long CraftBenchWindow;
		[FieldOffset(0x758)] public long UnveilWindow;
        //[FieldOffset(0x760)] public long BetrayalTrappedStashPanel;
        //[FieldOffset(0x768)] public long BetrayalTinysTrialPanel;
        //[FieldOffset(0x770)] public long BetrayalSyndicateCraftingBenchPanel;
        //[FieldOffset(0x778)] public long SynthesisSynthesiserPanel;
        [FieldOffset(0x780)] public long SynthesisWindow;
        //[FieldOffset(0x788)] public long CassiaAnointPanel;
        [FieldOffset(0x790)] public long MetamorphWindow;  // This is the panel you encounter in maps.
        //[FieldOffset(0x798)] public long TanesLabMetamorphWindow; // This is the panel you encounter in Tane's Lab.
        //[FieldOffset(0x7A0)] public long HarvestCraftPanel;
        //[FieldOffset(0x7A8)] public long MetamorphPanel;
        //[FieldOffset(0x7B0)] public long HarvestSeedStockpilePanel;
        //[FieldOffset(0x7B8)] public long HeistContractPanel;
        //[FieldOffset(0x7C0)] public long HeistBlueprintPanel;
        //[FieldOffset(0x7C8)] public long HeistAllyEquipmentPanel;
        //[FieldOffset(0x7D0)] public long HeistGrandHeistPanel;
        //[FieldOffset(0x7D8)] public long HeistLockerPanel;
        //[FieldOffset(0x810)] public long GuildPermissionsPanel;
        [FieldOffset(0x830)] public long AreaInstanceUi;
        //[FieldOffset(0x840)] public long HeistLeaveNotifyPanel;
        //[FieldOffset(0x848)] public long MysteryBoxPanel;
        //[FieldOffset(0x850)] public long MtxSalvagePanel;
        //[FieldOffset(0x868)] public long ReportPlayerPanel;
        //[FieldOffset(0x8E0)] public long SummonTanePanel;
        //[FieldOffset(0x8F8)] public long HideoutMusicPanel;
        //[FieldOffset(0x908)] public long ZoneTravelNotifyPanel;
        [FieldOffset(0x990)] public long GemLvlUpPanel;
        //[FieldOffset(0x9B8)] public long RampagePanel;
        //[FieldOffset(0x9C0)] public long IncursionProgressPanel;
        //[FieldOffset(0x9C8)] public long ZeroMustSurvivePanel;
        //[FieldOffset(0x9D8)] public long SythesisFragmentsCappedNotifyPanel;
        //[FieldOffset(0x9E0)] public long SynthesisRewardNotifyPanel;
        //[FieldOffset(0x9E8)] public long BlightProgressPanel;
        //[FieldOffset(0xA00)] public long HeistNotifyPanel;
        [FieldOffset(0x8A8)] public long InvitesPanel;
        [FieldOffset(0xA10)] public long ItemOnGroundTooltip;
		[FieldOffset(0xAA0)] public long MapTabWindowStartPtr; //TODO: Find out what exactly this is...
        [FieldOffset(0x7E8)] public long RitualWindow;
    }
}
