using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCollider : MonoBehaviour {
    PlayerController playerCtrl;
	[SerializeField] bool bottomCollider = false;

	// Use this for initialization
	void Start () {
		playerCtrl = gameObject.GetComponentInParent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionStay2D(Collision2D col) {
		//Debug.Log (col.transform.name);
		if (!bottomCollider)
			return;
		if(!playerCtrl.jumped &&
			(col.gameObject.tag == "Road" ||
				col.gameObject.tag == "MoveObject" ||
				col.gameObject.tag == "Enemy" )) {
			playerCtrl.groundY = transform.parent.transform.position.y;
		}

	}

	void OnTriggerStay2D(Collider2D col) {
		if (!playerCtrl.isStarted)
			return;
		if (col.gameObject.tag == "MsgCollider") {
			col.enabled = false;
		}
	}
}