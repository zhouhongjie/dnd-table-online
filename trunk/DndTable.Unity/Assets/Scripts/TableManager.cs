using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.UnityUI;
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

        //ProcessUserInput();

        if (_moveActionUI != null && !_moveActionUI.IsDone)
            _moveActionUI.Update();
        if (_attackActionUI != null && !_attackActionUI.IsDone)
            _attackActionUI.Update();
	}

    public ICharacter CurrentPlayer { get { return CurrentEncounter.GetCurrentCharacter(); } }

    //private void ProcessUserInput()
    //{
    //    var x = CurrentPlayer.Position.X;
    //    var y = CurrentPlayer.Position.Y;

    //    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
    //    {
    //        y += 1;
    //    }
    //    if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
    //        y -= 1;
    //    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
    //    {
    //        x -= 1;
    //    }
    //    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    //    {
    //        x += 1;
    //    }

    //    Game.ActionFactory.Move(CurrentPlayer).Target(Position.Create(x, y)).Do();
    //}

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

        // CharacterSheetInfo
        if (entity is ICharacter)
        {
            var caracterSheetInfoScript = newObj.GetComponent("CharacterSheetInfo") as CharacterSheetInfo;
            if (caracterSheetInfoScript != null)
            {
                caracterSheetInfoScript.Character = entity as ICharacter;
            }
        }

    }
}
