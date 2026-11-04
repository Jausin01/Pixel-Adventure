using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player the quest board (the generated quests) and the quest log (kill progress and hand-in).
    /// </summary>
    public static class QuestLogBuilder
    {
        /// <summary>Adds the board and the log to the player and returns the log.</summary>
        public static QuestLog Attach(GameObject player, QuestPools pools)
        {
            QuestBoard board = player.AddComponent<QuestBoard>();
            board.pools = pools;

            QuestLog log = player.AddComponent<QuestLog>();
            log.board = board;
            return log;
        }
    }
}
