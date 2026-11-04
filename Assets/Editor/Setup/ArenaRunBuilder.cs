using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the objects that run the arenas: the wave director and the run flow, plus the run ledger on the player.
    /// </summary>
    public static class ArenaRunBuilder
    {
        /// <summary>Builds the run controllers.</summary>
        public static ArenaRun Build(GameObject player)
        {
            RunLedger ledger = player.AddComponent<RunLedger>();

            GameObject root = new GameObject("Arena Run");
            WaveDirector director = root.AddComponent<WaveDirector>();
            director.player = player.transform;

            ArenaRun run = root.AddComponent<ArenaRun>();
            run.player = player.transform;
            run.director = director;
            run.ledger = ledger;

            ArenaStageController stages = root.AddComponent<ArenaStageController>();
            stages.run = run;
            stages.director = director;
            stages.player = player.transform;
            return run;
        }
    }
}
