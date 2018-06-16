using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour {
	Collider2D[] col;
	float time = 0;
	public float jumpPower = 200f;
	// Use this for initialization
	void Start () {
		col =GameObject.Find("Player").GetComponentsInChildren<Collider2D> ();
		foreach(Collider2D c in col) {
			Physics2D.IgnoreCollision (c, transform.GetComponent<Collider2D> ());
		}	
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 1.5f) {
			if (Random.Range (0, 500) < 10) {
				Jump ();
				time = 0;
			}
		}
	}

	void Jump() {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpPower));
	}
}
