using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class Game : IGame
    {
        public IBoard GameBoard { get { return _gameBoard; } }
        private Board _gameBoard;

        public IDiceMonitor DiceMonitor { get { return _diceRoller; } }
        private DiceRoller _diceRoller;

        private List<ICharacter> _characters = new List<ICharacter>();

        public Game(Board board, DiceRoller diceRoller)
        {
            _gameBoard = board;
            _diceRoller = diceRoller;
        }

        public bool AddCharacter(ICharacter character, Position position)
        {
            if (_characters.Contains(character))
                return false;

            if (!_gameBoard.AddEntity(character, position))
                return false;

            _characters.Add(character);
            return true;
        }

        public List<ICharacter> GetCharacters()
        {
            return _characters;
        }

        public void MeleeAttack(ICharacter attacker, ICharacter target)
        {
            // Has weapon?
            if (attacker.CharacterSheet.EquipedWeapon == null)
                throw new ArgumentException("attacker has no equiped weapon");

            // Can reach


            // Check hit
            if (!_diceRoller.Check(DiceRollEnum.Attack, 20, attacker.CharacterSheet.MeleeAttackBonus, target.CharacterSheet.ArmourClass))
                return;

            // Check crit failure

            // Check crit

            // Do damage
            var damage = _diceRoller.Roll(DiceRollEnum.Damage, attacker.CharacterSheet.EquipedWeapon.DamageD, attacker.CharacterSheet.CurrentMeleeDamageBonus);
            if (damage < 1)
                damage = 1;
            GetEditableSheet(target).HitPoints -= damage;
        }

        public void Move(ICharacter character, Position to)
        {
            // Can move

            // Can move to this point

            

            // TEMP
            _gameBoard.MoveEntity(character.Position, to);
        }

        public void EquipWeapon(ICharacter character, IWeapon weapon)
        {
            GetEditableSheet(character).EquipedWeapon = weapon;
        }

        private static CharacterSheet GetEditableSheet(ICharacter character)
        {
            var sheet = character.CharacterSheet as CharacterSheet;
            if (sheet == null)
                throw new ArgumentException();
            return sheet;
        }
    }
}
