using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStamp : EnemyMain {
	TURN turn = TURN.WALK;
	public float jumpTime = 2f;
	public float jumpInterval = 0.5f;
	bool first = true;
	// Use this for initialization
	void Start () {
		enemyCtrl.DontUseBaseVelocity = true;
		enemyCtrl.dir = 1;
		enemyCtrl.setHP (10, 10);
		enemyCtrl.gravity = 1;//		SetAIState (ENEMYSTS.WAIT, 2.0f);
		SetAIState (ENEMYSTS.WAIT, 5.0f);
	}
	

	public override void FixedUpdateAI() {

		//	transform.localScale = new Vector3 (3, 3, 1);
		if (!enemyCtrl.activeSts) {
			turn = TURN.DEAD;
			return;
		}
		switch(state) {
		case ENEMYSTS.SELECT:
			//	Debug.Log ("敵セレクト");
			Debug.Log (enemyCtrl.cameraRendered);
			if (first) {

				SetAIState (ENEMYSTS.WAIT, 3.0f);
				first = false;
			} else {
				SetAIState (ENEMYSTS.JUMPTOPLAYER, 1.0f);
			}
			//enemyCtrl.Move (0f);
			break;
		case ENEMYSTS.WAIT:
			turn = TURN.WALK;

			//enemyCtrl.Move (0.0f);

			break;

		case ENEMYSTS.WALK:

			enemyCtrl.Move (enemyCtrl.dir);
			if (!enemyCtrl.CheckGround ()) {
				enemyCtrl.dir = -enemyCtrl.dir;
				turn = TURN.WAIT;
				SetAIState (ENEMYSTS.WAIT, jumpInterval);
			}
			break;
		case ENEMYSTS.JUMPTOPLAYER:
			if (enemyCtrl.grounded) {
				JumpToPlayer ();
				SetAIState (ENEMYSTS.WAIT, jumpTime);
			}
			//enemyCtrl.anime.SetTrigger ("Jump");
			break;
		}
	}

	public void JumpToPlayer() {

		Rigidbody2D rigid;
		rigid = GetComponent<Rigidbody2D> ();

		Vector2 dv = (Vector2)player.transform.position - (Vector2)transform.position;

			Vector2 vec = Vector2.zero;
		vec.x = dv.x / jumpTime;
		vec.y = (dv.y + 0.5f* enemyCtrl.gravityScale * 9.8f * jumpTime* jumpTime) / jumpTime;
		rigid.velocity = vec;
		//enemyCtrl.Jump (vec);

			//rigid.velocity = vec;
	}
}