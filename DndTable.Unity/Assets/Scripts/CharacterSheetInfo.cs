using System;
using UnityEngine;
using System.Collections;
using DndTable.Core.Characters;

public class CharacterSheetInfo : MonoBehaviour {

    public ICharacter Character;


    void OnGUI()
    {
        if (Character == null)
            return;

        if (Camera.main == null)
            return;

        if (!Character.CharacterSheet.CanAct())
            GUI.color = Color.red;


        const float offset = 1.5f;

        var topOfAvatar = transform.position;
        topOfAvatar.y += transform.lossyScale.y + offset;

        var screenPos = Camera.main.WorldToScreenPoint(topOfAvatar);
        var labelRect = new Rect(screenPos.x, Screen.height - screenPos.y, 100, 25);

        //var label = string.Format("({0}, {1})", (int)transform.position.x, (int)transform.position.z);
        var label = Character.CharacterSheet.Name + ": " + Character.CharacterSheet.HitPoints + "hp";
        
        GUI.Box(labelRect, label);
    }
}
