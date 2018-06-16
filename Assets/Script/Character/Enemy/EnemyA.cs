using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyA : EnemyMain {
	public bool doesMove = true;
	// Use this for initialization
	void Start () {
		enemyCtrl.dir = 1;

	}

	public override void FixedUpdateAI() {
		if (!playerCtrl.isStarted) {
			enemyCtrl.anime.SetTrigger ("Idle");
			return;
		}
		if (enemyCtrl.GetGroundTransform () != null) {
			transform.rotation = enemyCtrl.GetGroundTransform ().rotation;
		}
		if (!enemyCtrl.activeSts || !doesMove) {
			enemyCtrl.Move (0f);
			GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

			return;
		}

		switch(state) {

		case ENEMYSTS.SELECT:

				SetAIState (ENEMYSTS.WALK, 10.0f);
	
			enemyCtrl.Move (0f);
			break;
		case ENEMYSTS.WAIT:

			enemyCtrl.anime.SetTrigger ("Idle");
		//	Debug.Log ("WAIT");
			enemyCtrl.Move (0.0f);

			break;

		case ENEMYSTS.WALK:
		//	Debug.Log ("WALK");
			enemyCtrl.anime.SetTrigger ("Walk");
			enemyCtrl.Move (enemyCtrl.dir);
			if (!enemyCtrl.CheckGround () || enemyCtrl.CheckWall()) {
				enemyCtrl.dir = -enemyCtrl.dir;
				enemyCtrl.anime.SetTrigger ("Idle");
				SetAIState (ENEMYSTS.WAIT, 1.0f);
			}
			break;
	}
	}
}
