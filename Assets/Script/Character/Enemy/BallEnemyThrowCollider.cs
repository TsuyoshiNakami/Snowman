using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEnemyThrowCollider : MonoBehaviour {

	Animator playerAnim;
	PlayerController pc;
	void Awake() {
		pc = GameObject.Find ("Player").GetComponent<PlayerController>();
		playerAnim = PlayerController.GetAnimator ();
	}

	void OnCollisionEnter2D(Collision2D other) {

		if (other.transform.tag == "PlayerArmBullet") {

			//Destroy (gameObject);
		}
		if (other.transform.tag == "Player") {
			pc.Damage(1);
			Vector2 vec = other.transform.position - transform.position;
			vec.Normalize ();

			//	vec = new Vector2 (vec.x * 5000, 1500);

			pc.DamageNockBack(vec.x * 700, (vec.y + 0.5f) * 800);
			//			pc.AddForceAnimatorVx (vec.x * 700);
			//			pc.AddForceAnimatorVy ((vec.y + 0.5f) * 1000);
			pc.SetInvincible (1.5f);
			Destroy (gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {

		if (other.transform.tag == "PlayerArmBullet") {

			//Destroy (gameObject);
		}

		if (other.tag == "PlayerBody") {

			pc.Damage(1);
			Vector2 vec = other.transform.position - transform.position;
			vec.Normalize ();

			//	vec = new Vector2 (vec.x * 5000, 1500);

			pc.DamageNockBack(vec.x * 700, (vec.y + 0.5f) * 800);
			//			pc.AddForceAnimatorVx (vec.x * 700);
			//			pc.AddForceAnimatorVy ((vec.y + 0.5f) * 1000);
			pc.SetInvincible (1.5f);
			Destroy (gameObject);
		}
	}
}
