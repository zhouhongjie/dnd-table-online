using System;
using System.Collections.Generic;
using System.Text;
using DndTable.Core;
using UnityEngine;

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

        public void InitializeRangeCheck(Position center, int rangeInTiles)
        {
            _doRangeCheck = true;
            _rangeCheckCenter = center;
            _rangeCheckMaxRange = rangeInTiles;
        }

        public void Update()
        {
            var currentTransform = GetCurrentTile();
            if (currentTransform != _lastTransform && !_pathMode)
                RevertToOriginalColors();

            if (currentTransform != null)
            {
                MarkTarget(currentTransform);
                _lastTransform = currentTransform;
                // _currentTarget = GetCharacter from transform
            }
        }

        public Position GetCurrentPosition()
        {
            if (_lastTransform == null)
                return null;
            return Position.Create((int)_lastTransform.position.x, (int)_lastTransform.position.z);
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
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
                return hit.transform;
            return null;
        }

        private void MarkTarget(Transform target)
        {
            if (!_originalColors.ContainsKey(target))
                _originalColors.Add(target, target.renderer.material.color);

            var markColor = IsRangeValid(target) ? Color.green : Color.red;
            target.renderer.material.color = markColor;
        }

        private bool IsRangeValid(Transform target)
        {
            if (!_doRangeCheck)
                return true;

            var distance = GetDistance(_rangeCheckCenter, Position.Create((int)target.position.x, (int)target.position.z));
            var distanceRounded = (int)Math.Floor(distance);

            return distanceRounded <= _rangeCheckMaxRange;
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
