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
        private List<ICharacter> _selectedAwareCharacters = new List<ICharacter>();
        private List<ICharacter> _selectedUnawareCharacters = new List<ICharacter>();
        private Func<List<ICharacter>, List<ICharacter>, bool> _stopAction;

        public SelectMultipleCharactersUI(IGame game, Func<List<ICharacter>, List<ICharacter>, bool> stopAction)
        {
            IsMultiStep = true;

            _game = game;
            _stopAction = stopAction;
            _selector = new TileSelectorUI();
        }

        public override void Update()
        {
            _selector.Update();

            // Left mouse = awareChars
            // Right mouse = unawareChars
            if (_selector.IsCurrentPositionValid() && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                var selectedPosition = _selector.GetCurrentPosition();

                var selectedChar = _game.GameBoard.GetEntity(selectedPosition, EntityTypeEnum.Character) as ICharacter;
                if (selectedChar == null)
                    return;

                // Remove
                if (_selectedAwareCharacters.Contains(selectedChar))
                    _selectedAwareCharacters.Remove(selectedChar);
                else if (_selectedUnawareCharacters.Contains(selectedChar))
                    _selectedUnawareCharacters.Remove(selectedChar);
                else
                // Add
                {
                    var characterSet = Input.GetMouseButtonDown(0) ? _selectedAwareCharacters : _selectedUnawareCharacters;
                    characterSet.Add(selectedChar);
                }

                // Mark selected chars
                _selector.SetAlreadySelected(
                    _selectedAwareCharacters.Select(c => c.Position).ToList(),
                    _selectedUnawareCharacters.Select(c => c.Position).ToList());
            }
        }

        public override void Stop()
        {
            _stopAction(_selectedAwareCharacters, _selectedUnawareCharacters);

            _selector.Stop();
            IsDone = true;
        }
    }
}
