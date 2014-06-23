using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class SelectEntityUI  : BaseActionUI
    {
        private IGame _game;
        private TileSelectorUI _selector;

        private IEncounter _encounter;

        public SelectEntityUI(IGame game, IEncounter encounter, ICharacter currentPlayer)
        {
            IsMultiStep = false;

            _game = game;
            _encounter = encounter;
            _selector = new TileSelectorUI();

            // interaction only on range 1
            _selector.InitializeRangeCheck(currentPlayer.Position, 1);
        }

        public override void Update()
        {
            _selector.Update();

            if (_selector.IsCurrentPositionValid() && Input.GetMouseButtonDown(0))
            {
                var selectedPosition = _selector.GetCurrentPosition();

                var possibleTargets = _game.GameBoard.GetEntities(selectedPosition);
                if (possibleTargets == null || possibleTargets.Count == 0)
                    return;
                if (possibleTargets.Count > 1)
                    throw new NotSupportedException();

                _encounter.SetEntityContext(possibleTargets.First());

                Stop();
            }
        }

        public override void Stop()
        {
            _selector.Stop();
            IsDone = true;
        }
    }
}
