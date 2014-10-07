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
        public SizeEnum Size { get; set; }

        public int BaseAttackBonus { get; set; }


        public void CopyFrom(CharacterSheet sheet)
        {
            Name = sheet.Name;
            Race = sheet.Race;
            FactionId = sheet.FactionId;

            Strength = sheet.StrengthAttribute.BaseStat;
            Dexterity = sheet.DexterityAttribute.BaseStat;
            Constitution = sheet.ConstitutionAttribute.BaseStat;
            Intelligence = sheet.IntelligenceAttribute.BaseStat;
            Wisdom = sheet.WisdomAttribute.BaseStat;
            Charisma = sheet.CharismaAttribute.BaseStat;

            Fortitude = sheet.FortitudeProperty.BaseValue;
            Reflex = sheet.ReflexProperty.BaseValue;
            Will = sheet.WillProperty.BaseValue;

            MaxHitPoints = sheet.MaxHitPoints;

            Speed = sheet.Speed;
            Size = sheet.Size;

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

            sheet.FortitudeProperty.BaseValue = Fortitude;
            sheet.ReflexProperty.BaseValue = Reflex;
            sheet.WillProperty.BaseValue = Will;

            sheet.MaxHpProperty.BaseValue = MaxHitPoints;
            sheet.HpProperty.BaseValue = MaxHitPoints;         // Set to MaxHitPoints

            sheet.Speed = Speed;
            sheet.Size = Size;

            sheet.BaseAttackBonus = BaseAttackBonus;
        }
    }
}
