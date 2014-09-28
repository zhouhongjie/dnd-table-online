namespace DndTable.Core.Armors
{
    public static class ArmorFactory
    {
        public static IArmor Leather()
        {
            var armour = new Armor();

            armour.Description = "Leather armor";
            armour.Proficiency = ArmorProficiencyEnum.Light;
            armour.ArmorBonus = 2;
            armour.MaxDexBonus = 6;
            armour.ArmorCheckPenalty = 0;
            armour.ArcaneSpellFailure = 10;

            return armour;
        }

        public static IArmor StuddedLeather()
        {
            var armour = new Armor();

            armour.Description = "Studded leather armor";
            armour.Proficiency = ArmorProficiencyEnum.Light;
            armour.ArmorBonus = 3;
            armour.MaxDexBonus = 5;
            armour.ArmorCheckPenalty = -1;
            armour.ArcaneSpellFailure = 15;

            return armour;
        }

        public static IArmor ScaleMail()
        {
            var armour = new Armor();

            armour.Description = "Scale mail";
            armour.Proficiency = ArmorProficiencyEnum.Medium;
            armour.ArmorBonus = 4;
            armour.MaxDexBonus = 3;
            armour.ArmorCheckPenalty = 4;
            armour.ArcaneSpellFailure = 25;

            return armour;
        }

        public static IArmor FullPlate()
        {
            var armour = new Armor();

            armour.Description = "Full plate";
            armour.Proficiency = ArmorProficiencyEnum.Heavy;
            armour.ArmorBonus = 8;
            armour.MaxDexBonus = 1;
            armour.ArmorCheckPenalty = 6;
            armour.ArcaneSpellFailure = 35;

            return armour;
        }
    }
}
