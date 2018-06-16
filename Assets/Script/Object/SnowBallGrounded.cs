using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallGrounded : MonoBehaviour {
	// 外部
	public GameObject player;
	public GameObject prefabPC;
	[System.NonSerialized] public bool canResize = true;
	// 内部
	bool ignorePlayer = true;
	SnowBallGroundedPlayerCheck PC;
	GameObject playerCheck;


	// Use this for initialization
	void Awake () {
		playerCheck = Instantiate (prefabPC);
		player = GameObject.Find ("Player");
		PC = playerCheck.GetComponent<SnowBallGroundedPlayerCheck> ();
		Collider2D[] col = player.GetComponentsInChildren<Collider2D> ();
		foreach(Collider2D c in col) {
			Physics2D.IgnoreCollision (c, transform.GetComponent<Collider2D> ());
		}

	}
	
	// Update is called once per frame
	void Update () {


		playerCheck.transform.position = transform.position;
		playerCheck.transform.localScale = transform.localScale;

		if (ignorePlayer && PC.playerExit) {
			Collider2D[] col = player.GetComponentsInChildren<Collider2D> ();

			foreach(Collider2D c in col) {
				Physics2D.IgnoreCollision (c, transform.GetComponent<Collider2D> (),false);
			}
			ignorePlayer = false;
		}
	}


}
