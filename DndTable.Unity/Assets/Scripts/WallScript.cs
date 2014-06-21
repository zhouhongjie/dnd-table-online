using UnityEngine;
using System.Collections;
using DndTable.Core;
using DndTable.Core.Entities;

public class WallScript : MonoBehaviour
{
    public IBoard GameBoard;
    public IEntity Wall;

    private Transform Pillar { get { return transform.FindChild("Pillar").transform; } }
    private Transform Left { get { return transform.FindChild("Left").transform; } }
    private Transform Right { get { return transform.FindChild("Right").transform; } }
    private Transform Top { get { return transform.FindChild("Top").transform; } }
    private Transform Bottom { get { return transform.FindChild("Bottom").transform; } }

	
	// Update is called once per frame
	void Update ()
	{
        Pillar.renderer.enabled = true;

        Left.renderer.enabled = IsWall(Position.Create(Wall.Position.X - 1, Wall.Position.Y));
        Right.renderer.enabled = IsWall(Position.Create(Wall.Position.X + 1, Wall.Position.Y));
        Top.renderer.enabled = IsWall(Position.Create(Wall.Position.X, Wall.Position.Y + 1));
        Bottom.renderer.enabled = IsWall(Position.Create(Wall.Position.X, Wall.Position.Y - 1));
	}

    private bool IsWall(Position position)
    {
        var entity = GameBoard.GetEntity(position, EntityTypeEnum.Wall);
        if (entity == null)
            return false;
        return entity.EntityType == EntityTypeEnum.Wall;
    }
}
