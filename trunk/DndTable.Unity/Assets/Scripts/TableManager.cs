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
    public AttackActionUI _attackActionUI;

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
        if (_attackActionUI != null && !_attackActionUI.IsDone)
            _attackActionUI.Update();
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
        foreach (var character in Game.GetCharacters())
        {
            GUI.Label(new Rect(0, start, Screen.width, Screen.height), character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp");
            start += 10;
        }


        // Show action buttons
        var offset = 0;
        foreach (var action in CurrentEncounter.GetPossibleActionsForCurrentCharacter())
        {
            if (GUI.Button(new Rect(10, 70 + offset, 300, 30), action.GetType().ToString()))
            {
                if (action is IMoveAction)
                    _moveActionUI = new MoveActionUI(CurrentPlayer, action as IMoveAction);
                else if (action is IMeleeAttackAction)
                    _attackActionUI = new AttackActionUI(Game, action as IMeleeAttackAction);
                else if (action is IRangeAttackAction)
                    _attackActionUI = new AttackActionUI(Game, action as IRangeAttackAction);
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

    private bool _started = false;
    private List<Position> _path;
    private TileSelectorUI _selector;

    private ICharacter _currentPlayer;
    private IMoveAction _moveAction;

    //private int MaxLength { get { return _currentPlayer.CharacterSheet.Speed/5; }}
    private int MaxLength { get { return 10; }}

    public bool IsDone { get; private set; }

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

    public void Update()
    {
        _selector.Update();

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
        _selector.Stop();
        IsDone = true;
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

public class AttackActionUI
{
    private IGame _game;
    private IMeleeAttackAction _meleeAttackAction;
    private IRangeAttackAction _rangeAttackAction;

    private Position _selectedPosition;
    private TileSelectorUI _selector;

    public bool IsDone { get; private set; }

    public AttackActionUI(IGame game, IMeleeAttackAction meleeAttackAction)
    {
        _game = game;
        _meleeAttackAction = meleeAttackAction;
        _selector = new TileSelectorUI();
    }

    public AttackActionUI(IGame game, IRangeAttackAction rangeAttackAction)
    {
        _game = game;
        _rangeAttackAction = rangeAttackAction;
        _selector = new TileSelectorUI();
    }

    public void Update()
    {
        _selector.Update();

        // Mark
        _selectedPosition = _selector.GetCurrentPosition();

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            var target = _game.GameBoard.GetEntity(_selectedPosition) as ICharacter;
            if (target != null)
            {
                if (_meleeAttackAction != null)
                    _meleeAttackAction.Target(target).Do();
                if (_rangeAttackAction != null)
                    _rangeAttackAction.Target(target).Do();

                _selector.Stop();
                IsDone = true;
            }
        }
    }
}

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