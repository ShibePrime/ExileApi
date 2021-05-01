using GameOffsets;

namespace ExileCore.PoEMemory.MemoryObjects
{
    public class QuestState : RemoteMemoryObject
    {
        public QuestStateOffsets QuestStateOffsets => M.Read<QuestStateOffsets>(Address);
        public long QuestPtr => QuestStateOffsets.QuestAddress;
        public Quest Quest => TheGame.Files.Quests.GetByAddress(QuestPtr);
        public int QuestStateId => (int) QuestStateOffsets.QuestStateId;
        public string QuestStateText => M.ReadStringU(QuestStateOffsets.QuestStateTextAddress);
        public string QuestProgressText => M.ReadStringU(QuestStateOffsets.QuestProgressTextAddress);

        public override string ToString()
        {
            return $"State: {QuestStateId}, Quest.Id: {Quest.Id}, Quest.Name: {Quest.Name}, ProgressText {QuestProgressText}, StateText {QuestStateText}";
        }
    }
}
