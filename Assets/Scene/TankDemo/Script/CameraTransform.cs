using UnityEngine;
using System.Collections;

public class CameraTransform : MonoBehaviour {
	// Use this for initialization

    private const int MOUSE_LEFT = 0;
    private const int MOUSE_RIGHT = 1;
    private const int MOUSE_MIDDLE = 2;

	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 cameraUp = transform.up;
        Vector3 cameraPosition = transform.position;
        Quaternion cameraRotation = transform.rotation;
        Vector3 cameraPosMove = Vector3.zero;
        float ifps = Time.deltaTime;
        float speedMove = 10;
        float speedRot = 3;
        if (Input.GetMouseButton(MOUSE_RIGHT))
        {
	        if(Input.GetKey("w"))
            {
                cameraPosMove += (transform.forward * ifps) * speedMove;
            }
            if (Input.GetKey("s"))
            {
                cameraPosMove += (-transform.forward * ifps) * speedMove;
            }
            if (Input.GetKey("a"))
            {
                cameraPosMove += (-transform.right * ifps) * speedMove;
            }
            if (Input.GetKey("d"))
            {
                cameraPosMove += (transform.right * ifps) * speedMove;
            }
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * speedRot, Space.World);
            transform.Rotate(transform.right, -Input.GetAxis("Mouse Y") * speedRot, Space.World);
            transform.position += cameraPosMove;
        }
	}
}
