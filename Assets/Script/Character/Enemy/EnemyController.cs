using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterController {
	public bool isBoss = false;
	[System.NonSerialized] public bool isInvincible = false;
	public float initHpMax = 1.0f;
	public float initSpeed = 6.0f;
	public bool jumpActionEnabled = true;
	public Vector2 jumpPower = new Vector2 (0.0f, 1500.0f);
	public int addScore = 500;
	//public bool isInvincible = false;
	bool breakEnabled = true;
	float groundFriction = 0.0f;
	[System.NonSerialized] public bool attackEnabled = false;
	[System.NonSerialized] public int attackDamage = 1;
	[System.NonSerialized] public Vector2 attackNockBackVector = Vector3.zero;
	[System.NonSerialized] public bool isFly = false;
	[System.NonSerialized] public bool cameraRendered = false;
	public float gravity = 10;
	Transform[] wallCheck;
	public bool deadEvent = false;
	[System.NonSerialized]public bool deadEventStarted = false;
    //===== キャッシュ ==============================
    PlayerController playerCtrl;
	Animator playerAnim;
	public bool cameraOK = false;
	protected override void Awake() {
		base.Awake ();
		rbody2D = GetComponent<Rigidbody2D> ();

		playerCtrl = PlayerController.GetController ();
		playerAnim = playerCtrl.GetComponent<Animator> ();
		hpMax = initHpMax;
		hp = hpMax;
		speed = initSpeed;
		wallCheck = new Transform[2];
		wallCheck [0] = transform.Find ("wallCheck");
		wallCheck [1] = transform.Find ("wallCheck2");

	}

	protected override void FixedUpdateCharacter() {
		if (isFly) {
			gravityScale = 0;
		} else {
			gravityScale = gravity;
		}
		if (!cameraRendered) {
			return;
		}
		// ジャンプチェック
		if (jumped) {
			if ((grounded && !groundedPrev) ||
			    (grounded && Time.fixedTime > jumpStartTime + 1.0f)) {
				jumped = false;
			}
			if (Time.fixedTime > jumpStartTime + 1.0f) {
				if (rbody2D.gravityScale < gravityScale) {
					rbody2D.gravityScale = gravityScale;
				}
			}
		} else {
			rbody2D.gravityScale = gravityScale;
		}

		//　ジャンプ中の横移動減速
		if (jumped && !grounded) {
			if (breakEnabled) {
				breakEnabled = false;
				speedVx *= 0.9f;
			}
		}
		/*
		//減速処理
		if (breakEnabled) {
			speedVx *= groundFriction;
		}
		*/
		// キャラの方向
		transform.localScale = new Vector3 (
			basScaleX * dir, transform.localScale.y, transform.localScale.z);

	}
	// Use this for initialization
	void Start () {
		
	}

	public bool Jump() {
		if (jumpActionEnabled && grounded && !jumped) {
			rbody2D.AddForce (jumpPower);
			jumped = true;
			jumpStartTime = Time.fixedTime;
		}
		return jumped;
	}
	public bool Jump(Vector2 vec) {
		if (jumpActionEnabled && grounded && !jumped) {
			rbody2D.AddForce(vec);
			jumped = true;
			jumpStartTime = Time.fixedTime;
		}
		return jumped;
	}
	/*
	public override void Move(float n) {

		breakEnabled = false;
		if (!activeSts) {
			return;
		}
		float dirOld = dir;
		float moveSpeed = Mathf.Clamp (Mathf.Abs (n), -1.0f, +1.0f);
		anime.SetFloat ("MovSpeed", moveSpeed);

		//移動チェック
		if (n != 0.0f) {

			dir = Mathf.Sign (n);


			moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
			speedVx = initSpeed * moveSpeed * dir;
		} else {
			breakEnabled = true;
		}

		if (dirOld != dir) {
			breakEnabled = true;
		}
	}
	*/
	public void Damage(int damage) {
		if(hp <= 0 || IsInvincible) {
			return;
		}
		if(setHP(hp - damage, hpMax)) {
			Dead (false);
		}
	}
		public override void Dead(bool gameOver) {

		if(!deadEvent) {
			base.Dead(gameOver);
			rbody2D.AddForce (new Vector2 (0, 1000));
			Pauser_old.targets.Remove(GetComponent<Pauser_old>());
			Destroy(gameObject, 0.5f);
		}

		deadEventStarted = true;
		}

	public bool CheckGround() {

		bool groundForward = false;
		Collider2D[] groundCheckCollider;
		groundCheckCollider = Physics2D.OverlapPointAll (groundCheck_R.position);


			Collider2D[] groundCheckList = groundCheckCollider;

			foreach (Collider2D groundCheck in groundCheckList) {
				if (groundCheck != null) {

						if (groundCheck.CompareTag ("Road")) {

						groundForward = true;

							}
						
				
				}
			}

		if (!groundForward) {

			return false;
		}

		return true;
		}
	public bool CheckGround(float dir) {

		bool groundForward = false;
		Collider2D[] groundCheckCollider;
		groundCheckCollider = Physics2D.OverlapPointAll (groundCheck_R.position);
		if (dir < 0) {
			groundCheckCollider = Physics2D.OverlapPointAll (groundCheck_L.position);
		}

		Collider2D[] groundCheckList = groundCheckCollider;

		foreach (Collider2D groundCheck in groundCheckList) {
			if (groundCheck != null) {

				if (groundCheck.CompareTag ("Road")) {

					groundForward = true;

				}


			}
		}

		if (!groundForward) {

			return false;
		}

		return true;
	}
	public Transform GetGroundTransform() {
		Collider2D[] groundCheckCollider;
		groundCheckCollider = Physics2D.OverlapPointAll (groundCheck_R.position);


		foreach (Collider2D groundCheck in groundCheckCollider) {
			if (groundCheck != null) {

				if (groundCheck.CompareTag ("Road")) {

					return groundCheck.transform;
				}


			}
		}
		return null;
	}
	public bool CheckWall() {
		if (wallCheck [0] == null) {
			return false;
		}
		Collider2D[][] wallCheckCollider = new Collider2D[2][];
		wallCheck [0] = transform.Find ("wallCheck");
		wallCheck [1] = transform.Find ("wallCheck2");
		wallCheckCollider[0] = Physics2D.OverlapPointAll (wallCheck[0].position);
		wallCheckCollider[1] = Physics2D.OverlapPointAll (wallCheck[1].position);

		foreach (Collider2D[] wallCheckList in wallCheckCollider) {
			foreach (Collider2D wallCheck2 in wallCheckList) {
				if (wallCheck2 != null) {

					if (wallCheck2.CompareTag ("Road")) {

						return true;

					}


				}
			}
		}
		//Collider2D[] groundCheckList;// = groundCheckCollider;

		return false;
	}

	public bool CheckWall(float dir) {
		if (wallCheck [0] == null) {
			return false;
		}
		Collider2D[][] wallCheckCollider = new Collider2D[2][];
		wallCheck [0] = transform.Find ("wallCheck");
		wallCheck [1] = transform.Find ("wallCheck2");
		if (dir < 0) {
			wallCheck [0] = transform.Find ("wallCheckBehind");
			wallCheck [1] = transform.Find ("wallCheckBehind2");
		}
		wallCheckCollider[0] = Physics2D.OverlapPointAll (wallCheck[0].position);
		wallCheckCollider[1] = Physics2D.OverlapPointAll (wallCheck[1].position);

		foreach (Collider2D[] wallCheckList in wallCheckCollider) {
			foreach (Collider2D wallCheck2 in wallCheckList) {
				if (wallCheck2 != null) {

					if (wallCheck2.CompareTag ("Road")) {

						return true;

					}


				}
			}
		}
		//Collider2D[] groundCheckList;// = groundCheckCollider;

		return false;
	}
	public void DamageNockBack(float x, float y) {
		if (!IsInvincible) {
	//		Debug.Log ("Move");
			AddForceAnimatorVx (x);

				AddForceAnimatorVy (y);

		} 


	}

	public void EndInvincible() {

	//	isInvincible = false;
	}
	// Update is called once per frame
	void Update () {
		
	}

}
