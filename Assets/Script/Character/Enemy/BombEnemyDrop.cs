using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyDrop: MonoBehaviour {

	PlayerController pc;
	void Awake() {

	}

	void OnCollisionEnter2D(Collision2D other) {
		pc = GameObject.Find ("Player").GetComponent<PlayerController>();
		if (other.transform.tag == "PlayerArmBullet") {
			Destroy(other.gameObject);
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
		}
		if (other.transform.tag == "Road") {
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -40) {
			Destroy (gameObject);
		}
	}
}
