using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyCollider : MonoBehaviour {
	EnemyController enemyCtrl;
	Animator playerAnim;
	Animator animator;
    PlayerController pc;
	int attackHash = 0;
	public GameObject damageEffect;
	public int damage = 1;
	public bool hitPlayerBullet = true;
	SoundManager soundManager;
	public Vector2 playerNockBack;
	void Awake() {
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();
		enemyCtrl = GetComponentInParent<EnemyController> ();
		playerAnim = PlayerController.GetAnimator ();
		animator = GetComponentInParent<Animator> ();
		pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	void OnCollisionExit2D(Collision2D other) {
		if (other.transform.tag == "Player" && enemyCtrl.activeSts == true) {
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), other.collider, false);
		}
	}
	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "PlayerArmBullet" && hitPlayerBullet == true) {
			if (!enemyCtrl.isInvincible) {
				soundManager.PlaySEOneShot ("PlayerBulletHit");
			}
		}
	}
	void OnCollisionStay2D(Collision2D other) {
		if (other.transform.tag == "PlayerArmBullet" && hitPlayerBullet == true) {

			if (!enemyCtrl.isInvincible) {
				if (enemyCtrl.isBoss) {

					enemyCtrl.isInvincible = true;
					animator.SetTrigger ("SuperArmor");
				}
				Instantiate (damageEffect, transform.position, transform.rotation);
				enemyCtrl.Damage (1);

			}

			Vector2 vec = transform.position - other.transform.position;
			Pauser_old.targets.Remove(other.transform.GetComponent<Pauser_old>());
			Destroy(other.gameObject);
			vec.Normalize ();
			if (!enemyCtrl.isBoss) {
				enemyCtrl.DamageNockBack (vec.x * 10 * enemyCtrl.gravity, (vec.y + 0.5f) * 10 * enemyCtrl.gravity);
			}
			if (enemyCtrl.hp <= 0) {
				Destroy (this.gameObject);
			}
		}
		if (other.transform.tag == "Player" && enemyCtrl.activeSts == true && pc.hp > 0) {
			pc.Damage(damage);
			Vector2 vec = other.transform.position - transform.position;
			vec.Normalize ();

			//	vec = new Vector2 (vec.x * 5000, 1500);
			if (damage > 0) {
				//pc.DamageNockBack (vec.x * playerNockBack.x, (vec.y + 0.5f) * playerNockBack.y);
				pc.DamageNockBack (vec.x * 400, (vec.y + 0.5f) * 800);
				pc.SetInvincible (1.5f);
			} else {

				pc.AddForcePC (0, (vec.y + 0.5f) * 800);
			}
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), other.collider, true);
//			pc.AddForceAnimatorVx (vec.x * 700);
//			pc.AddForceAnimatorVy ((vec.y + 0.5f) * 1000);

		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
