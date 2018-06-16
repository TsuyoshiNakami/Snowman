using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : PointMove {
	ButtonEvent button;
	bool enterSnow = false;
	public Sprite snowWine;
	// Use this for initialization
	void Start () {
		button = transform.Find ("Button").GetComponent<ButtonEvent>();

	}
	
	// Update is called once per frame
	void Update () {
		Move (transform.position);
		if (button.Switched && !enterSnow) {
			enterSnow = true;
			GetComponent<SpriteRenderer> ().sprite = snowWine;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		target = null;

	}
}
