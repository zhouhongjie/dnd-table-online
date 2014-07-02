using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class MapEditorUI : BaseActionUI
    {
        private IGame _game;
        private TileSelectorUI _selector;
        private EntityTypeEnum _entityType;
        private CharacterTypeEnum _npcType;

        public MapEditorUI(IGame game, EntityTypeEnum entityType)
        {
            _game = game;
            _entityType = entityType;
            _selector = new TileSelectorUI();
        }

        public MapEditorUI(IGame game, CharacterTypeEnum characterType)
        {
            _game = game;
            _entityType = EntityTypeEnum.Character;
            _npcType = characterType;
            _selector = new TileSelectorUI();
        }

        public override void Update()
        {
            _selector.Update();

            // Mark
            var selectedPosition = _selector.GetCurrentPosition();

            // Add/Remove entity
            if (Input.GetMouseButtonDown(0))
            {
                var target = _game.GameBoard.GetEntity(selectedPosition, _entityType);
                if (target == null)
                {
                    if (_entityType == EntityTypeEnum.Character)
                        _game.AddCharacter(Factory.CreateNpc(_npcType), selectedPosition);
                    else
                    _game.AddMapEntity(selectedPosition, _entityType);
                }
                else
                {
                    _game.RemoveMapEntity(selectedPosition);
                }
            }
        }

        public override void Stop()
        {
            IsDone = true;
            _selector.Stop();
        }

    }
}