using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.UnityUI;
using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour 
{
    public int MaxX = 20;
    public int MaxY = 20;
    public Transform TileTemplate;
    public Transform PlayerTemplate;
    public Transform WallTemplate;

    public Camera PlayerCameraTemplate;
    public Transform IndicatorTemplate;


    public IGame Game;
    public IEncounter CurrentEncounter;

    public Transform GetCurrentCharacterTransform()
    {
        if (CurrentPlayer == null)
            return null;

        return _entityIdToTransformMap[CurrentPlayer.Id];
    }

    private BaseActionUI  _currentActionUI;

    private enum ModeEnum
    {
        Player, MapEditor
    }

    private ModeEnum _mode = ModeEnum.Player;

    private Dictionary<int, Transform> _entityIdToTransformMap = new Dictionary<int, Transform>();

	// Use this for initialization
	void Start ()
    {
        Game = Factory.CreateGame(MaxX, MaxY);

        // Temp: manual board setup
        {
            // Players
            var regdar = Factory.CreateCharacter("Regdar");
            var tordek = Factory.CreateCharacter("Tordek");
            Game.EquipWeapon(regdar, WeaponFactory.CrossbowLight());
            Game.EquipWeapon(tordek, WeaponFactory.Dagger());
            Game.AddCharacter(regdar, Position.Create(10, 10));
            Game.AddCharacter(tordek, Position.Create(10, 20));

            // Walls
            Game.AddWall(Position.Create(5, 5));
            Game.AddWall(Position.Create(5, 6));
            Game.AddWall(Position.Create(5, 7));
            Game.AddWall(Position.Create(5, 9));
            Game.AddWall(Position.Create(5, 10));

            // Start encounter
	        CurrentEncounter = Game.StartEncounter(new List<ICharacter>() { regdar, tordek });
        }

	    CreateSupportObjects();
        CreateBoard();
        UpdateEntities();
    }

    // Update is called once per frame
	void Update ()
	{
        UpdateEntities();
	    UpdateEditorMode();

        //ProcessUserInput();
        UpdateFieldOfView();


        if (_currentActionUI != null && !_currentActionUI.IsDone)
            _currentActionUI.Update();

    }

    private void UpdateEditorMode()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            StopCurrentAction();
            _mode = ModeEnum.MapEditor;
        }
        else if (Input.GetKey(KeyCode.F2))
        {
            StopCurrentAction();
            _mode = ModeEnum.Player;
        }
    }

    private void StopCurrentAction()
    {
        if (_currentActionUI != null && !_currentActionUI.IsDone)
            _currentActionUI.Stop();
        _currentActionUI = null;
    }

    public ICharacter CurrentPlayer { get { return CurrentEncounter.GetCurrentCharacter(); } }

    void OnGUI()
    {
        if (!Application.isEditor)  // or check the app debug flag
            return;

        UpdateCharacterMonitorUI();
        UpdateDiceMonitorUI();

        if (_mode == ModeEnum.Player) 
            UpdatePossibleActionsUI();
        else if (_mode == ModeEnum.MapEditor) 
            UpdateMapEditorActionsUI();
    }

    private void UpdateMapEditorActionsUI()
    {
        var offset = 0;
        {
            if (GUI.Button(new Rect(10, 70 + offset, 300, 30), "Walls"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game);
            }
        }     
    }

    private void UpdatePossibleActionsUI()
    {
        // Check for a Multi-step operation (needs to be stopped before chosing a new action)
        if (_currentActionUI != null && !_currentActionUI.IsDone && _currentActionUI.IsMultiStep)
        {
            GUI.color = Color.yellow;
            if (GUI.Button(new Rect(10, 70, 300, 30), "Stop current action"))
            {
                _currentActionUI.Stop();
            }
            return;
        }


        var offset = 0;
        foreach (var action in CurrentEncounter.GetPossibleActionsForCurrentCharacter())
        {
            if (GUI.Button(new Rect(10, 70 + offset, 300, 30), action.Description))
            {
                StopCurrentAction();

                if (action is IMoveAction)
                {
                    _currentActionUI = new MoveActionUI(CurrentPlayer, action as IMoveAction);
                }
                else if (action is IAttackAction)
                {
                    _currentActionUI = new AttackActionUI(Game, action as IAttackAction, CurrentPlayer);
                }
                else if (action is IStraightLineMove)
                {
                    _currentActionUI = new StraightLineMoveUI(Game, action as IStraightLineMove, CurrentPlayer);
                }
                else
                {
                    throw new NotSupportedException("TODO: UI for " + action);
                }
            }

            offset += 35;
        }
        if (GUI.Button(new Rect(10, 70 + offset, 300, 30), "Next player"))
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

    private void UpdateFieldOfView()
    {
        //var fieldOfView = Game.GameBoard.GetFieldOfViewForCurrentPlayer();
        var fieldOfView = Game.GameBoard.GetFieldOfView(CurrentPlayer.Position);

        for (var i=0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.position.x < 0 || child.position.x > fieldOfView.GetLength(0))
                throw new IndexOutOfRangeException("FieldOfView problem with child '" + child.name + "' @ position.x: " + child.position.x);
            if (child.position.z < 0 || child.position.z > fieldOfView.GetLength(0))
                throw new IndexOutOfRangeException("FieldOfView problem with child '" + child.name + "' @ position.z: " + child.position.z);

            bool isVisible = fieldOfView[(int)child.position.x, (int)child.position.z];

            SetColorRecursive(child.transform, isVisible ? Color.white : Color.gray);
        }
    }

    private static void SetColorRecursive(Transform currentTransform, Color color)
    {
        if (currentTransform.renderer != null)
            currentTransform.renderer.material.color = color;

        for (var i = 0; i < currentTransform.childCount; i++)
        {
            var child = currentTransform.GetChild(i);
            SetColorRecursive(child, color);
        }
    }

    private void CreateBoard()
    {
        for (int i=0; i < Game.GameBoard.MaxX; i++)
        {
            for (int j=0; j < Game.GameBoard.MaxY; j++)
            {
                CreateTile(i, j);
            }
        }
    }

    private void UpdateEntities()
    {
        for (int i=0; i < Game.GameBoard.MaxX; i++)
        {
            for (int j=0; j < Game.GameBoard.MaxY; j++)
            {
                var entity = Game.GameBoard.GetEntity(Position.Create(i, j));
                if (entity != null)
                {
                    if (_entityIdToTransformMap.ContainsKey(entity.Id))
                        continue;

                    Transform newTransform = null;

                    if (entity.EntityType == EntityTypeEnum.Character)
                    {
                        newTransform = CreateEntity(PlayerTemplate, i, j, entity);
                    }
                    else if (entity.EntityType == EntityTypeEnum.Wall)
                    {
                        newTransform = CreateEntity(WallTemplate, i, j, entity);
                    }
                    else
                    {
                        throw new NotSupportedException("EntityType does not have a template Transform: " + entity.EntityType);
                    }

                    _entityIdToTransformMap.Add(entity.Id, newTransform);
                }
            }
        }
    }

    private void CreateSupportObjects()
    {
        var indicator = CreateIndicator();
        CreateCamera(indicator);
    }

    private Transform CreateIndicator()
    {
        var newObj = (Transform) Instantiate(IndicatorTemplate);

        // Set as child
        newObj.transform.parent = transform;

        return newObj;
    }

    private void CreateCamera(Transform indicator)
    {
        var position = new Vector3(0, 20, 0);
        var newObj = (Component)Instantiate(PlayerCameraTemplate, position, Quaternion.identity);

        // Do NOT Set as child => will cause problems with FoV markers (camera can be outside the board dimensions)
        // TODO: keep the gameBoard entities & helper entities in different root containers
        //newObj.transform.parent = transform;

        // Point @ indicator
        var cameraScript = newObj.transform.GetComponent("MouseOrbitImproved") as MouseOrbitImproved;
        if (cameraScript != null)
        {
            cameraScript.target = indicator;
        }
    }

    private void CreateTile(int i, int j)
    {
        Vector3 position = new Vector3(i, 0, j);
        var newObj = (Component)Instantiate(TileTemplate, position, Quaternion.identity);

        // Set as child
        newObj.transform.parent = transform;
    }

    private Transform CreateEntity(Transform template, int i, int j, IEntity entity)
    {
        Vector3 position = new Vector3(i, 0, j);
        Transform newObj = (Transform)Instantiate(template, position, Quaternion.identity);

        // Set as child
        newObj.transform.parent = transform;

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
            var charName = (entity as ICharacter).CharacterSheet.Name;
            var texture = Resources.Load<Texture2D>(charName);

            ApplyTextureToChild(newObj, "Body", texture);
            ApplyTextureToChild(newObj, "Foot", texture);

            var characterSheetInfoScript = newObj.GetComponent("CharacterSheetInfo") as CharacterSheetInfo;
            if (characterSheetInfoScript != null)
            {
                characterSheetInfoScript.Character = entity as ICharacter;
            }
        }

        // Wall
        {
            var script = newObj.GetComponent("WallScript") as WallScript;
            if (script != null)
            {
                script.Wall = entity;
                script.GameBoard = Game.GameBoard;
            }
        }

        return newObj;
    }

    private static void ApplyTextureToChild(Transform parent, string childName, Texture2D texture)
    {
        var body = parent.FindChild(childName);
        if (body != null)
            body.renderer.material.mainTexture = texture;
    }
}
