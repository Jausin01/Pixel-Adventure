namespace MobileRpg
{
    /// <summary>
    /// Touch button on the doctor's screen: pays for the treatment.
    /// </summary>
    public class DoctorHealButton : PointerDownAction
    {
        public Doctor doctor;

        /// <inheritdoc/>
        protected override void Execute()
        {
            doctor.TryTreat();
        }
    }
}
