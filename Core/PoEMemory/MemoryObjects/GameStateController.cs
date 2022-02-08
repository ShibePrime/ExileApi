using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using SharpDX.DirectWrite;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class TheGame : RemoteMemoryObject
    {
        private static readonly int CurrentAreaHashOffset = Extensions.GetOffset<IngameDataOffsets>(nameof(IngameDataOffsets.CurrentAreaHash));
        private static readonly int DataOffset = Extensions.GetOffset<IngameStateOffsets>(nameof(IngameStateOffsets.Data));

        //I hope this caching will works fine
        private static long PreGameStatePtr = -1;
        private static long LoginStatePtr = -1;
        private static long SelectCharacterStatePtr = -1;
        private static long WaitingStatePtr = -1;
        private static long InGameStatePtr = -1;
        private static long LoadingStatePtr = -1;
        private static long EscapeStatePtr = -1;
        private static TheGame Instance;
        private readonly CachedValue<int> _AreaChangeCount;
        private readonly CachedValue<bool> _inGame;
        public readonly Dictionary<GameStateE, GameState> AllGameStates;

        public TheGame(IMemory m, Cache cache)
        {
            pM = m;
            pCache = cache;
            pTheGame = this;
            Instance = this;
            Address = m.Read<long>(m.BaseOffsets[OffsetsName.GameStateOffset] + m.AddressOfProcess);
            _AreaChangeCount = new TimeCache<int>(() => M.Read<int>(M.AddressOfProcess + M.BaseOffsets[OffsetsName.AreaChangeCount]), 50);
            AllGameStates = ReadList(Address);
            PreGameStatePtr = AllGameStates[GameStateE.PreGameState].Address;
            LoginStatePtr = AllGameStates[GameStateE.LoginState].Address;
            SelectCharacterStatePtr = AllGameStates[GameStateE.SelectCharacterState].Address;
            WaitingStatePtr = AllGameStates[GameStateE.WaitingState].Address;
            InGameStatePtr = AllGameStates[GameStateE.InGameState].Address;
            LoadingStatePtr = AllGameStates[GameStateE.LoadingState].Address;
            EscapeStatePtr = AllGameStates[GameStateE.EscapeState].Address;
            
            LoadingState = new AreaLoadingState(AllGameStates[GameStateE.AreaLoadingState]);
            IngameState = new IngameState(AllGameStates[GameStateE.InGameState]);

            _inGame = new FrameCache<bool>(
                () => IngameState.Address != 0 && IngameState.Data.Address != 0 && IngameState.Data.ServerData.Address != 0 && !IsLoading /*&&
                                                 IngameState.ServerData.IsInGame*/);

            Files = new FilesContainer(m);
        }

        public FilesContainer Files { get; set; }
        public AreaLoadingState LoadingState { get; }
        public IngameState IngameState { get; }
        public IList<GameState> CurrentGameStates => M.ReadDoublePtrVectorClasses<GameState>(Address + 0x8, TheGame);
        public IList<GameState> ActiveGameStates => M.ReadDoublePtrVectorClasses<GameState>(Address + 0x20, TheGame, true);
        public bool IsPreGame => GameStateActive(PreGameStatePtr);
        public bool IsLoginState => GameStateActive(LoginStatePtr);
        public bool IsSelectCharacterState => GameStateActive(SelectCharacterStatePtr);
        public bool IsWaitingState => GameStateActive(WaitingStatePtr); //This happens after selecting character, maybe other cases
        public bool IsInGameState => GameStateActive(InGameStatePtr); //In game, with selected character
        public bool IsLoadingState => GameStateActive(LoadingStatePtr);
        public bool IsEscapeState => GameStateActive(EscapeStatePtr);
        public bool IsLoading => LoadingState.IsLoading;
        public int AreaChangeCount => _AreaChangeCount.Value;
        public bool InGame => _inGame.Value;

        public uint CurrentAreaHash
        {
            get
            {
                var hash = M.Read<uint>(IngameState.Address + DataOffset, CurrentAreaHashOffset);
                return hash;
            }
        }

        public void Init()
        {
        }

        private static bool GameStateActive(long stateAddress)
        {
            var gameStateController = Instance;
            if (gameStateController == null) return false;
            var M = gameStateController.M;
            var address = Instance.Address + 0x20;

            var start = M.Read<long>(address);
            var last = M.Read<long>(address + 0x8);
            //var end = Read<long>(address + 0x10);

            var length = (int)(last - start);
            var bytes = M.ReadMem(start, length);

            for (var readOffset = 0; readOffset < length; readOffset += 16)
            {
                var pointer = BitConverter.ToInt64(bytes, readOffset);
                if (stateAddress == pointer) return true;
            }

            return false;
        }

        private Dictionary<GameStateE, GameState> ReadList(long pointer)
        {
            var states = M.ReadAsArray<(IntPtr addr, long)>(pointer + 0x48, 12);
            return states.Select((state, i) => (state.addr, i)).ToDictionary(x => (GameStateE)x.i, x => GetObject<GameState>(x.addr));
        }
    }

    public class GameState : RemoteMemoryObject
    {
        public string StateName;
        public GameStateE Type => M.Read<GameStateE>(Address + 0x0B);

        public GameState() : base() { }

        public GameState(long address) : base()
        {
            Address = address;
        }
    }

    public class AreaLoadingState : GameState
    {
        public AreaLoadingState(GameState parent)
        {
            Address = parent.Address;
            StateName = parent.StateName;
        }
        //This is actually pointer to loading screen stuff (image, etc), but should works fine.
        public bool IsLoading => M.Read<long>(Address + 0xB8) == 1;
        public string AreaName => M.ReadStringU(M.Read<long>(Address + 0x388));

        public override string ToString()
        {
            return $"{AreaName}, IsLoading: {IsLoading}";
        }
    }

    public enum GameStateTypes
    {
        AreaLoadingState,
        WaitingState,
        CreditsState,
        EscapeState,
        InGameState,
        ChangePasswordState,
        LoginState,
        PreGameState,
        CreateCharacterState,
        SelectCharacterState,
        DeleteCharacterState,
        LoadingState
    }
}