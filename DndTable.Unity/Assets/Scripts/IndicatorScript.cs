using Assets.Scripts.Helpers;
using UnityEngine;
using System.Collections;

public class IndicatorScript : MonoBehaviour {

    private TableManager TableManager { get { return Singleton<TableManager>.Instance; } }

    private LerpInfo _positionLerp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var currentCharacterTransform = TableManager.GetCurrentCharacterTransform();
        if (currentCharacterTransform == null)
            return;

	    var position = new Vector3(currentCharacterTransform.position.x, transform.position.y, currentCharacterTransform.position.z);

        if (_positionLerp == null)
        {
            _positionLerp = new LerpInfo(position, 10);
        }

        transform.position = _positionLerp.UpdateLerp(position);
	}
}
