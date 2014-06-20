using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core;
using DndTable.Core.Entities;
using UnityEngine;

namespace DndTable.UnityUI
{
    public class MapEditorUI : BaseActionUI
    {
        private IGame _game;
        private TileSelectorUI _selector;

        public MapEditorUI(IGame game)
        {
            _game = game;
            _selector = new TileSelectorUI();
        }

        public override void Update()
        {
            _selector.Update();

            // Mark
            var selectedPosition = _selector.GetCurrentPosition();

            // Add/Remove wall
            if (Input.GetMouseButtonDown(0))
            {
                var target = _game.GameBoard.GetEntity(selectedPosition, EntityTypeEnum.Wall);
                if (target == null)
                {
                    _game.AddWall(selectedPosition);
                }
                else
                {
                    if (target.EntityType == EntityTypeEnum.Wall)
                        _game.RemoveWall(selectedPosition);
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