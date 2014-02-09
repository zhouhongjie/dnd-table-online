using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class LerpInfo
    {
        private Vector3 _endMarker;
        private Vector3 _current;

        private Vector3 _step;
        private int _stepCounter;

        private readonly int _nrOfSteps;

        public LerpInfo(Vector3 start, int nrOfStep)
        {
            _current = start;
            _nrOfSteps = nrOfStep;

            SetNewTarget(start);
        }

        private void SetNewTarget(Vector3 newTarget)
        {
            if (newTarget == _endMarker)
                return;

            _endMarker = newTarget;

            _step = (_endMarker - _current) / _nrOfSteps;
            _stepCounter = 0;
        }

        public Vector3 UpdateLerp(Vector3 newTarget)
        {
            SetNewTarget(newTarget);

            if (_stepCounter >= _nrOfSteps)
                return _current;

            _current += _step;
            _stepCounter++;

            return _current;
        }
    }
}
