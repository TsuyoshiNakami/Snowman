using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPText : MonoBehaviour {
	Text text;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "HP:" + player.hp;
	}
}
