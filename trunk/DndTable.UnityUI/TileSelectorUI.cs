using System;
using System.Collections.Generic;
using System.Text;
using DndTable.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DndTable.UnityUI
{
    public class TileSelectorUI
    {
        private Dictionary<Transform, Color> _originalColors = new Dictionary<Transform, Color>();
        private Transform _lastTransform;
        private bool _pathMode;

        private bool _doRangeCheck;
        private Position _rangeCheckCenter;
        private int _rangeCheckMaxRange;
        private int _rangeCheckMinRange = 1;

        private List<Position> _alreadySelectedTiles;
        private List<Position> _alreadySelectedTiles2;

        public void InitializeRangeCheck(Position center, int rangeInTiles)
        {
            _doRangeCheck = true;
            _rangeCheckCenter = center;
            _rangeCheckMaxRange = rangeInTiles;
        }

        public void InitializeRangeCheck(Position center, int minRangeInTiles, int maxRangeInTiles)
        {
            _doRangeCheck = true;
            _rangeCheckCenter = center;
            _rangeCheckMinRange = minRangeInTiles;
            _rangeCheckMaxRange = maxRangeInTiles;
        }

        public void Update()
        {
            var currentTransform = GetCurrentTile();
            if (currentTransform != _lastTransform && !_pathMode)
                RevertToOriginalColors();

            if (currentTransform != null)
            {
                var markColor = IsRangeValid(currentTransform) ? Color.green : Color.red;
                MarkTile(currentTransform, markColor);
            }
            _lastTransform = currentTransform;

            // Mark selectedTiles
            if (_alreadySelectedTiles != null)
            {
                foreach (var selected in _alreadySelectedTiles)
                    MarkTile(FindTileOnPosition(selected), Color.yellow);
            }
            if (_alreadySelectedTiles2 != null)
            {
                foreach (var selected in _alreadySelectedTiles2)
                    MarkTile(FindTileOnPosition(selected), Color.blue);
            }
        }

        public Position GetCurrentPosition()
        {
            return GetPosition(_lastTransform);
        }

        public void SetAlreadySelected(List<Position> tilePositions, List<Position> tilePositions2 = null)
        {
            _alreadySelectedTiles = tilePositions;
            _alreadySelectedTiles2 = tilePositions2;
        }

        private static Position GetPosition(Transform transform)
        {
            if (transform == null)
                return null;
            return Position.Create((int)transform.position.x, (int)transform.position.z);
        }

        public bool IsCurrentPositionValid()
        {
             if (_lastTransform == null)
                return false;
            return IsRangeValid(_lastTransform);
        }

        public void Stop()
        {
            RevertToOriginalColors();
            _lastTransform = null;
        }

        public void StartPath()
        {
            _pathMode = true;
        }

        public void EndPath()
        {
            _pathMode = false;
        }

        private Transform GetCurrentTile()
        {
            RaycastHit hit; // cast a ray from mouse pointer:
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return null;

            // Hit is a tile
            if (hit.transform.CompareTag("Tile"))
                return hit.transform;

            // Hit is another object => find tile
            var position = GetPosition(hit.transform);
            var tile = FindTileOnPosition(position);
            return tile;
        }

        private Transform FindTileOnPosition(Position position)
        {
            var objs = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in objs)
            {
                if (go.CompareTag(("Tile")))
                {
                    var currentPosition = GetPositionOfTransform(go.transform);
                    if (position == currentPosition)
                    {
                        return go.transform;
                    }
                }
            }
            return null;
        }

        private static Position GetPositionOfTransform(Transform transform)
        {
            return Position.Create((int)transform.position.x, (int)transform.position.z);
        }

        private void MarkTile(Transform target, Color markColor)
        {
            if (!_originalColors.ContainsKey(target))
                _originalColors.Add(target, target.renderer.material.color);

            target.renderer.material.color = markColor;
        }

        private bool IsRangeValid(Transform target)
        {
            if (!_doRangeCheck)
                return true;

            var distance = GetDistance(_rangeCheckCenter, Position.Create((int)target.position.x, (int)target.position.z));
            var distanceRounded = (int)Math.Floor(distance);

            return distanceRounded <= _rangeCheckMaxRange &&
                   distanceRounded >= _rangeCheckMinRange;
        }

        protected static double GetDistance(Position position1, Position position2)
        {
            var dx = position1.X - position2.X;
            var dy = position1.Y - position2.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void RevertToOriginalColors()
        {
            foreach (var kvp in _originalColors)
            {
                kvp.Key.renderer.material.color = kvp.Value;
            }
            _originalColors.Clear();
        }
    }
}
