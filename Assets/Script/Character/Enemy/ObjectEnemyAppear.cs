using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnemyAppear : MonoBehaviour {
	public GameObject appearObj;
	public float interval = 2;
	public float DistancefromPlayer = 5;
	float time = 0;
	GameObject pc;
	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (Vector3.Distance (transform.position, pc.transform.position) <= DistancefromPlayer) {
			if (time >= interval) {
				Vector2 pos = new Vector2 (transform.position.x+ 15, transform.position.y);
				Instantiate (appearObj,pos, transform.rotation);
				time = 0;
			}
		}
	}
}
