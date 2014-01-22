using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class AttackActionUI
    {
        private IGame _game;
        private IMeleeAttackAction _meleeAttackAction;
        private IRangeAttackAction _rangeAttackAction;

        private Position _selectedPosition;
        private TileSelectorUI _selector;

        public bool IsDone { get; private set; }

        public AttackActionUI(IGame game, IMeleeAttackAction meleeAttackAction)
        {
            _game = game;
            _meleeAttackAction = meleeAttackAction;
            _selector = new TileSelectorUI();
        }

        public AttackActionUI(IGame game, IRangeAttackAction rangeAttackAction)
        {
            _game = game;
            _rangeAttackAction = rangeAttackAction;
            _selector = new TileSelectorUI();
        }

        public void Update()
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
                    if (_meleeAttackAction != null)
                        _meleeAttackAction.Target(target).Do();
                    if (_rangeAttackAction != null)
                        _rangeAttackAction.Target(target).Do();

                    _selector.Stop();
                    IsDone = true;
                }
            }
        }
    }
}