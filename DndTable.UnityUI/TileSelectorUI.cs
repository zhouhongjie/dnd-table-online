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

        public void Update()
        {
            Console.WriteLine("ping");

            var currentCharacterTransform = GetCurrentTile();
            if (currentCharacterTransform != _lastTransform && !_pathMode)
                RevertToOriginalColors();

            if (currentCharacterTransform != null)
            {
                MarkTarget(currentCharacterTransform);
                _lastTransform = currentCharacterTransform;
                // _currentTarget = GetCharacter from transform
            }
        }

        public Position GetCurrentPosition()
        {
            if (_lastTransform == null)
                return null;
            return Position.Create((int)_lastTransform.position.x, (int)_lastTransform.position.z);
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

            target.renderer.material.color = Color.red;
        }

        private void RevertToOriginalColors()
        {
            foreach (var kvp in _originalColors)
            {
                kvp.Key.renderer.material.color = kvp.Value;
            }
        }

    }
}
