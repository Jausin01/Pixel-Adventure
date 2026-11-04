namespace MobileRpg
{
    /// <summary>
    /// Touch button (a quest row) that hands in the quest in its board slot when touched.
    /// </summary>
    public class TurnInQuestButton : PointerDownAction
    {
        public QuestBoard board;
        public int slot;
        public QuestLog log;

        /// <inheritdoc/>
        protected override void Execute()
        {
            QuestDefinition quest = board.Slots[slot];
            if (quest != null) log.TryTurnIn(quest);
        }
    }
}
