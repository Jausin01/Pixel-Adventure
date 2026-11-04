namespace MobileRpg
{
    /// <summary>
    /// One building block of a generated quest (items to hand in, enemies to kill, a wave to reach...).
    /// To add a new kind of goal, write a module and list it in <see cref="QuestGenerator"/>.
    /// </summary>
    public interface IQuestModule
    {
        /// <summary>Adds this module's goal to the draft. Returns false when nothing could be added.</summary>
        bool Apply(QuestDraft draft, QuestPools pools);
    }
}
