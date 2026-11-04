namespace MobileRpg
{
    /// <summary>
    /// Implemented by the components that perform a skill's movement and damage.
    /// </summary>
    public interface ISkillExecutor
    {
        /// <summary>The skill type this component performs.</summary>
        SkillType Type { get; }

        /// <summary>Performs the skill.</summary>
        /// <param name="skill">The skill being cast.</param>
        /// <param name="damage">Damage already scaled from the character's attack.</param>
        void Execute(SkillDefinition skill, float damage);
    }
}
