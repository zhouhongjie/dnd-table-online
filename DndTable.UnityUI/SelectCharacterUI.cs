using System;
using System.Collections.Generic;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class SelectChararterUI : BaseActionUI
    {
        private IGame _game;
        private Func<ICharacter, bool> _stopAction;
        private ICharacter _selectedTarget = null;

        private TileSelectorUI _selector;

        public SelectChararterUI(IGame game, Position center, int maxRange, Func<ICharacter, bool> stopAction)
        {
            _game = game;
            _stopAction = stopAction;

            _selector = new TileSelectorUI();
            _selector.InitializeRangeCheck(center, 0, maxRange);
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
                    var target = _game.GameBoard.GetEntity(selectedPosition, EntityTypeEnum.Character) as ICharacter;
                    if (target != null)
                    {
                        _selectedTarget = target;

                        Stop();
                    }
                }
            }
        }

        public override void Stop()
        {
            if (_selectedTarget != null)
                _stopAction(_selectedTarget);

            _selector.Stop();
            IsDone = true;
        }
    }
}