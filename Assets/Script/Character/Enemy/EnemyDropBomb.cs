using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDropBomb : EnemyMain {
	TURN turn = TURN.WALK;
	public GameObject bomb;
	public float BombTime = 1f;
	public float TurnSpeed = 5.0f;
	float time = 0;
	float vx= 0;
	float acSpeed = 0.04f;
	bool accel = true;
	public bool oneWay = false;
	float speedRnd = 0;
	// Use this for initialization
	void Start () {
		speedRnd = Random.Range (1f, 4f);
		enemyCtrl.isFly = true;
		enemyCtrl.DontUseBaseVelocity = false;
		enemyCtrl.dir = -1;
		enemyCtrl.speed = 3;
		time = 0;
	}


	public override void FixedUpdateAI() {
		time += Time.fixedDeltaTime;
		if (time >= BombTime) {
			time = 0;
			DropBomb ();
		}
		if (!enemyCtrl.activeSts) {
			turn = TURN.DEAD;
			return;
		}
		if (oneWay) {
			switch(state) {
			case ENEMYSTS.SELECT:
				if (vx == 10) {
					Destroy (gameObject);
				} else {
					SetAIState (ENEMYSTS.WALK, 10);

				}
				break;
			case ENEMYSTS.WALK:

				enemyCtrl.speed = speedRnd;
				enemyCtrl.Move (-1);

				vx = 10;
				break;
			}
			return;
		}
		switch(state) {
		case ENEMYSTS.SELECT:

				SetAIState (ENEMYSTS.WALK, 10);

			break;
		case ENEMYSTS.WAIT:
			turn = TURN.WALK;

			enemyCtrl.Move (0.0f);

			break;

		case ENEMYSTS.WALK:

			enemyCtrl.speed = vx;
			enemyCtrl.Move (enemyCtrl.dir);
			if (enemyCtrl.speed >= TurnSpeed) {
				accel = false;
			}
			if (enemyCtrl.speed <= 0) {
				accel = true;
				enemyCtrl.dir = -enemyCtrl.dir;
			}
			if (accel) {
				vx += acSpeed;
			} else {
				vx -= acSpeed;
			}

			break;
		}
	}

	void DropBomb() {
		GameObject trans;
		trans = Instantiate (bomb, transform.Find ("DropPoint").position, transform.rotation);
		if (state == ENEMYSTS.WALK) {
			trans.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (enemyCtrl.speed * enemyCtrl.dir * 50, 0));
		} 
	}
}