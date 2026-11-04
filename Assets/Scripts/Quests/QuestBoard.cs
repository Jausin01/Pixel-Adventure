using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The quests currently on offer in the town. Every time one is completed a new random quest takes its place,
    /// so the board never runs out and never shows two identical quests.
    /// </summary>
    public class QuestBoard : MonoBehaviour
    {
        public QuestPools pools;

        [Tooltip("How many quests are on the board at once.")]
        public int slotCount = 8;

        private QuestDefinition[] slots;
        private QuestGenerator generator;
        private int serial;

        /// <summary>Raised when any slot gets a different quest.</summary>
        public event Action Changed;

        /// <summary>The quest in each slot. A slot is null only when no new quest could be generated.</summary>
        public QuestDefinition[] Slots
        {
            get
            {
                if (slots == null) slots = new QuestDefinition[slotCount];
                return slots;
            }
        }

        /// <summary>How many quests were completed so far. Makes new quests bigger.</summary>
        public int Done { get; private set; }

        /// <summary>Counter used to give each generated quest a unique id.</summary>
        public int Serial { get { return serial; } }

        /// <summary>The slot holding the quest, or -1.</summary>
        public int IndexOf(QuestDefinition quest)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] == quest) return i;
            }
            return -1;
        }

        /// <summary>Generates a quest for every empty slot.</summary>
        public void Fill()
        {
            bool changed = false;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] != null) continue;

                Slots[i] = Generate();
                changed |= Slots[i] != null;
            }
            if (changed && Changed != null) Changed();
        }

        /// <summary>
        /// Counts the quest as completed and replaces it with a new one. The old quest object is destroyed,
        /// so callers must not use it afterwards.
        /// </summary>
        public void Complete(QuestDefinition quest)
        {
            int index = IndexOf(quest);
            if (index < 0) return;

            Done++;
            Slots[index] = null;
            UnityEngine.Object.Destroy(quest);
            Fill();
        }

        /// <summary>Puts a saved board back (used when loading a save).</summary>
        public void Restore(IList<QuestDefinition> saved, int done, int savedSerial)
        {
            Done = done;
            serial = Mathf.Max(serial, savedSerial);

            for (int i = 0; i < Slots.Length; i++) Slots[i] = i < saved.Count ? saved[i] : null;
            if (Changed != null) Changed();
        }

        private QuestDefinition Generate()
        {
            if (generator == null) generator = new QuestGenerator(pools);

            serial++;
            return generator.Generate(Slots, Done, "quest_" + serial);
        }
    }
}
