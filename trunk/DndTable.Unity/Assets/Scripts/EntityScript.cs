using System;
using System.IO;
using DndTable.Core;
using DndTable.Core.Entities;
using UnityEngine;
using System.Collections;

public class EntityScript : MonoBehaviour {

    public IEntity Entity;
    public IGame Game;

    private LerpInfo _positionLerp;
    private LerpInfo _angleLerp;

	// Use this for initialization
	void Start ()
	{
        _positionLerp = new LerpInfo(transform.position, 5);
        _angleLerp = new LerpInfo(transform.eulerAngles, 5);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Entity == null)
            return;

	    LerpUpdatePosition();
	    UpdateSelectState();
	}

    private bool IsSelected()
    {
        if (Game == null)
            throw new NullReferenceException("Game is not set during creation");
        if (Game.CurrentEncounter == null)
            return false;
        return Game.CurrentEncounter.GetCurrentCharacter() == Entity;
    }

    private bool IsVisibleForCurrent()
    {
        if (Game == null)
            throw new NullReferenceException("Game is not set during creation");
        if (Game.CurrentEncounter == null)
            return false;

        var position = Position.Create((int)transform.position.x, (int)transform.position.z);
        return Game.GameBoard.IsVisibleForCurrentPlayer(position);
    }

    private Transform GetIndicator()
    {
        var indicator = transform.FindChild("Indicator");
        if (indicator == null)
            throw new InvalidOperationException("No Indicator child for the entity");
        return indicator;
    }

    private void UpdateSelectState()
    {
        GetIndicator().renderer.enabled = IsSelected();
    }

    private void LerpUpdatePosition()
    {
        var newPosition = new Vector3(Entity.Position.X, 0, Entity.Position.Y);
        var newAngle = new Vector3(0, (float)RadianToDegree(Entity.Angle), 0);

        transform.position = _positionLerp.UpdateLerp(newPosition);
        transform.eulerAngles = _angleLerp.UpdateLerp(newAngle);
    }

    private static double RadianToDegree(double angle)
    {
        return angle * (180.0 / Math.PI);
    }
}

class LerpInfo
{
    private Vector3 _endMarker;
    private Vector3 _current;

    private Vector3 _step;
    private int _stepCounter;

    private int _nrOfSteps = 1;

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
