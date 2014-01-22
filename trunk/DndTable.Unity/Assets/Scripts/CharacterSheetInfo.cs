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

        const float offset = 0.2f;

        var topOfAvatar = transform.position;
        topOfAvatar.y += transform.lossyScale.y + offset;

        var screenPos = Camera.main.WorldToScreenPoint(topOfAvatar);
        var labelRect = new Rect(screenPos.x, Screen.height - screenPos.y, Screen.width, Screen.height);

        //var label = string.Format("({0}, {1})", (int)transform.position.x, (int)transform.position.z);
        var label = Character.CharacterSheet.Name + ": " + Character.CharacterSheet.HitPoints + "hp";
        
        GUI.Label(labelRect, label);
    }
}
