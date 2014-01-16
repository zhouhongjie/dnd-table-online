using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using UnityEngine;
using System.Collections;

public class MarkPath : MonoBehaviour
{
    public int MaxLength = 10;

    private bool _started = false;
    private List<Position> _path;

    // We expect a TableManager script on this GameObject
    private IGame GetGame()
    {
        var manager = GetComponent<TableManager>();
        return manager.Game;
    }

    // We expect a TableManager script on this GameObject
    private ICharacter GetCurrentPlayer()
    {
        var manager = GetComponent<TableManager>();
        return manager.CurrentPlayer;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartPath();

        UpdatePath();

        if (Input.GetMouseButtonUp(0))
            EndPath();
    }

    private void StartPath()
    {
        if (!IsCorrectStartingPosition())
            return;

        _started = true;
        _path = new List<Position>();
    }

    private bool IsCorrectStartingPosition()
    {
        // Should be a tile
        var currentTile = GetCurrentTile();
        if (currentTile == null)
            return false;

        // should be 1 tile away from current player
        var currentPosition = GetPosition(currentTile);
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

        var game = GetGame();
        var currentPlayer = GetCurrentPlayer();

        // TODO step by step move

        //foreach (var position in _path)
        {
            game.Move(currentPlayer, _path.Last());   
        }
    }

    private void UpdatePath()
    {
        if (!_started)
            return;

        if (_path.Count >= MaxLength)
            return;

        var currentTile = GetCurrentTile();
        if (currentTile == null)
            return;

        var currentPosition = GetPosition(currentTile);

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

        MarkTarget(currentTile);
    }

    private Position GetPosition(Transform tile)
    {
        return Position.Create((int)tile.position.x, (int)tile.position.z);
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
        target.renderer.material.color = Color.red;
        //PlayerAttack pa = (PlayerAttack)GetComponent("PlayerAttack");
        //pa.target = selectedTarget.gameObject;
    }

}
