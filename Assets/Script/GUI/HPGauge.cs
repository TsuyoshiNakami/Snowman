using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour {
    PlayerController pc;

	Image image;
	public int gaugeNumber = 1;
	// Use this for initialization
	void Start () {

		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (pc.hp >= gaugeNumber && pc.isStarted) {
			image.enabled = true;
		} else {
			image.enabled = false;
		}
	}
}
