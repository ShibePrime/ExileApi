using System;
using System.Collections;
using System.Collections.Generic;
using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using SharpDX;

namespace ExileCore.PoEMemory.Components
{
    public class Player : Component
    {
        private readonly CachedValue<PlayerComponentOffsets> _player;

        public Player()
        {
            _player = new FrameCache<PlayerComponentOffsets>(() =>
                Address == 0 ? default : M.Read<PlayerComponentOffsets>(Address));
        }

        public string PlayerName => NativeStringReader.ReadString(Address + 0x160, M);
        public uint XP => Address != 0 ? _player.Value.XP : 0;
        public int Strength => Address != 0 ? _player.Value.Strength : 0;
        public int Dexterity => Address != 0 ? _player.Value.Dexterity : 0;
        public int Intelligence => Address != 0 ? _player.Value.Intelligence : 0;
        public int Level => Address != 0 ? _player.Value.Level : 1;
        public int AllocatedLootId => Address != 0 ? _player.Value.AllocatedLootId : 1;

        [ObsoleteAttribute("Hideoutproperties are obsolete.", true)]
        public int HideoutLevel => Address != 0 ? _player.Value.HideoutLevel : 0;

        public int PropheciesCount => Address != 0 ? _player.Value.PropheciesCount : 0;

        public IList<ProphecyDat> Prophecies
        {
            get
            {
                var result = new List<ProphecyDat>();
                var readAddr = Address + _player.Value.PropheciesOffset;

                for (var i = 0; i < 7; i++)
                {
                    var prophecyId = M.Read<ushort>(readAddr);

                    //if(prophacyId > 0)//Dunno why it will never be 0 even if there is no prophecy
                    {
                        var prophecy = TheGame.Files.Prophecies.GetProphecyById(prophecyId);

                        // if (prophecy != null)
                        result.Add(prophecy);
                    }

                    readAddr += _player.Value
                        .ProphecyLength; //prophecy prophecyId(UShort), Skip index(byte), Skip unknown(byte)
                }

                return result;
            }
        }

        [ObsoleteAttribute("Hideoutproperties are obsolete.", true)]
        public HideoutWrapper Hideout => ReadObject<HideoutWrapper>(Address + _player.Value.HideoutWrapperOffset);

        public PantheonGod PantheonMinor => (PantheonGod)_player.Value.PantheonMinor;
        public PantheonGod PantheonMajor => (PantheonGod)_player.Value.PantheonMajor;

        private IList<PassiveSkill> AllocatedPassivesM()
        {
            var result = new List<PassiveSkill>();
            var passiveIds = TheGame.IngameState.Data.ServerData.PassiveSkillIds;

            foreach (var id in passiveIds)
            {
                var passive = TheGame.Files.PassiveSkills.GetPassiveSkillByPassiveId(id);

                if (passive == null)
                {
                    DebugWindow.LogMsg($"Can't find passive with id: {id}", 10, Color.Red);
                    continue;
                }

                result.Add(passive);
            }

            return result;
        }

        #region Trials

        public bool IsTrialCompleted(string trialId)
        {
            var trialWrapper = TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByAreaId(trialId);

            if (trialWrapper == null)
            {
                throw new ArgumentException(
                    $"Trial with id '{trialId}' is not found. Use WorldArea.Id or LabyrinthTrials.LabyrinthTrialAreaIds[]");
            }

            return TrialPassStates.Get(trialWrapper.Id - 1);
        }

        public bool IsTrialCompleted(LabyrinthTrial trialWrapper)
        {
            if (trialWrapper == null)
                throw new ArgumentException($"Argument {nameof(trialWrapper)} should not be null");

            return TrialPassStates.Get(trialWrapper.Id - 1);
        }

        public bool IsTrialCompleted(WorldArea area)
        {
            if (area == null)
                throw new ArgumentException($"Argument {nameof(area)} should not be null");

            var trialWrapper = TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByArea(area);

            if (trialWrapper == null)
                throw new ArgumentException(
                    $"Can't find trial wrapper for area '{area.Name}' (seems not a trial area).");

            return TrialPassStates.Get(trialWrapper.Id - 1);
        }

        [HideInReflection]
        private BitArray TrialPassStates
        {
            get
            {
                // TODO FIX ME
                var stateBuff = M.ReadBytes(
                    Address + _player.Value.TrialPassStatesOffset,
                    _player.Value.TrialPassStatesLength); // (286+) bytes of info.
                return new BitArray(stateBuff);
            }
        }

        #region Debug things

        public IList<TrialState> TrialStates
        {
            get
            {
                return new List<TrialState>(); // TODO: Files do not load.
                //var result = new List<TrialState>();
                //var passStates = TrialPassStates;

                //foreach (var trialAreaId in LabyrinthTrials.LabyrinthTrialAreaIds)
                //{
                //    var wrapper = TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByAreaId(trialAreaId);

                //    result.Add(
                //        new TrialState
                //        {
                //            TrialAreaId = trialAreaId,
                //            TrialArea = wrapper,
                //            IsCompleted = passStates.Get(wrapper.Id - 1)
                //        });
                //}

                //return result;
            }
        }

        public class TrialState
        {
            public LabyrinthTrial TrialArea { get; internal set; }
            public string TrialAreaId { get; internal set; }
            public bool IsCompleted { get; internal set; }
            public string AreaAddr => TrialArea.Address.ToString("x");

            public override string ToString()
            {
                return $"Completed: {IsCompleted}, Trial {TrialArea.Area.Name}, AreaId: {TrialArea.Id}";
            }
        }

        #endregion

        #endregion

        public new string ToString()
        {
            return PlayerName;
        }
    }
}
