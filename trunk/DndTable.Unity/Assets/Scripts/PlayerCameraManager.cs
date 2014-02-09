using System;
using DndTable.Core;
using UnityEngine;
using System.Collections;

public class PlayerCameraManager : MonoBehaviour {

    private TableManager TableManager { get { return Singleton<TableManager>.Instance; } }

    private int _currentCharacterId = -1;

	// Update is called once per frame
    void Update()
    {
        if (TableManager == null)
            return;

        if (TableManager.CurrentPlayer == null)
            return;

        if (TableManager.CurrentPlayer.Id != _currentCharacterId)
        {
            SetOrbitalCameraTarget(TableManager.GetCurrentCharacterTransform());
            _currentCharacterId = TableManager.CurrentPlayer.Id;
        }
    }


    private void SetOrbitalCameraTarget(Transform characterTransform)
    {
        var cameraScript = transform.GetComponent("MouseOrbitImproved") as MouseOrbitImproved;
        if (cameraScript != null)
        {
            cameraScript.target = characterTransform;
        }
    }
}
