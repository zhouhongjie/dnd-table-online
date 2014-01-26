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

        private bool _started = false;
        private List<Position> _path;
        private TileSelectorUI _selector;

        private ICharacter _currentPlayer;
        private IMoveAction _moveAction;

        //private int MaxLength { get { return _currentPlayer.CharacterSheet.Speed/5; }}
        private int MaxLength { get { return 10; } }

        public MoveActionUI(ICharacter currentPlayer, IMoveAction moveAction)
        {
            _currentPlayer = currentPlayer;
            _moveAction = moveAction;
            _selector = new TileSelectorUI();
        }

        private ICharacter GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public override void Update()
        {
            _selector.Update();

            if (Input.GetMouseButtonDown(0))
                StartPath();

            UpdatePath();

            if (Input.GetMouseButtonUp(0))
                EndPath();
        }

        public override void Stop()
        {
            _selector.Stop();
            IsDone = true;
        }

        private void StartPath()
        {
            if (!IsCorrectStartingPosition())
                return;

            _started = true;
            _selector.StartPath();
            _path = new List<Position>();
        }

        private bool IsCorrectStartingPosition()
        {
            // should be 1 tile away from current player
            var currentPosition = _selector.GetCurrentPosition();
            if (currentPosition == null)
                return false;

            var currentPlayer = GetCurrentPlayer();

            if ((currentPlayer.Position.X == currentPosition.X) && (currentPlayer.Position.Y == currentPosition.Y))
                return false;
            if (Math.Abs(currentPlayer.Position.X - currentPosition.X) > 1)
                return false;
            if (Math.Abs(currentPlayer.Position.Y - currentPosition.Y) > 1)
                return false;

            return true;
        }

        private void EndPath()
        {
            if (!_started)
                return;

            _started = false;


            // TODO step by step move

            //foreach (var position in _path)
            {
                _moveAction.Target(_path.Last()).Do();
            }

            _selector.EndPath();

            Stop();
        }

        private void UpdatePath()
        {
            if (!_started)
                return;

            if (_path.Count >= MaxLength)
                return;

            var currentPosition = _selector.GetCurrentPosition();
            if (currentPosition == null)
                return;

            // First
            if (_path.Count == 0)
                _path.Add(currentPosition);
            else
            {
                // Check already part of path
                if (_path.Find(p => (p.X == currentPosition.X) && (p.Y == currentPosition.Y)) != null)
                    return;

                // Check adjacent
                var lastPosition = _path.Last();

                if (Math.Abs(lastPosition.X - currentPosition.X) > 1)
                    return;
                if (Math.Abs(lastPosition.Y - currentPosition.Y) > 1)
                    return;

                _path.Add(currentPosition);
            }

            //MarkTarget(_selector.GetCurrentTile());
        }
    }
}