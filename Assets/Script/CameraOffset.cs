using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour {
    private Camera cam;
    private float prev_y = 0;
    private float curr_y = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Camera cam = GetComponent<Camera>();
        //cam.transform.position = new Vector3(cam.transform.position.x, curr_y - (float)((int)(curr_y)), cam.transform.position.z);

        curr_y = cam.transform.position.y;
        if (Mathf.Abs(prev_y-curr_y) > 0.5)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, prev_y, cam.transform.position.z);
        }
        prev_y = curr_y;
	}
}
