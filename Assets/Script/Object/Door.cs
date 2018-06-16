using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorPlace {
	Stage1,
	MiniGame,
}
public class Door : MonoBehaviour {
	public  DoorPlace place;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Collider2D[] colList = Physics2D.OverlapPointAll (GetComponent<BoxCollider2D>().transform.position);
		foreach (Collider2D col in colList) {
			if (col.CompareTag ("PlayerBody")) {

				if (Input.GetButtonDown ("Vertical")) {
					PlayerController.enterDoor = true;
					switch(place) {
					case DoorPlace.Stage1:
						GameManager.WarpedPlayerPosition = new Vector2 (-4, 2);
						GameManager.LoadScene ("MiniGame");
						break;
					case DoorPlace.MiniGame:
						GameManager.WarpedPlayerPosition = new Vector2 (86, 8);
						GameManager.LoadScene ("Stage1");
						break;
					}
					break;
				}
			}
		}

	}

}
