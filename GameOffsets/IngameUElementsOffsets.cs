using System.Runtime.InteropServices;

namespace GameOffsets
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IngameUElementsOffsets
    {
        [FieldOffset(0x218)] public long GetQuests;
        [FieldOffset(0x240)] public long GameUI;
        [FieldOffset(0x378)] public long Mouse;
        [FieldOffset(0x390)] public long SkillBar;
        [FieldOffset(0x398)] public long HiddenSkillBar; 
        [FieldOffset(0x418)] public long ChatPanel; 
        [FieldOffset(0x498)] public long QuestTracker;
        [FieldOffset(0x500)] public long OpenLeftPanel;
        [FieldOffset(0x508)] public long OpenRightPanel;
        [FieldOffset(0x538)] public long InventoryPanel;
        [FieldOffset(0x540)] public long StashElement;
        [FieldOffset(0x568)] public long TreePanel;
        [FieldOffset(0x570)] public long AtlasPanel;
        [FieldOffset(0x5A0)] public long WorldMap;
        [FieldOffset(0x5C0)] public long Map;
        [FieldOffset(0x5C8)] public long itemsOnGroundLabelRoot;
        [FieldOffset(0x5D8)] public long BanditDialog;
        [FieldOffset(0x650)] public long NpcDialog;
        [FieldOffset(0x668)] public long QuestRewardWindow;
        [FieldOffset(0x670)] public long PurchaseWindow;
        [FieldOffset(0x678)] public long SellWindow;
        [FieldOffset(0x680)] public long TradeWindow;
        [FieldOffset(0x6B8)] public long MapDeviceWindow;
        [FieldOffset(0x710)] public long IncursionWindow;
        [FieldOffset(0x730)] public long DelveWindow;
        [FieldOffset(0x740)] public long ZanaMissionChoice;
        [FieldOffset(0x750)] public long BetrayalWindow;
        [FieldOffset(0x768)] public long CraftBenchWindow;
        [FieldOffset(0x770)] public long UnveilWindow;
        [FieldOffset(0x798)] public long SynthesisWindow;
        [FieldOffset(0x7A8)] public long MetamorphWindow;
        [FieldOffset(0x7B8)] public long HarvestWindow;
        [FieldOffset(0x7F0)] public long RitualWindow;
        [FieldOffset(0x850)] public long DelveDarkness;
        [FieldOffset(0x860)] public long AreaInstanceUi;
        [FieldOffset(0x958)] public long InvitesPanel;
        [FieldOffset(0x9A8)] public long GemLvlUpPanel;
        [FieldOffset(0xA68)] public long ItemOnGroundTooltip;
        [FieldOffset(0xAA8)] public long MapTabWindowStartPtr;

        //[FieldOffset(0x218)] public long GetQuests;
        //[FieldOffset(0x240)] public long GameUI;
        //[FieldOffset(0x378)] public long Mouse;
        //[FieldOffset(0x390)] public long SkillBar;
        //[FieldOffset(0x398)] public long HiddenSkillBar;
        //[FieldOffset(0x3B8)] public long PvpLeaveWithdrawPanel;
        //[FieldOffset(0x3C0)] public long PvpTimerPanel;
        //[FieldOffset(0x3C8)] public long PvpFightNotifyPanel;
        //[FieldOffset(0x3D0)] public long PvpNextRoundNotifyNotifyPanel;
        //[FieldOffset(0x3D8)] public long PvpStopSpectatingPanel;

        //[FieldOffset(0x3E8)] public long PvpMoreTimersPanel;
        //[FieldOffset(0x3F0)] public long UI[25][6];
        //[FieldOffset(0x3F8)] public long PvpSpectatingPanel;
        //[FieldOffset(0x400)] public long UI[104];

        //[FieldOffset(0x418)] public long ChatPanel;

        //[FieldOffset(0x428)] public long UI[111];
        //[FieldOffset(0x430)] public long UI[112];
        //[FieldOffset(0x438)] public long BestiaryMissionCompletePanel;
        //[FieldOffset(0x440)] public long BestiaryNewBeastNotifyPanel;
        //[FieldOffset(0x448)] public long BestiaryNewRecipeNotifyPanel;

        //[FieldOffset(0x480)] public long SulphiteProgressBar;
        //[FieldOffset(0x488)] public long UI[113];
        //[FieldOffset(0x490)] public long UI[114];
        //[FieldOffset(0x498)] public long QuestTracker;
        //[FieldOffset(0x4A0)] public long BenchCraftNewRecipeNotifyPanel;

        //[FieldOffset(0x4C0)] public long HideoutUnlockNotifyPanel;

        //[FieldOffset(0x500)] public long OpenLeftPanel;
        //[FieldOffset(0x508)] public long OpenRightPanel;
        //[FieldOffset(0x510)] public long OpenLeftPanel2; // Holds same address as above.
        //[FieldOffset(0x518)] public long OpenRightPanel2; // Holds same address as above.
        
        //[FieldOffset(0x528)] public long MtxStashPanel;
        //[FieldOffset(0x530)] public long MtxShopPanel;
        //[FieldOffset(0x538)] public long InventoryPanel;
        //[FieldOffset(0x540)] public long StashElement;

        //[FieldOffset(0x550)] public long GuildStashPanel;
        //[FieldOffset(0x558)] public long HideoutStashPanel; // Hideout stash panels from before rework.
        //[FieldOffset(0x560)] public long SocialPanel;
        //[FieldOffset(0x568)] public long TreePanel;
        //[FieldOffset(0x570)] public long AtlasPanel;
        //[FieldOffset(0x578)] public long CharacterPanel;
        //[FieldOffset(0x580)] public long OptionsPanel;
        //[FieldOffset(0x588)] public long ChallengesPanel;
        //[FieldOffset(0x590)] public long PantheonPanel;
        //[FieldOffset(0x598)] public long EventsPanel;
        //[FieldOffset(0x5A0)] public long WorldMap;
        //[FieldOffset(0x5A8)] public long MtxPanel;
        //[FieldOffset(0x5B0)] public long DecorationsPanel;
        //[FieldOffset(0x5B8)] public long HelpPanel;
        //[FieldOffset(0x5C0)] public long Map;
        //[FieldOffset(0x5C8)] public long itemsOnGroundLabelRoot;
        //[FieldOffset(0x5BB)] public long VisibleHealthBars;
        //[FieldOffset(0x5D8)] public long BanditDialog;

        //[FieldOffset(0x600)] public long UI[123];

        //[FieldOffset(0x648)] public long BuffPanel;
        //[FieldOffset(0x650)] public long NpcDialog;
        //[FieldOffset(0x668)] public long QuestRewardWindow;
        //[FieldOffset(0x670)] public long PurchaseWindow;
        //[FieldOffset(0x678)] public long SellWindow;
        //[FieldOffset(0x680)] public long TradeWindow;
        //[FieldOffset(0x688)] public long MapReceptacle;
        //[FieldOffset(0x690)] public long PerandusCadiroOfferPanel;
        //[FieldOffset(0x698)] public long LabyrinthDivineFontPanel;
        //[FieldOffset(0x6A0)] public long TalismanStoneCirclePanel;
        //[FieldOffset(0x6A8)] public long TrialPlaquePanel;
        //[FieldOffset(0x6B0)] public long AscendancySelectPanel;
        //[FieldOffset(0x6B8)] public long MapDeviceWindow;
        //[FieldOffset(0x6C0)] public long DarkshrinePanel;
        //[FieldOffset(0x6C8)] public long BestiaryCraftPanel;
        //[FieldOffset(0x6D0)] public long LabyrinthSelectPanel;
        //[FieldOffset(0x6D8)] public long LabyrinthMapPanel;
        //[FieldOffset(0x6E0)] public long GuildTagEditor;
        //[FieldOffset(0x6E8)] public long BroadcastMessagePanel;
        //[FieldOffset(0x6F0)] public long FriendNoteEditorPanel;
        //[FieldOffset(0x6F8)] public long BetaLadderScreen;

        //[FieldOffset(0x708)] public long DivinationCardTradeScreen;
        //[FieldOffset(0x710)] public long IncursionWindow;
        //[FieldOffset(0x718)] public long IncursionCorruptionAltarPanel;
        //[FieldOffset(0x720)] public long IncursionAltarOfSacrificePanel;
        //[FieldOffset(0x728)] public long IncursionLapidaryLensPanel;
        //[FieldOffset(0x730)] public long NikoSubterraneanChartPanel; // This is the subterrranean chart when talking to niko.
        //[FieldOffset(0x738)] public long DelveWindow;  // This is the subterranean chart using in the mine.
        //[FieldOffset(0x740)] public long ZanaMissionChoice;
        
        //[FieldOffset(0x750)] public long JunSyndicateInvestigationPanel; // this is the murder board when talking to Jun.
        //[FieldOffset(0x758)] public long BetrayalWindow; // this is the murder board shown during Betrayal encounter.
        //[FieldOffset(0x760)] public long HelenaHideoutSelectPanel;
  //      [FieldOffset(0x768)] public long CraftBenchWindow;
		//[FieldOffset(0x770)] public long UnveilWindow;
        //[FieldOffset(0x778)] public long BetrayalTrappedStashPanel;
        //[FieldOffset(0x780)] public long BetrayalTinysTrialPanel;
        //[FieldOffset(0x788)] public long BetrayalSyndicateCraftingBenchPanel;
        //[FieldOffset(0x790)] public long SynthesisSynthesiserPanel;
        //[FieldOffset(0x798)] public long SynthesisWindow;
        //[FieldOffset(0x7A0)] public long CassiaAnointPanel;
        //[FieldOffset(0x7A8)] public long MetamorphWindow;  // This is the panel you encounter in maps.
        //[FieldOffset(0x7B0)] public long TanesLabMetamorphWindow; // This is the panel you encounter in Tane's Lab.
        //[FieldOffset(0x7B8)] public long HarvestWindow;
        //[FieldOffset(0x7C0)] public long HorticraftingStationPanel;
        //[FieldOffset(0x7C8)] public long HeistContractPanel;
        //[FieldOffset(0x7D0)] public long HeistBlueprintPanel;
        //[FieldOffset(0x7D8)] public long HeistAllyEquipmentPanel;
        //[FieldOffset(0x7E0)] public long HeistGrandHeistPanel;
        //[FieldOffset(0x7E8)] public long HeistLockerPanel;
        //[FieldOffset(0x7F0)] public long RitualWindow;
        //[FieldOffset(0x7F8)] public long RitualFavourPanel;
        //[FieldOffset(0x800)] public long UltimatumPanel;
        //[FieldOffset(0x808)] public long UI[25][10];

        //[FieldOffset(0x828)] public long UI[24]; Guild Perms

        //[FieldOffset(0x840)] public long MapSettingsPanel;
        //[FieldOffset(0x848)] public long UI[100];
        //[FieldOffset(0x850)] public long DelveDarkness;

        //[FieldOffset(0x860)] public long AreaInstanceUi;

        //[FieldOffset(0x880)] public long MtxSalvagePanel;

        //[FieldOffset(0x898)] public long ReportPlayerPanel;

        //[FieldOffset(0x8B0)] public long DeadNotifyPanel;

        //[FieldOffset(0x940)] public long HideoutMusicPanel;
        //[FieldOffset(0x948)] public long UI[25][9];
        //[FieldOffset(0x950)] public long ZoneTravelNotifyPanel;
        //[FieldOffset(0x958)] public long InvitesPanel;

        //[FieldOffset(0x9A8)] public long GemLvlUpPanel;

        //[FieldOffset(0xA00)] public long RampageKillCountPanel;
        //[FieldOffset(0xA08)] public long IncursionKillCountPanel;
        //[FieldOffset(0xA18)] public long ZeroMustSurvivePanel;
        //[FieldOffset(0xA20)] public long SynthesisStabilizerProgressPanel;
        //[FieldOffset(0xA28)] public long SynthesisRewardNotifyPanel;
        //[FieldOffset(0xA30)] public long BlightProgressPanel;

        //[FieldOffset(0xA48)] public long HeistNotifyPanel;
        //[FieldOffset(0xA50)] public long UltimatumChallengeProgressPanel;
        //[FieldOffset(0xA58)] public long UltimatumReturnToRingNotifyPanel;

        //[FieldOffset(0xA68)] public long ItemOnGroundTooltip;
        
        //[FieldOffset(0xAA8)] public long MapTabWindowStartPtr;
    }
}
