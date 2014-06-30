using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Persistence
{
    [Serializable()]
    public class CharacterSheetXml
    {
        public string Name { get; set; }
        CharacterRace Race { get; set; }
        public int FactionId { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int Fortitude { get; set; }
        public int Reflex { get; set; }
        public int Will { get; set; }

        public int MaxHitPoints { get; set; }

        public int Speed { get; set; }
        public int SizeModifier { get; set; }

        public int BaseAttackBonus { get; set; }


        public void CopyFrom(CharacterSheet sheet)
        {
            Name = sheet.Name;
            Race = sheet.Race;
            FactionId = sheet.FactionId;

            Strength = sheet.StrengthAttribute.GetValue();
            Dexterity = sheet.DexterityAttribute.GetValue();
            Constitution = sheet.ConstitutionAttribute.GetValue();
            Intelligence = sheet.IntelligenceAttribute.GetValue();
            Wisdom = sheet.WisdomAttribute.GetValue();
            Charisma = sheet.CharismaAttribute.GetValue();

            Fortitude = sheet.Fortitude;
            Reflex = sheet.Reflex;
            Will = sheet.Will;

            MaxHitPoints = sheet.MaxHitPoints;

            Speed = sheet.Speed;
            SizeModifier = sheet.SizeModifier;

            BaseAttackBonus = sheet.BaseAttackBonus;
        }

        public void CopyTo(CharacterSheet sheet)
        {
            sheet.Name = Name;
            sheet.Race = Race;
            sheet.FactionId = FactionId;

            sheet.StrengthAttribute.SetValue(Strength);
            sheet.DexterityAttribute.SetValue(Dexterity);
            sheet.ConstitutionAttribute.SetValue(Constitution);
            sheet.IntelligenceAttribute.SetValue(Intelligence);
            sheet.WisdomAttribute.SetValue(Wisdom);
            sheet.CharismaAttribute.SetValue(Charisma);

            sheet.Fortitude = Fortitude;
            sheet.Reflex = Reflex;
            sheet.Will = Will;

            sheet.MaxHitPoints = MaxHitPoints;
            sheet.HitPoints = MaxHitPoints;         // Set to MaxHitPoints

            sheet.Speed = Speed;
            sheet.SizeModifier = SizeModifier;

            sheet.BaseAttackBonus = BaseAttackBonus;
        }
    }
}
