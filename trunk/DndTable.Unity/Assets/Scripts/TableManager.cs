using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour 
{
    public int MaxX = 100;
    public int MaxY = 100;
    public Transform TileTemplate;
    public Transform PlayerTemplate;


    public IGame Game;
    public IEncounter CurrentEncounter;

    public MoveActionUI _moveActionUI;

	// Use this for initialization
	void Start ()
	{
        Game = Factory.CreateGame(MaxX, MaxY);

        // Temp
	    var regdar = Factory.CreateCharacter("Regdar");
	    var tordek = Factory.CreateCharacter("Tordek");
        Game.EquipWeapon(regdar, WeaponFactory.CrossbowLight());
        Game.EquipWeapon(tordek, WeaponFactory.Dagger());
        Game.AddCharacter(regdar, Position.Create(10, 10));
        Game.AddCharacter(tordek, Position.Create(10, 20));

	    CurrentEncounter = Game.StartEncounter(new List<ICharacter>() {regdar, tordek});

	    CreateBoard();
	}
	
	// Update is called once per frame
	void Update () {

        ProcessUserInput();

        if (_moveActionUI != null && !_moveActionUI.IsDone)
            _moveActionUI.Update();
	}

    public ICharacter CurrentPlayer { get { return CurrentEncounter.GetCurrentCharacter(); } }

    private void ProcessUserInput()
    {
        var x = CurrentPlayer.Position.X;
        var y = CurrentPlayer.Position.Y;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
        {
            x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            x += 1;
        }

        Game.ActionFactory.Move(CurrentPlayer).Target(Position.Create(x, y)).Do();
    }

    void OnGUI()
    {
        if (!Application.isEditor)  // or check the app debug flag
            return;

        var start = 10;
        GUI.Label(new Rect(0, start + 0, Screen.width, Screen.height), "x: " + CurrentPlayer.Position.X);
        GUI.Label(new Rect(0, start + 10, Screen.width, Screen.height), "y: " + CurrentPlayer.Position.Y);


        // Show action buttons
        var offset = 0;
        foreach (var action in CurrentEncounter.GetPossibleActionsForCurrentCharacter())
        {
            if (GUI.Button(new Rect(10, 70 + offset, 300, 30), "Click " + action.GetType()))
            {
                if (action is IMoveAction)
                    _moveActionUI = new MoveActionUI(CurrentPlayer, action as IMoveAction);
                else
                {
                    throw new NotSupportedException("TODO: UI for " + action);
                }
            }

            offset += 35;
        }
        if (GUI.Button(new Rect(10, 70 + offset, 300, 30), "Click next player"))
            CurrentEncounter.GetNextCharacter();
    }

    private void CreateBoard()
    {
        for (int i=0; i < Game.GameBoard.MaxX; i++)
        {
            for (int j=0; j < Game.GameBoard.MaxY; j++)
            {
                CreateTile(i, j);

                var entity = Game.GameBoard.GetEntity(Position.Create(i, j));
                if (entity != null)
                {
                    if (entity.EntityType == EntityTypeEnum.Character)
                    {
                        CreateEntity(PlayerTemplate, i, j, entity);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }
    }

    private void CreateTile(int i, int j)
    {
        Vector3 position = new Vector3(i, 0, j);
        Transform newObj = (Transform)Instantiate(TileTemplate, position, Quaternion.identity);
    }

    private void CreateEntity(Transform template, int i, int j, IEntity entity)
    {
        Vector3 position = new Vector3(i, 0, j);
        Transform newObj = (Transform)Instantiate(template, position, Quaternion.identity);

        // Entity
        {
            var entityScript = newObj.GetComponent("EntityScript") as EntityScript;
            if (entityScript != null)
            {
                entityScript.Entity = entity;
                entityScript.Game = Game;
            }
        }

    }
}

public class MoveActionUI
{
    public int MaxLength = 10;

    private bool _started = false;
    private List<Position> _path;
    private Dictionary<Transform, Color> _originalColors = new Dictionary<Transform, Color>();

    private ICharacter _currentPlayer;
    private IMoveAction _moveAction;

    public bool IsDone { get; private set; }

    public MoveActionUI(ICharacter currentPlayer, IMoveAction moveAction)
    {
        _currentPlayer = currentPlayer;
        _moveAction = moveAction;  
    }

    private ICharacter GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public void Update()
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

      
        // TODO step by step move

        //foreach (var position in _path)
        {
            _moveAction.Target(_path.Last()).Do();
        }

        RevertToOriginalColors();
        IsDone = true;
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