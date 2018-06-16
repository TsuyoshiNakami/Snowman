using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHop : EnemyMain {
	Rigidbody2D rigid;
	SpriteRenderer renderer;
	[SerializeField] Sprite[] sprite;
	public float interval = 0.5f;
	float JumpStartTime = 0;
	bool charging = false;
	bool jumped = false;
	// Use this for initialization
	void Start () {
		enemyCtrl.setHP (3, 3);
		renderer = GetComponentInChildren<SpriteRenderer> ();
		rigid = GetComponent<Rigidbody2D> ();
		enemyCtrl.dir = 1;
		enemyCtrl.DontUseBaseVelocity = true;
		enemyCtrl.speed = 0.04f;
	}



	public override void FixedUpdateAI() {
		if (!enemyCtrl.activeSts) {

			return;
		}

		switch(state) {
		case ENEMYSTS.SELECT:
			if (jumped) {
				if (Mathf.Abs (rigid.velocity.y) < 4) {
					renderer.sprite = sprite [0];
					renderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				} else {
					if (rigid.velocity.y >= 0) {
						renderer.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -15));
					} else {
						renderer.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 15));
					}
					renderer.sprite = sprite [1];
				}
			}
			if (enemyCtrl.grounded && !charging) {
				renderer.sprite = sprite [2];
				JumpStartTime = Time.fixedTime;
				charging = true;

			}
			if (Time.fixedTime - JumpStartTime >= interval && !jumped) {

				jumped = true;
				Jump ();
			}
			if (enemyCtrl.grounded && !enemyCtrl.groundedPrev) {
				charging = false;
				jumped = false;
				renderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
			if(!enemyCtrl.grounded) {
				transform.Translate(new Vector3(enemyCtrl.dir * enemyCtrl.speed, 0, 0));
			}
			if (enemyCtrl.CheckWall()) {
				enemyCtrl.dir = -enemyCtrl.dir;
			}
			break;

		}
	}
	void Jump() {
		renderer.sprite = sprite[1];
		rigid.AddForce (enemyCtrl.jumpPower);
	}
}

