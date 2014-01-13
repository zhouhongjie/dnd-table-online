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


    private IGame _game;


	// Use this for initialization
	void Start ()
	{
        _game = Factory.CreateGame(MaxX, MaxY);

        // Temp
	    _game.AddCharacter(Factory.CreateCharacter(), 10, 10);

	    CreateBoard();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateBoard()
    {
        for (int i=0; i < _game.GameBoard.MaxX; i++)
        {
            for (int j=0; j < _game.GameBoard.MaxY; j++)
            {
                CreateEntity(TileTemplate, i, j);

                var entity = _game.GameBoard.GetEntity(i, j);
                if (entity != null)
                {
                    if (entity.EntityType == EntityTypeEnum.Character)
                    {
                        CreateEntity(PlayerTemplate, i, j);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }
    }

    private void CreateEntity(Transform template, int i, int j)
    {
        Vector3 position = new Vector3(i, 0, j);
        Transform newObj = (Transform)Instantiate(template, position, Quaternion.identity);
    }
}
