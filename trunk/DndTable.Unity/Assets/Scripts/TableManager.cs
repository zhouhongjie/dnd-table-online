using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
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

    public BaseActionUI  _currentActionUI;

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

        if (_currentActionUI != null && !_currentActionUI.IsDone)
            _currentActionUI.Update();
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

        UpdateCharacterMonitorUI();
        UpdateDiceMonitorUI();
        UpdatePossibleActionsUI();
    }

    private void UpdatePossibleActionsUI()
    {
        var offset = 0;
        foreach (var action in CurrentEncounter.GetPossibleActionsForCurrentCharacter())
        {
            if (GUI.Button(new Rect(10, 70 + offset, 300, 30), action.Description))
            {
                if (action is IMoveAction)
                {
                    if (_currentActionUI != null)
                        _currentActionUI.Stop();
                    _currentActionUI = new MoveActionUI(CurrentPlayer, action as IMoveAction);
                }
                else if (action is IAttackAction)
                {
                    if (_currentActionUI != null)
                        _currentActionUI.Stop();
                    _currentActionUI = new AttackActionUI(Game, action as IAttackAction, CurrentPlayer);
                }
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

    private void UpdateCharacterMonitorUI()
    {
        var height = 0;
        var label = String.Empty;
        foreach (var character in Game.GetCharacters())
        {
            //GUI.Label(new Rect(0, start, Screen.width, Screen.height), character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp");
            label += character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp" + "\n";
            height += 20;
        }
        GUI.Box(new Rect(0, 0, 200, height), label);
    }

    private void UpdateDiceMonitorUI()
    {
        var height = 0;
        var label = String.Empty;
        foreach (var roll in Game.DiceMonitor.GetLastRolls(10))
        {
            var currentLine = roll.Description;

            label += currentLine + "\n";
            height += 18;
        }

        const int width = 500;

        GUI.Box(new Rect(Screen.width - width, 0, width, height), label);
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
