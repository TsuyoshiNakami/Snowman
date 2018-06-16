using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour {
	EnemyMain enemyMain;
	// Use this for initialization
	void Awake() {
		enemyMain = GetComponentInParent<EnemyMain> ();
	}
	void OnWillRenderObject() {
		if (Camera.current.tag == "MainCamera") {
			enemyMain.cameraEnabled = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
