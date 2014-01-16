using System;
using DndTable.Core;
using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour 
{
    public int MaxX = 100;
    public int MaxY = 100;
    public Transform TileTemplate;
    public Transform PlayerTemplate;


    public IGame Game;
    public ICharacter CurrentPlayer;


	// Use this for initialization
	void Start ()
	{
        Game = Factory.CreateGame(MaxX, MaxY);

        // Temp
	    CurrentPlayer = Factory.CreateCharacter("Regdar");
        Game.AddCharacter(CurrentPlayer, Position.Create(10, 10));

	    CreateBoard();
	}
	
	// Update is called once per frame
	void Update () {

        ProcessUserInput();

	}


    private void ProcessUserInput()
    {
        var x = CurrentPlayer.Position.X;
        var y =CurrentPlayer.Position.Y;

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

        Game.Move(CurrentPlayer, Position.Create(x, y));
    }

    void OnGUI()
    {
        if (!Application.isEditor)  // or check the app debug flag
            return;

        var start = 10;
        GUI.Label(new Rect(0, start + 0, Screen.width, Screen.height), "x: " + CurrentPlayer.Position.X);
        GUI.Label(new Rect(0, start + 10, Screen.width, Screen.height), "y: " + CurrentPlayer.Position.Y);
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
            }
        }

    }
}
