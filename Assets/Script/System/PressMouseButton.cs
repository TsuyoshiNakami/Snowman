using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressMouseButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameManager.LoadScene ("Stage1");
		}
	}
}
