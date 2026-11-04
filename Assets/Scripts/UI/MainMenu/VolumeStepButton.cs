namespace MobileRpg
{
    /// <summary>
    /// Touch button that raises or lowers the music or effects volume by a step.
    /// </summary>
    public class VolumeStepButton : PointerDownAction
    {
        public bool music = true;
        public float step = 0.1f;

        /// <summary>Sets which volume the button changes and by how much.</summary>
        public void Configure(bool changesMusic, float amount)
        {
            music = changesMusic;
            step = amount;
        }

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (music) VolumeSettings.SetMusic(VolumeSettings.Music + step);
            else VolumeSettings.SetSfx(VolumeSettings.Sfx + step);
        }
    }
}
