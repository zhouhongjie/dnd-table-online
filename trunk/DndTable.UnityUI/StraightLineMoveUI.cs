using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class StraightLineMoveUI : BaseActionUI
    {
        private IGame _game;
        private IStraightLineMove _straightLineMoveAction;

        private Position _selectedPosition;
        private TileSelectorUI _selector;

        public StraightLineMoveUI(IGame game, IStraightLineMove straightLineMoveAction, ICharacter attacker)
        {
            _game = game;
            _straightLineMoveAction = straightLineMoveAction;
            _selector = new TileSelectorUI();

            _selector.InitializeRangeCheck(attacker.Position, straightLineMoveAction.MinRange, straightLineMoveAction.MaxRange);
        }

        public override void Update()
        {
            _selector.Update();

            // Can attack?
            if (_selector.IsCurrentPositionValid())
            {
                _selectedPosition = _selector.GetCurrentPosition();

                // Move
                if (Input.GetMouseButtonDown(0))
                {
                    _straightLineMoveAction.Target(_selectedPosition).Do();

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