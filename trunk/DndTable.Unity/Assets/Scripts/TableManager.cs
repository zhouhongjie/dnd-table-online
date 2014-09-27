using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core;
using DndTable.Core.Actions;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;
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
    public Transform ChestTemplate;
    public Transform DoorTemplate;
    public Transform PitTemplate;

    public Camera PlayerCameraTemplate;
    public Transform IndicatorTemplate;


    public IGame Game;
    public IEncounter CurrentEncounter;

    public string Dungeon = "Dungeon1";


    private const int _calculatorWindowWidth = 400;
    private const int _characterMonitorWindowWidth = 150;

    public Transform GetCurrentCharacterTransform()
    {
        if (CurrentPlayer == null)
            return null;

        return _entityIdToTransformMap[CurrentPlayer.Id];
    }

    private BaseActionUI  _currentActionUI;

    private enum ModeEnum
    {
        DM, Player, MapEditor
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

            var allPcs = new List<ICharacter>();

            // Boris
            {
                var boris = Factory.CreateCharacter("Boris", 14, 12);
                boris.SaveCharacterSheet("Boris");
                boris.EquipWeapon(WeaponFactory.Longsword());
                boris.EquipArmor(ArmorFactory.StuddedLeather());
                //boris.Give(WeaponFactory.CrossbowLight());
                Game.AddCharacter(boris, Position.Create(3, 3));
                allPcs.Add(boris);


                boris.Give(PotionFactory.CreatePotionOfCureLightWound());
                //boris.Give(PotionFactory.CreatePotionOfCureLightWound());
            }

            // Maiko
            {
                var maiko = Factory.CreateCharacter("Maiko", 12, 16, 10, 12, 10, 13);
                maiko.SaveCharacterSheet("Maiko");
                maiko.EquipWeapon(WeaponFactory.Longbow());
                maiko.EquipArmor(ArmorFactory.Leather());
                //maiko.Give(WeaponFactory.Rapier());
                maiko.PrepareSpell(SpellFactory.MagicMissile());
                maiko.PrepareSpell(SpellFactory.MagicMissile());
                maiko.PrepareSpell(SpellFactory.SleepArrow());
                maiko.PrepareSpell(SpellFactory.SleepArrow());
                maiko.PrepareSpell(SpellFactory.SleepArrow());
                Game.AddCharacter(maiko, Position.Create(3, 4));
                allPcs.Add(maiko);

                maiko.Give(PotionFactory.CreatePotionOfCureLightWound());
            }

            // Healer
            {
                var healer = Factory.CreateCharacter("Jozan", 12, 12);
                healer.SaveCharacterSheet("Jozan");
                healer.EquipArmor(ArmorFactory.FullPlate());
                healer.EquipWeapon(WeaponFactory.MaceHeavy());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                healer.PrepareSpell(SpellFactory.CureLightWound());
                Game.AddCharacter(healer, Position.Create(3, 5));
                allPcs.Add(healer);

                healer.Give(PotionFactory.CreatePotionOfCureLightWound());
            }

            // Thogeon
            {
                var thogeon = Factory.CreateCharacter("Thogeon", 12, 16, 10, 12, 10, 12);
                thogeon.SaveCharacterSheet("Thogeon");
                thogeon.AddSneakAttackFeat();
                thogeon.EquipArmor(ArmorFactory.Leather());
                thogeon.EquipWeapon(WeaponFactory.Dagger());
                Game.AddCharacter(thogeon, Position.Create(3, 6));
                allPcs.Add(thogeon);
            }

            // Start encounter
            CurrentEncounter = Game.StartEncounter(allPcs);
            //CurrentEncounter = Game.StartEncounter();
        }

	    CreateSupportObjects();
        CreateBoard();
        UpdateEntities();
    }

    // Update is called once per frame
	void Update ()
	{
        // Show all = reset renderers for UpdateFieldOfView
        // This gameObject should be handled first
	    ShowAllRecursive(transform);
        if (CurrentPlayer != null)
            Game.GameBoard.OptimizeFieldOfViewForCurrentPlayer(CurrentPlayer.Position);

        UpdateEntities();
	    UpdateEditorMode();

        //ProcessUserInput();


        if (_currentActionUI != null && !_currentActionUI.IsDone)
            _currentActionUI.Update();

    }

    void LateUpdate()
    {
        UpdateFieldOfView();
    }

    private void UpdateEditorMode()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            StopCurrentAction();
            _mode = ModeEnum.Player;
        }
        else if (Input.GetKey(KeyCode.F2))
        {
            StopCurrentAction();
            _mode = ModeEnum.DM;
        }
        else if (Input.GetKey(KeyCode.F3))
        {
            StopCurrentAction();
            _mode = ModeEnum.MapEditor;
        }
    }

    private void StopCurrentAction()
    {
        if (_currentActionUI != null && !_currentActionUI.IsDone)
            _currentActionUI.Stop();
        _currentActionUI = null;
    }

    public ICharacter CurrentPlayer
    {
        get
        {
            if (CurrentEncounter == null)
                return null;
            return CurrentEncounter.GetCurrentCharacter();
        }
    }

    void OnGUI()
    {
        //if (!Application.isEditor)  // or check the app debug flag
        //    return;

        // See: http://3dgep.com/?p=5169
        GUILayout.Window(0, new Rect(Screen.width - _calculatorWindowWidth - _characterMonitorWindowWidth, 0, 150, 0), UpdateCharacterMonitorUI, "Character monitor");

        GUILayout.Window(1, new Rect(Screen.width - _calculatorWindowWidth, 0, _calculatorWindowWidth, 0), UpdateDiceMonitorUI, "Calculator");

        if (!UpdateMultistepOperation())
        {
            if (_mode == ModeEnum.Player && CurrentPlayer != null)
                GUILayout.Window(2, new Rect(0, 0, 150, 0), UpdatePossibleActionsUI, "Player: " + CurrentPlayer.CharacterSheet.Name);
            else if (_mode == ModeEnum.MapEditor)
                GUILayout.Window(3, new Rect(0, 0, 150, 0), UpdateMapEditorActionsUI, "Map editor");
            else if (_mode == ModeEnum.DM)
                GUILayout.Window(4, new Rect(0, 0, 150, 0), UpdateDMActionsUI, "DM");
        }
    }

    private void UpdateDMActionsUI(int windowId)
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Start encounter"))
        {
            StopCurrentAction();
            _currentActionUI = new SelectMultipleCharactersUI(Game, (awareCharacters, unawareCharacters) =>
                                                                        {
                                                                            if (awareCharacters.Count == 0 && unawareCharacters.Count == 0)
                                                                                return false;

                                                                            CurrentEncounter = Game.StartEncounter(awareCharacters, unawareCharacters);
                                                                            return true;
                                                                        });
        }

        GUILayout.EndVertical();
    }

    private void UpdateMapEditorActionsUI(int windowId)
    {
        GUILayout.BeginVertical();

        {
            if (GUILayout.Button("Save"))
            {
                StopCurrentAction();
                Game.GameBoard.Save(Dungeon);
            }
            if (GUILayout.Button("Walls"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, EntityTypeEnum.Wall);
            }
            if (GUILayout.Button("Chests"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, EntityTypeEnum.Chest);
            }
            if (GUILayout.Button("Doors"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, EntityTypeEnum.Door);
            }
            if (GUILayout.Button("Pits"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, EntityTypeEnum.Pit);
            }
            if (GUILayout.Button("Orc"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, CharacterTypeEnum.Orc);
            }
            if (GUILayout.Button("Orc chief"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, CharacterTypeEnum.OrcChief);
            }
            if (GUILayout.Button("Kobolt"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, CharacterTypeEnum.Kobolt);
            }
            if (GUILayout.Button("Wolf"))
            {
                StopCurrentAction();
                _currentActionUI = new MapEditorUI(Game, CharacterTypeEnum.Wolf);
            }
        }
        GUILayout.EndVertical();
    }

    private bool UpdateMultistepOperation()
    {
        // Check for a Multi-step operation (needs to be stopped before chosing a new action)
        if (_currentActionUI != null && !_currentActionUI.IsDone && _currentActionUI.IsMultiStep)
        {
            GUILayout.BeginVertical();
            GUI.color = Color.yellow;
            if (GUILayout.Button("Stop current action"))
            {
                _currentActionUI.Stop();
            }
            GUILayout.EndVertical();
            return true;
        }

        return false;
    }

    private void UpdatePossibleActionsUI(int windowId)
    {
        if (CurrentPlayer == null)
            return;


        GUILayout.BeginHorizontal();

        // Add control buttons
        {
            GUILayout.BeginVertical();

            // Title
            GUILayout.Label(CurrentPlayer.CharacterSheet.Name);

            if (GUILayout.Button("Select"))
            {
                StopCurrentAction();
                _currentActionUI = new SelectEntityUI(Game, CurrentEncounter, CurrentPlayer);
            }
            if (GUILayout.Button("Next player"))
            {
                StopCurrentAction();
                CurrentEncounter.GetNextCharacter();
            }
            GUILayout.EndVertical();
        }
        
        // Add character actions
        {
            var actionsRemaining = CurrentEncounter.GetPossibleActionsForCurrentCharacter();
            AddButtonsForCategory(ActionCategoryEnum.Combat, ref actionsRemaining);
            AddButtonsForCategory(ActionCategoryEnum.Move, ref actionsRemaining);
            AddButtonsForCategory(ActionCategoryEnum.Spell, ref actionsRemaining);
            AddButtonsForCategory(ActionCategoryEnum.Other, ref actionsRemaining);
            AddButtonsForCategory(ActionCategoryEnum.Context, ref actionsRemaining);

            if (actionsRemaining.Count > 0)
                throw new NotSupportedException("Some ActionCategories are not supported");
        }

        GUILayout.EndHorizontal();
    }

    private void AddButtonsForCategory(ActionCategoryEnum category, ref List<IAction> actions)
    {
        var allActions = new List<IAction>(actions);

        GUILayout.BeginVertical();

        // Title
        GUILayout.Label(Enum.Format(typeof(ActionCategoryEnum), category, "g"));

        foreach (var action in allActions)
        {
            if (action.Category != category)
                continue;

            // Action handled => remove from list
            actions.Remove(action);

            if (GUILayout.Button(action.Description))
            {
                StopCurrentAction();

                if (!action.RequiresUI)
                {
                    action.Do();
                }
                else if (action is IMoveAction)
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
                else if (action is ICastSpellAction)
                {
                    var castSpellAction = action as ICastSpellAction;
                    _currentActionUI = new SelectChararterUI(Game, CurrentPlayer.Position, castSpellAction.MaxRange, 
                        (target) =>
                            {
                                if (target == null)
                                    return false;
                                castSpellAction.Target(target).Do();
                                return true;
                            });
                }
                else
                {
                    throw new NotSupportedException("TODO: UI for " + action);
                }
            }
        }

        GUILayout.EndVertical();
    }

    private void UpdateCharacterMonitorUI(int windowId)
    {
        var label = String.Empty;
        foreach (var character in Game.GetCharacters())
        {
            //GUI.Label(new Rect(0, start, Screen.width, Screen.height), character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp");
            label += character.CharacterSheet.Name + ": " + character.CharacterSheet.HitPoints + "hp" + "\n";

            if (character.CharacterSheet.Conditions.IsSleeping)
                label += " Zzzzz";
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
        // TEMP disable in DM mode
        if (_mode != ModeEnum.Player)
            return;

        if (CurrentPlayer == null)
            return;

        var fieldOfView = Game.GameBoard.GetFieldOfViewForCurrentPlayer();

        for (var i=0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.position.x < 0 || child.position.x > fieldOfView.GetLength(0))
                throw new IndexOutOfRangeException("FieldOfView problem with child '" + child.name + "' @ position.x: " + child.position.x);
            if (child.position.z < 0 || child.position.z > fieldOfView.GetLength(0))
                throw new IndexOutOfRangeException("FieldOfView problem with child '" + child.name + "' @ position.z: " + child.position.z);

            bool isVisible = fieldOfView[(int)child.position.x, (int)child.position.z];

            if (_mode == ModeEnum.Player)
            {
                if (!isVisible)
                    HideAllRecursive(child.transform);
            }
            //if (_mode == ModeEnum.MapEditor)
            //{
            //    SetColorRecursive(child.transform, isVisible ? Color.white : Color.gray);
            //}
        }
    }

    private static void ShowAllRecursive(Transform currentTransform)
    {
        if (currentTransform.renderer != null)
            currentTransform.renderer.enabled = true;

        for (var i = 0; i < currentTransform.childCount; i++)
        {
            var child = currentTransform.GetChild(i);
            ShowAllRecursive(child);
        }
    }

    private static void HideAllRecursive(Transform currentTransform)
    {
        if (currentTransform.renderer != null)
            currentTransform.renderer.enabled = false;

        for (var i = 0; i < currentTransform.childCount; i++)
        {
            var child = currentTransform.GetChild(i);
            HideAllRecursive(child);
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

        foreach (var entity in Game.GameBoard.GetEntities())
        {
            currentEntities.Add(entity.Id);

            // Add new
            if (_entityIdToTransformMap.ContainsKey(entity.Id))
                continue;

            Transform newTransform = null;

            if (entity.EntityType == EntityTypeEnum.Character)
            {
                newTransform = CreateEntity(PlayerTemplate, entity);
            }
            else if (entity.EntityType == EntityTypeEnum.Wall)
            {
                newTransform = CreateEntity(WallTemplate, entity);
            }
            else if (entity.EntityType == EntityTypeEnum.Chest)
            {
                newTransform = CreateEntity(ChestTemplate, entity);
            }
            else if (entity.EntityType == EntityTypeEnum.Door)
            {
                newTransform = CreateEntity(DoorTemplate, entity);
            }
            else if (entity.EntityType == EntityTypeEnum.Pit)
            {
                newTransform = CreateEntity(PitTemplate, entity);
            }
            else
            {
                throw new NotSupportedException("EntityType does not have a template Transform: " + entity.EntityType);
            }

            _entityIdToTransformMap.Add(entity.Id, newTransform);
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

    private Transform CreateEntity(Transform template, IEntity entity)
    {
        Vector3 position = new Vector3(entity.Position.X, 0, entity.Position.Y);
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

        // Door
        {
            var script = newObj.GetComponent("DoorScript") as DoorScript;
            if (script != null)
            {
                script.Door = entity;
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
