namespace MobileRpg
{
    /// <summary>
    /// Which behavior a skill runs. Each value is implemented by one <see cref="ISkillExecutor"/>. Append new values only.
    /// </summary>
    public enum SkillType
    {
        SpinSlash = 0,
        PiercingThrust = 1,
        SwordThrow = 2,
        GroundSlam = 3,
        HatchetVolley = 4,
        SkullSplitter = 5,
        Javelin = 6,
        TripleThrust = 7,
        SpearRain = 8,
        Multishot = 9,
        PiercingShot = 10,
        FanOfKnives = 11,
        ShadowFlurry = 12,
        SeismicSlam = 13,
        TitanSmash = 14,
        Fireball = 15,
        ChainLightning = 16,
        MeteorShower = 17
    }
}
