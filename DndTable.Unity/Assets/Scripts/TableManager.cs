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

    public string Dungeon = "Dungeon1";


    private const int _calculatorWindowWidth = 400;

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
            // Load dungeon
            Game.GameBoard.Load(Dungeon);

            // Players
            //var regdar = Factory.CreateCharacter("Regdar");
            //var tordek = Factory.CreateCharacter("Tordek");
            var boris = Factory.CreateCharacter("Boris");
            var maiko = Factory.CreateCharacter("Maiko");

            Game.EquipWeapon(boris, WeaponFactory.Longsword());
            Game.EquipWeapon(maiko, WeaponFactory.Longbow());
            Game.EquipArmor(boris, ArmorFactory.Leather());
            Game.EquipArmor(maiko, ArmorFactory.Leather());

            Game.AddCharacter(boris, Position.Create(10, 10));
            Game.AddCharacter(maiko, Position.Create(10, 12));

            // Orcs
            Game.AddCharacter(Factory.CreateOrc(), Position.Create(20, 20));
            Game.AddCharacter(Factory.CreateOrc(), Position.Create(20, 21));
            Game.AddCharacter(Factory.CreateOrc(), Position.Create(20, 22));

            // Build dungeon
            // Walls
            //Game.AddWall(Position.Create(5, 5));
            //Game.AddWall(Position.Create(5, 6));
            //Game.AddWall(Position.Create(5, 7));
            //Game.AddWall(Position.Create(5, 9));
            //Game.AddWall(Position.Create(5, 10));

            // Start encounter
	        CurrentEncounter = Game.StartEncounter();
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
            Game.GameBoard.Save(Dungeon);
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
        //if (!Application.isEditor)  // or check the app debug flag
        //    return;

        // See: http://3dgep.com/?p=5169
        GUILayout.Window(0, new Rect(0, 0, 150, 0), UpdateCharacterMonitorUI, "Character monitor");

        GUILayout.Window(1, new Rect(Screen.width - _calculatorWindowWidth, 0, _calculatorWindowWidth, 0), UpdateDiceMonitorUI, "Calculator");

        if (_mode == ModeEnum.Player)
            GUILayout.Window(2, new Rect(0, 150, 150, 0), UpdatePossibleActionsUI, "Actions");
        else if (_mode == ModeEnum.MapEditor)
            GUILayout.Window(3, new Rect(0, 150, 150, 0), UpdateMapEditorActionsUI, "Map editor");
    }

    private void UpdateMapEditorActionsUI(int windowId)
    {
        GUILayout.BeginVertical();

        var offset = 0;
        {
            if (GUILayout.Button("Walls"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game);
            }
        }
        GUILayout.EndVertical();
    }

    private void UpdatePossibleActionsUI(int windowId)
    {
        GUILayout.BeginVertical();

        // Check for a Multi-step operation (needs to be stopped before chosing a new action)
        if (_currentActionUI != null && !_currentActionUI.IsDone && _currentActionUI.IsMultiStep)
        {
            GUI.color = Color.yellow;
            if (GUILayout.Button("Stop current action"))
            {
                _currentActionUI.Stop();
            }
            GUILayout.EndVertical();
            return;
        }


        var offset = 0;
        foreach (var action in CurrentEncounter.GetPossibleActionsForCurrentCharacter())
        {
            if (GUILayout.Button(action.Description))
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
        if (GUILayout.Button("Next player"))
            CurrentEncounter.GetNextCharacter();

        
        GUILayout.EndVertical();
    }

    private void UpdateCharacterMonitorUI(int windowId)
    {
        var label = String.Empty;
        foreach (var character in Game.GetCharacters())
        {
            //GUI.Label(new Rect(0, start, Screen.width, Screen.height), character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp");
            label += character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp" + "\n";
        }
        GUILayout.Label(label);
    }

    private void UpdateDiceMonitorUI(int windowId)
    {
        var label = String.Empty;
        foreach (var message in Game.Logger.GetLast(15))
        {
            label += message + "\n";
            //GUILayout.Label(message, GUILayout.Height(15));
        }

        GUILayout.Label(label);
        //GUILayout.Box(label);
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
        var currentEntities = new HashSet<int>();

        for (int i=0; i < Game.GameBoard.MaxX; i++)
        {
            for (int j=0; j < Game.GameBoard.MaxY; j++)
            {
                var entity = Game.GameBoard.GetEntity(Position.Create(i, j));
                if (entity != null)
                {
                    currentEntities.Add(entity.Id);

                    // Add new
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

        // Delete no longer existing
        var transformMapCopy = new Dictionary<int, Transform>(_entityIdToTransformMap);
        foreach (var kvp in transformMapCopy)
        {
            if (currentEntities.Contains(kvp.Key))
                continue;

            _entityIdToTransformMap.Remove(kvp.Key);
            Destroy(kvp.Value.gameObject);
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
