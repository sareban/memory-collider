using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	private Rigidbody rb;
	private float defaultHeight;
	public float dir;

	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody> ();
		rb.AddForce(new Vector3 (dir*10, 0, dir*10));
		defaultHeight = transform.position.y;
	}

	void FixedUpdate() {
		//rb.AddForce (new Vector3 (dir, 0, 0));
	}
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
		transform.position = new Vector3 (transform.position.x,defaultHeight, transform.position.z);
	}
}
