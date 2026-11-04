using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Loads the save when the game starts and saves again shortly after anything worth keeping changes,
    /// and when the app is paused or closed.
    /// The game is saved in town right before a run starts and right after it ends. Nothing is saved during the run,
    /// so closing the app mid-run returns the player to the state before it: closing the game is never a way to
    /// escape the risk, and a run can never leave the save in a half-finished state.
    /// </summary>
    [DefaultExecutionOrder(-100)] // load before the debug starters (items, skills) decide whether to run
    public class GameSaver : MonoBehaviour
    {
        public GameDatabase database;
        public RunLedger ledger;

        [Tooltip("The stash chest's storage.")]
        public Inventory stash;

        [Tooltip("Seconds to wait after a change before writing, so a burst of changes is saved once.")]
        public float saveDelay = 1f;

        private Inventory inventory;
        private Equipment equipment;
        private SkillUnlocks unlocks;
        private SkillLoadout loadout;
        private QuestLog quests;
        private QuickItem quickItem;
        private SaveSerializer serializer;
        private bool loading;
        private bool runWasActive;
        private bool dirty;
        private float timer;

        private void Start()
        {
            inventory = GetComponent<Inventory>();
            equipment = GetComponent<Equipment>();
            unlocks = GetComponent<SkillUnlocks>();
            loadout = GetComponent<SkillLoadout>();
            quests = GetComponent<QuestLog>();
            quickItem = GetComponent<QuickItem>();
            serializer = new SaveSerializer(database, inventory, stash, equipment, unlocks, loadout, quests) { QuickItem = quickItem };

            Load();
            quests.board.Fill(); // a new game, or slots whose saved quest no longer exists, get fresh quests
            Subscribe();
        }

        private void OnDestroy()
        {
            if (serializer == null) return;

            inventory.Changed -= OnInventoryChanged;
            stash.Changed -= OnInventoryChanged;
            equipment.Changed -= MarkDirty;
            quickItem.Changed -= MarkDirty;
            unlocks.Changed -= MarkDirty;
            loadout.Changed -= MarkDirty;
            quests.Changed -= OnQuestChanged;
            quests.board.Changed -= MarkDirty;
            ledger.Changed -= OnLedgerChanged;
        }

        private void Update()
        {
            if (!dirty || ledger.Active) return;

            timer -= Time.unscaledDeltaTime;
            if (timer <= 0f) Save();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused) SaveIfPending();
        }

        private void OnApplicationQuit()
        {
            SaveIfPending();
        }

        private void Load()
        {
            SaveData data = SaveFile.Read();
            if (data == null) return;

            loading = true;
            serializer.Apply(data);
            loading = false;
        }

        private void Subscribe()
        {
            inventory.Changed += OnInventoryChanged;
            stash.Changed += OnInventoryChanged;
            equipment.Changed += MarkDirty;
            quickItem.Changed += MarkDirty;
            unlocks.Changed += MarkDirty;
            loadout.Changed += MarkDirty;
            quests.Changed += OnQuestChanged;
            quests.board.Changed += MarkDirty;
            ledger.Changed += OnLedgerChanged;
        }

        // The ledger also reports every pickup; only the start and the end of a run matter here.
        private void OnLedgerChanged()
        {
            if (ledger.Active == runWasActive) return;

            runWasActive = ledger.Active;
            if (dirty) Save();
        }

        private void OnInventoryChanged(ItemDefinition item, int count) { MarkDirty(); }

        private void OnQuestChanged(QuestDefinition quest) { MarkDirty(); }

        private void MarkDirty()
        {
            if (loading) return;

            dirty = true;
            timer = saveDelay;
        }

        /// <summary>Writes the save right now. Returns false during a run, when saving is not allowed.</summary>
        public bool SaveNow()
        {
            if (ledger.Active) return false;

            Save();
            return true;
        }

        private void SaveIfPending()
        {
            if (dirty && !ledger.Active) Save();
        }

        private void Save()
        {
            dirty = false;
            SaveFile.Write(serializer.Capture());
        }
    }
}
