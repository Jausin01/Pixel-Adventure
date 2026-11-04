using System.Collections.Generic;

namespace MobileRpg
{
    /// <summary>
    /// A text that identifies what a quest asks for, ignoring amounts and rewards.
    /// Two quests with the same signature are considered the same quest.
    /// </summary>
    public static class QuestSignature
    {
        /// <summary>Builds the signature of a quest.</summary>
        public static string Of(QuestDefinition quest)
        {
            List<string> keys = new List<string>();
            for (int i = 0; quest.requirements != null && i < quest.requirements.Length; i++) keys.Add("item:" + quest.requirements[i].item.id);
            for (int i = 0; quest.kills != null && i < quest.kills.Length; i++)
            {
                QuestDefinition.KillGoal goal = quest.kills[i];
                keys.Add("kill:" + goal.enemy + "@" + (goal.arena != null ? goal.arena.id : string.Empty));
            }
            for (int i = 0; quest.waves != null && i < quest.waves.Length; i++) keys.Add("wave:" + quest.waves[i].arena.id);

            keys.Sort(System.StringComparer.Ordinal);
            return string.Join("|", keys.ToArray());
        }
    }
}
