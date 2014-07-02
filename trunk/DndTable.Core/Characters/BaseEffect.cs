namespace DndTable.Core.Characters
{
    internal abstract class BaseEffect
    {
        protected CharacterSheet Sheet { get; private set; }
        protected int DurationInRounds { get; private set; }

        internal BaseEffect(CharacterSheet sheet, int durationInRounds)
        {
            Sheet = sheet;
            DurationInRounds = durationInRounds;
        }

        internal abstract void Apply();

        internal virtual bool CancelOnDamage { get { return false; } }

        internal bool DecreaseDurationAndCheck()
        {
            DurationInRounds--;
            return DurationInRounds >= 0;
        }
    }
}