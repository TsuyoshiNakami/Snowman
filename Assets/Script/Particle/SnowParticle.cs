using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticle : MonoBehaviour {
	Rigidbody2D R2D;
	public Vector2 min = new Vector2(-100f,200f);
	public Vector2 max = new Vector2(100f,500f);
	// Use this for initialization
	void Start () {
		R2D = transform.GetComponent<Rigidbody2D> ();
		R2D.AddForce (new Vector2 (Random.Range (min.x, max.x), 300));//Random.Range (min.y,max.y)));
		transform.localScale = transform.localScale * Random.Range (1f, 2f);
		GetComponent<Rigidbody2D> ().mass = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
