using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Converts between the live game state (inventory, gear, skills, quests) and <see cref="SaveData"/>.
    /// Does not decide when to save or load.
    /// </summary>
    public class SaveSerializer
    {
        private readonly GameDatabase database;
        private readonly Inventory inventory;
        private readonly Inventory stash;
        private readonly Equipment equipment;
        private readonly SkillUnlocks unlocks;
        private readonly SkillLoadout loadout;
        private readonly QuestLog quests;

        /// <summary>The quick button's item, saved with the rest. Optional.</summary>
        public QuickItem QuickItem { get; set; }

        /// <summary>Creates a serializer for one character.</summary>
        public SaveSerializer(GameDatabase database, Inventory inventory, Inventory stash, Equipment equipment, SkillUnlocks unlocks, SkillLoadout loadout, QuestLog quests)
        {
            this.database = database;
            this.inventory = inventory;
            this.stash = stash;
            this.equipment = equipment;
            this.unlocks = unlocks;
            this.loadout = loadout;
            this.quests = quests;
        }

        /// <summary>Reads the current state.</summary>
        public SaveData Capture()
        {
            SaveData data = new SaveData();

            CaptureItems(inventory, data.inventory);
            CaptureItems(stash, data.stash);

            for (int i = 0; i < EquipmentSlots.All.Length; i++)
            {
                InventorySlot worn = equipment.GetPiece(EquipmentSlots.All[i]);
                if (worn == null) continue;

                data.equipment.Add(new SaveData.WornEntry
                {
                    slot = (int)EquipmentSlots.All[i], id = worn.Item.id,
                    level = worn.Gear.Level, reforges = worn.Gear.Reforges, nature = NatureId(worn.Gear)
                });
            }

            foreach (SkillDefinition skill in unlocks.Unlocked) data.unlockedSkills.Add(skill.id);

            foreach (WeaponClass weaponClass in Enum.GetValues(typeof(WeaponClass)))
            {
                for (int index = 0; index < SkillLoadout.SlotCount; index++)
                {
                    SkillDefinition skill = loadout.Get(weaponClass, index);
                    if (skill != null) data.loadout.Add(new SaveData.LoadoutEntry { weaponClass = (int)weaponClass, index = index, skillId = skill.id });
                }
            }

            QuestDefinition[] board = quests.board.Slots;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null) data.quests.Add(QuestSaveMapper.ToEntry(board[i]));
            }
            if (QuickItem != null && QuickItem.Item != null) data.quickItem = QuickItem.Item.id;
            data.questsDone = quests.board.Done;
            data.questSerial = quests.board.Serial;
            foreach (System.Collections.Generic.KeyValuePair<string, int> pair in quests.KillProgress)
            {
                data.questKills.Add(new SaveData.KillEntry { key = pair.Key, count = pair.Value });
            }
            return data;
        }

        /// <summary>Puts a saved state into the game. Entries that no longer exist are skipped.</summary>
        public void Apply(SaveData data)
        {
            ApplyItems(inventory, data.inventory);
            ApplyItems(stash, data.stash);

            for (int i = 0; i < data.equipment.Count; i++)
            {
                SaveData.WornEntry entry = data.equipment[i];
                ItemDefinition item = database.FindItem(entry.id);
                if (item == null || item.equipKind == EquipmentKind.None) continue;

                // Worn gear is not stored in the inventory, so hand it over first and then wear it.
                InventorySlot piece = Piece(item, entry.level, entry.reforges, entry.nature);
                inventory.Insert(piece);
                equipment.Equip(piece, (EquipmentSlot)entry.slot);
            }

            for (int i = 0; i < data.unlockedSkills.Count; i++) unlocks.Unlock(database.FindSkill(data.unlockedSkills[i]));

            for (int i = 0; i < data.loadout.Count; i++)
            {
                SaveData.LoadoutEntry entry = data.loadout[i];
                loadout.Equip((WeaponClass)entry.weaponClass, entry.index, database.FindSkill(entry.skillId));
            }

            if (QuickItem != null && !string.IsNullOrEmpty(data.quickItem)) QuickItem.Select(database.FindItem(data.quickItem));

            System.Collections.Generic.List<QuestDefinition> savedQuests = new System.Collections.Generic.List<QuestDefinition>();
            for (int i = 0; i < data.quests.Count; i++) savedQuests.Add(QuestSaveMapper.FromEntry(data.quests[i], database));
            quests.board.Restore(savedQuests, data.questsDone, data.questSerial);
            for (int i = 0; i < data.questKills.Count; i++) quests.RestoreKills(data.questKills[i].key, data.questKills[i].count);
        }

        private static void CaptureItems(Inventory source, System.Collections.Generic.List<SaveData.ItemEntry> entries)
        {
            for (int i = 0; i < source.Slots.Count; i++)
            {
                InventorySlot slot = source.Slots[i];
                SaveData.ItemEntry entry = new SaveData.ItemEntry { id = slot.Item.id, count = slot.Count };
                if (slot.Gear != null)
                {
                    entry.level = slot.Gear.Level;
                    entry.reforges = slot.Gear.Reforges;
                    entry.nature = NatureId(slot.Gear);
                }
                entries.Add(entry);
            }
        }

        private void ApplyItems(Inventory target, System.Collections.Generic.List<SaveData.ItemEntry> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                SaveData.ItemEntry entry = entries[i];
                ItemDefinition item = database.FindItem(entry.id);
                if (item == null) continue;

                if (item.equipKind != EquipmentKind.None) target.Insert(Piece(item, entry.level, entry.reforges, entry.nature));
                else target.Add(item, entry.count);
            }
        }

        private static InventorySlot Piece(ItemDefinition item, int level, int reforges, string natureId)
        {
            return new InventorySlot(item, GearState.Restore(item, level, reforges, natureId));
        }

        private static string NatureId(GearState gear)
        {
            return gear.Nature != null ? gear.Nature.id : string.Empty;
        }
    }
}
