using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowPowerText : MonoBehaviour {
    Text text;
    PlayerController playerController;
    // Use this for initialization
    void Start () {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("{0:f2}", playerController.throwPower) + "/" + playerController.maxThrowPower;
	}
}
