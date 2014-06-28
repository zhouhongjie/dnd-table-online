using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class AttackActionUI : BaseActionUI
    {
        private IGame _game;
        private IAttackAction _attackAction;

        private TileSelectorUI _selector;

        public AttackActionUI(IGame game, IAttackAction attackAction, ICharacter attacker)
        {
            _game = game;
            _attackAction = attackAction;
            _selector = new TileSelectorUI();

            _selector.InitializeRangeCheck(attacker.Position, attackAction.MinRange, attackAction.MaxRange);
        }

        public override void Update()
        {
            _selector.Update();

            // Can attack?
            if (_selector.IsCurrentPositionValid())
            {
                var selectedPosition = _selector.GetCurrentPosition();

                // Attack
                if (Input.GetMouseButtonDown(0))
                {
                    // TODO: needs to be reworked together with the possibility to have multiple entities on 1 tile
                    // Find the correct character to hit.
                    // = a character, and preferably one that is still moving
                   var possibleTargets = _game.GameBoard.GetEntities(selectedPosition);
                    ICharacter target = null;
                    foreach (var current in possibleTargets)
                    {
                        var currentCharacter = current as ICharacter;
                        if (currentCharacter != null)
                        {
                            // A character!
                            target = currentCharacter;

                            // Someone worth hitting!
                            if (target.CharacterSheet.CanAct())
                                break;
                        }
                    }

                    if (target != null)
                    {
                        _attackAction.Target(target).Do();

                        Stop();
                    }
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