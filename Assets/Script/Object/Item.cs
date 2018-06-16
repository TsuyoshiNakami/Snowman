using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	public PlayerController pc;

	// Use this for initialization
	protected virtual void Awake () {
		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	protected virtual void ActEnter() {
	}
	protected virtual void ActStay() {
	}
	protected virtual void ActExit() {
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("PlayerBody") && pc.isStarted) {

			ActEnter ();
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.CompareTag("PlayerBody") && pc.isStarted) {
	
			ActStay ();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.CompareTag("PlayerBody") && pc.isStarted) {

			ActExit ();
		}
	}
}
