using UnityEngine;
using System.Collections;

/// Create a capsule.
/// - Add the FreeCamera script to the capsule.

/// Move camera by holding the right-mouse button
/// Zoom camera with the mouse scroll wheel

public class FreeCamera : MonoBehaviour {

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float sensitivityScroll = 5F;


    private float minimumX = -360F;
    private float maximumX = 360F;

    private float minimumY = -60F;
    private float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
        var zoom = Input.GetAxis("Mouse ScrollWheel") * sensitivityScroll;
        transform.position += transform.forward * sensitivityScroll * zoom;

        if (!Input.GetMouseButton(1))
            return;

		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}