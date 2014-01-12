using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public class CharacterSheet : ICharacterSheet
    {
        #region ICharacterSheet Members

        public int Strength
        {
            get { throw new NotImplementedException(); }
        }

        public int Dexterity
        {
            get { throw new NotImplementedException(); }
        }

        public int Constitution
        {
            get { throw new NotImplementedException(); }
        }

        public int Intelligent
        {
            get { throw new NotImplementedException(); }
        }

        public int Wisdom
        {
            get { throw new NotImplementedException(); }
        }

        public int Charisma
        {
            get { throw new NotImplementedException(); }
        }

        public int Fortitude
        {
            get { throw new NotImplementedException(); }
        }

        public int Reflex
        {
            get { throw new NotImplementedException(); }
        }

        public int Will
        {
            get { throw new NotImplementedException(); }
        }

        public int HitPoints
        {
            get { throw new NotImplementedException(); }
        }

        public int ArmourClass
        {
            get { throw new NotImplementedException(); }
        }

        public int Initiative
        {
            get { throw new NotImplementedException(); }
        }

        public int Speed
        {
            get { throw new NotImplementedException(); }
        }

        public int BaseAttackBonus
        {
            get { throw new NotImplementedException(); }
        }

        public int MeleeAttackBonus
        {
            get { throw new NotImplementedException(); }
        }

        public int RangedAttackBonus
        {
            get { throw new NotImplementedException(); }
        }

        public IArmour EquipedArmour
        {
            get { throw new NotImplementedException(); }
        }

        public IWeapon EquipedWeapon
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
