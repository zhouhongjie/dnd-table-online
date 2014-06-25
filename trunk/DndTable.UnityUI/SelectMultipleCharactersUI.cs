using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class SelectMultipleCharactersUI : BaseActionUI
    {
        private IGame _game;
        private TileSelectorUI _selector;
        private List<ICharacter> _selectedCharacters = new List<ICharacter>();
        private Func<List<ICharacter>, bool> _stopAction;

        public SelectMultipleCharactersUI(IGame game, Func<List<ICharacter>, bool> stopAction)
        {
            IsMultiStep = true;

            _game = game;
            _stopAction = stopAction;
            _selector = new TileSelectorUI();
        }

        public override void Update()
        {
            _selector.Update();

            if (_selector.IsCurrentPositionValid() && Input.GetMouseButtonDown(0))
            {
                var selectedPosition = _selector.GetCurrentPosition();

                var selectedChar = _game.GameBoard.GetEntity(selectedPosition, EntityTypeEnum.Character) as ICharacter;
                if (selectedChar == null)
                    return;

                if (_selectedCharacters.Contains(selectedChar))
                    _selectedCharacters.Remove(selectedChar);
                else
                    _selectedCharacters.Add(selectedChar);

                // Mark selected chars
                _selector.SetAlreadySelected(_selectedCharacters.Select(c => c.Position).ToList());
            }
        }

        public override void Stop()
        {
            _stopAction(_selectedCharacters);

            _selector.Stop();
            IsDone = true;
        }
    }
}
