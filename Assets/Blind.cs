using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blind : MonoBehaviour {
	Image image;
	bool isRemoving = false;
	PlayerController pc;
	float removeSpeed = 0.005f;
	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
		image = GetComponent<Image> ();
		if (!GameManager.isFirstPlay) {
			RemoveBlind ();
			removeSpeed = 0.02f;
		//	Destroy (gameObject);
		}
	}

	void Update () {
		if (isRemoving) {
			Color color = image.color;
			color.a -= removeSpeed;
			if (color.a < 0) {

				enabled = false;
			}
			image.color = color;
		}
	}
	// Update is called once per frame
	public void RemoveBlind () {
	//	Debug.Log ("RemoveBlind");
		isRemoving = true;

	}
}
