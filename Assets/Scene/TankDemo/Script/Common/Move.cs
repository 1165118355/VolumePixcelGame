using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public bool isControle = false;
    public int speedMove = 5;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isControle)
        {
            return;
        }

        float ifps = Time.deltaTime;
        Vector3 cameraPosMove = new Vector3();
        if (Input.GetKey("w"))
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
	}
}
