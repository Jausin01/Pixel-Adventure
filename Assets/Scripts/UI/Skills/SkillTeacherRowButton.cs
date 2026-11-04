namespace MobileRpg
{
    /// <summary>
    /// Touch button (a skill row on the teacher's screen): learns the skill if it is not known yet,
    /// otherwise puts it in a skill slot or takes it out.
    /// </summary>
    public class SkillTeacherRowButton : PointerDownAction
    {
        public SkillDefinition skill;
        public SkillTeacher teacher;
        public SkillUnlocks unlocks;
        public SkillLoadout loadout;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (!unlocks.IsUnlocked(skill)) teacher.TryLearn(skill);
            else loadout.Toggle(skill);
        }
    }
}
