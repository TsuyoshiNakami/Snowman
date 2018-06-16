using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinText : MonoBehaviour {
	Text text;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}
		
	// Update is called once per frame
	void Update () {
		if (player.isStarted) {
			text.enabled = true;
			text.text = "雪のかけら:" + PlayerController.coin;
		} else {
			text.enabled = false;
		}
	}
}
