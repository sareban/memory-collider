using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PanoramaCylinder : MonoBehaviour {
	public float shiftU = 0.0f;
	public float scaleCylinderV = 0.1f;
	private MeshRenderer meshRenderer = null;
	public RenderTexture tex;

	// Use this for initialization
	void Start () {
		CylinderGenerator.Create(gameObject, true, 64);

	}
	
	// Update is called once per frame
	void Update () {
	}

}

