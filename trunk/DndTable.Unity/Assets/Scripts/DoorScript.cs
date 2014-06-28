using UnityEngine;
using System.Collections;
using DndTable.Core;
using DndTable.Core.Entities;

public class DoorScript : MonoBehaviour {

    public IBoard GameBoard;
    public IEntity Door;

    private Transform LeftRight { get { return transform.FindChild("LeftRight").transform; } }
    private Transform LeftRightOpen { get { return transform.FindChild("LeftRight-open").transform; } }
    private Transform TopBottom { get { return transform.FindChild("TopBottom").transform; } }
    private Transform TopBottomOpen { get { return transform.FindChild("TopBottom-open").transform; } }


    // Update is called once per frame
    void Update()
    {
        // TODO: optimize

        var isLeftRight = IsWall(Position.Create(Door.Position.X - 1, Door.Position.Y)) && IsWall(Position.Create(Door.Position.X + 1, Door.Position.Y));
        var isTopBottom = IsWall(Position.Create(Door.Position.X, Door.Position.Y - 1)) && IsWall(Position.Create(Door.Position.X, Door.Position.Y + 1));

        if (isLeftRight)
        {
            ShowHideRecursive(LeftRight, Door.IsBlocking);
            ShowHideRecursive(LeftRightOpen, !Door.IsBlocking);
            ShowHideRecursive(TopBottom, false);
            ShowHideRecursive(TopBottomOpen, false);
        }
        if (isTopBottom)
        {
            ShowHideRecursive(LeftRight, false);
            ShowHideRecursive(LeftRightOpen, false);
            ShowHideRecursive(TopBottom, Door.IsBlocking);
            ShowHideRecursive(TopBottomOpen, !Door.IsBlocking);
        }
    }

    private bool IsWall(Position position)
    {
        var entity = GameBoard.GetEntity(position, EntityTypeEnum.Wall);
        if (entity == null)
            return false;
        return entity.EntityType == EntityTypeEnum.Wall;
    }

    private static void ShowHideRecursive(Transform currentTransform, bool show)
    {
        if (currentTransform.renderer != null)
            currentTransform.renderer.enabled = show;

        for (var i = 0; i < currentTransform.childCount; i++)
        {
            var child = currentTransform.GetChild(i);
            ShowHideRecursive(child, show);
        }
    }

}
