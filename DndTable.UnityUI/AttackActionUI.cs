using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class AttackActionUI : BaseActionUI
    {
        private IGame _game;
        private IAttackAction _attackAction;

        private Position _selectedPosition;
        private TileSelectorUI _selector;

        public AttackActionUI(IGame game, IAttackAction attackAction, ICharacter attacker)
        {
            _game = game;
            _attackAction = attackAction;
            _selector = new TileSelectorUI();

            _selector.InitializeRangeCheck(attacker.Position, attackAction.MaxRange);
        }

        public override void Update()
        {
            _selector.Update();

            // Mark
            _selectedPosition = _selector.GetCurrentPosition();

            // Attack
            if (Input.GetMouseButtonDown(0))
            {
                var target = _game.GameBoard.GetEntity(_selectedPosition) as ICharacter;
                if (target != null)
                {
                    _attackAction.Target(target).Do();

                    Stop();
                }
            }
        }

        public override void Stop()
        {
            _selector.Stop();
            IsDone = true;
        }
    }
}