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
		//Texture2D texture = toTexture2D (tex);
		//gameObject.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", texture);
	}

//	Texture2D toTexture2D(RenderTexture rTex)
//	{
//		Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
//		RenderTexture.active = rTex;
//		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
//		tex.Apply();
//		return tex;
	//}
}

//      meshRenderer = gameObject.AddComponent<MeshRenderer>();
//		meshRenderer.material.shader = cylinderShader;
//		meshRenderer.material.EnableKeyword("FROM_SPHERICAL");
//		meshRenderer.material.SetFloat("_Fade", 1f);
//		meshRenderer.material.SetFloat("_ShiftU", shiftU);
//		meshRenderer.material.SetFloat("_ScaleV", scaleCylinderV);