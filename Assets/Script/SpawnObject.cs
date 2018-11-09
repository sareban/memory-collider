using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
	public Vector3 center;
	public Vector3 size;

	public GameObject customcube;


	// Use this for initialization
	void Start () {
		for (int i = -50; i < 50; i++) {
			Instantiate(customcube, new Vector3(i*0.5f, 0.05f, 0.0f), Quaternion.identity); // Quaternion.identity - rotation nulle
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
				
}
