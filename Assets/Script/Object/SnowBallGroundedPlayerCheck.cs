using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallGroundedPlayerCheck : MonoBehaviour {
	public Collider2D[] col;
	public bool playerExit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playerExit = true;
	}
	void  OnTriggerEnter2D(Collider2D collider) {
		if (collider.transform.tag == "PlayerBody") {
		}
	}
	void  OnTriggerStay2D(Collider2D collider) {
		if (collider.transform.tag == "PlayerBody") {
			playerExit = false;
		}
	}
	void OnTriggerExit2D(Collider2D collider) {

		if (collider.transform.tag == "PlayerBody") {
			col = collider.transform.GetComponentsInChildren<Collider2D> ();
			playerExit = true;

		}
	}
}
