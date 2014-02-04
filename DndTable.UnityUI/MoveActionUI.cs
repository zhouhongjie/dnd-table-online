using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class MoveActionUI : BaseActionUI
    {
        private TileSelectorUI _selector;

        private ICharacter _currentPlayer;
        private IMoveAction _moveAction;

        private int _nrOfStepsCounter = 0;
        private int _maxNrOfSteps = 0;

        public MoveActionUI(ICharacter currentPlayer, IMoveAction moveAction)
        {
            _currentPlayer = currentPlayer;
            _moveAction = moveAction;
            _selector = new TileSelectorUI();
            _maxNrOfSteps = _currentPlayer.CharacterSheet.Speed/5;

            // One by one steps
            _selector.InitializeRangeCheck(currentPlayer.Position, 1);
        }

        public override void Update()
        {
            _selector.Update();

            if (_selector.IsCurrentPositionValid() && Input.GetMouseButtonDown(0))
            {
                var newPosition = _selector.GetCurrentPosition();

                _moveAction.DoOneStep(newPosition);
                _nrOfStepsCounter++;

                _selector.InitializeRangeCheck(newPosition, 1);

                // Max nr of tiles moved
                if (_nrOfStepsCounter >= _maxNrOfSteps)
                {
                    Stop();
                }
            }
        }

        public override void Stop()
        {
            // Register move when we have actually moved
            if (_nrOfStepsCounter > 0)
                _moveAction.Do();


            _selector.Stop();
            IsDone = true;
        }

  
    }
}