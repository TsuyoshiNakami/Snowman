using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {

	//=======外部パラメータ（Inspector表示）==================
	public  Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
	public  Vector2 velocityMax = new Vector2(100.0f, 50.0f);
    [System.NonSerialized] public bool superArmor = false;
    [System.NonSerialized] public bool superArmor_jumpAttackDmg = true;

	//======= 外部パラメータ =================================

	[System.NonSerialized] public Collider2D[] colObj;
	[System.NonSerialized] public bool activeSts = true;
	[System.NonSerialized] public float speed = 6.0f;
	[System.NonSerialized] public float dir = 1.0f;
	[System.NonSerialized] public float hp = 10.0f;
	[System.NonSerialized] public float hpMax = 10.0f;
	[System.NonSerialized] public float basScaleX = 1.0f;
	[System.NonSerialized] public bool jumped = false;
	[System.NonSerialized] public bool grounded = false;
	[System.NonSerialized] public bool groundedPrev = false;
	[System.NonSerialized] public bool IgnoreMoveDir = false;
	[System.NonSerialized] public float gravityScale = 10.0f;
	protected Rigidbody2D rbody2D;
	[System.NonSerialized] public bool DontUseBaseVelocity = false;
	public bool IsInvincible = false;
	[SerializeField] protected Sprite SpriteforCheck;
	//======= キャッシュ =================================
	[System.NonSerialized] public Animator anime;

	//======= 内部パラメータ =================================
	protected float jumpStartTime = 0.0f;
	protected float speedVx = 0.0f;
	protected float speedVxAddPower = 0.0f;
	public  Vector2 moveVec = Vector2.zero;
	protected float centerY = 0.5f;

	protected Transform groundCheck_L;
	protected Transform groundCheck_C;
	protected Transform groundCheck_R;


	//		アニメーション用
	protected bool			addForceVxEnabled	= false;
	protected float			addForceVxStartTime = 0.0f;

	protected bool			addVelocityEnabled	= false;
	protected float			addVelocityVx 		= 0.0f;
	protected float			addVelocityVy 		= 0.0f;

	protected bool			setVelocityVxEnabled= false;
	protected bool			setVelocityVyEnabled= false;
	protected float			setVelocityVx 		= 0.0f;
	protected float			setVelocityVy 		= 0.0f;
	//==========アニメーションイベント用コード===================
	public void EnableSuperArmor() {
		superArmor = true;
	}
	public void DisableSuperArmor() {
		superArmor = false;
	}

	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}
	protected virtual void Awake() {
		anime = GetComponent<Animator> ();

		groundCheck_L = transform.Find ("GroundCheck_L");
		groundCheck_C = transform.Find ("GroundCheck_C");
		groundCheck_R = transform.Find ("GroundCheck_R");
		dir = (transform.localScale.x > 0.0f) ? 1 : -1;
	}
	public virtual void Move(float n) {
		if (n != 0.0f) {
			if (!IgnoreMoveDir) {
				dir = Mathf.Sign (n);
			}
			moveVec = speed * n * (Vector2)transform.right;

		} else {
			moveVec = Vector2.zero;

		}
	}

	public bool Lookup(GameObject go, float near) {
		if(Vector3.Distance(transform.position, go.transform.position) > near) {
			dir = (transform.position.x < go.transform.position.x) ? 1 : -1;
			return true;
		}
		return false;
	}

	protected virtual void FixedUpdate() {

		// 落下チェック
		if (transform.position.y <= -30.0f) {
			Dead (false);
		}
		// 地面チェック
		groundedPrev = grounded;
		grounded = false;
		Collider2D[][] groundCheckCollider = new Collider2D[3][];
		groundCheckCollider [0] = Physics2D.OverlapPointAll (groundCheck_L.position);
		groundCheckCollider [1] = Physics2D.OverlapPointAll (groundCheck_C.position);
		groundCheckCollider [2] = Physics2D.OverlapPointAll (groundCheck_R.position);

		foreach (Collider2D[] groundCheckList in groundCheckCollider) {
			foreach (Collider2D groundCheck in groundCheckList) {
				if (groundCheck != null) {
					if (!groundCheck.isTrigger) {

                        //if (groundCheck.CompareTag ("Road") || groundCheck.CompareTag ("OtherCharacter")) {
                        if(rbody2D.velocity.y <= 0){ 
							grounded = true;
							//grounded = true;
						}
					}
				}
			}
		}
		//キャラクター個別の処理
		FixedUpdateCharacter();

		// 移動計算
		if (addForceVxEnabled) {
			// 移動計算は物理演算にまかせる
			if (Time.fixedTime - addForceVxStartTime > 0.5f) {
				addForceVxEnabled = false;
			}
		} else {
			// 移動計算
			//Debug.Log (">>>> " + string.Format("speedVx {0} y {1} g{2}",speedVx,rigidbody2D.velocity.y,grounded));
			GetComponent<Rigidbody2D>().velocity = new Vector2 (speedVxAddPower + speedVx, GetComponent<Rigidbody2D>().velocity.y) + moveVec;

		}

		// 最終的なVelocity計算
		if (addVelocityEnabled) {
			addVelocityEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x + addVelocityVx, GetComponent<Rigidbody2D>().velocity.y + addVelocityVy);
		}

		// 強制的にVelocityの値をセット
		if (setVelocityVxEnabled) {
			setVelocityVxEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (setVelocityVx, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (setVelocityVyEnabled) {
			setVelocityVyEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,setVelocityVy);
		}
		//移動計算
		if (DontUseBaseVelocity) {

		} else {
			//rbody2D.velocity = new Vector2 (speedVx, rbody2D.velocity.y);

			//Velocityの値をチェック
			float vx = Mathf.Clamp (rbody2D.velocity.x, velocityMin.x, velocityMax.x);
			float vy = Mathf.Clamp (rbody2D.velocity.y, velocityMin.y, velocityMax.y);
			rbody2D.velocity = new Vector2 (vx, vy);
		}
	}

	protected virtual void FixedUpdateCharacter() {
	}
	public virtual void Dead(bool gameOver) {
		if (!activeSts) {
			return;
		}
		activeSts = false;
	}

	public virtual bool setHP(float _hp, float _hpMax) {
		hp = _hp;
		hpMax = _hpMax;
		if (hp > hpMax) {
			hp = hpMax;
		}
		return (hp <= 0);
	}


	// === コード（アニメーションイベント用コード） ===============
	public virtual void AddForceAnimatorVx(float vx) {
		//Debug.Log (string.Format("--- AddForceAnimatorVx {0} ----------------",vx));
		if (vx != 0.0f) {
			GetComponent<Rigidbody2D>().AddForce (new Vector2(vx,0.0f));
			addForceVxEnabled	= true;
			addForceVxStartTime = Time.fixedTime;
		}
	}

	public virtual void AddForceAnimatorVy(float vy) {
		//Debug.Log (string.Format("--- AddForceAnimatorVy {0} ----------------",vy));
		if (vy != 0.0f) {
			GetComponent<Rigidbody2D>().AddForce (new Vector2(0.0f,vy));
			jumped = true;
			jumpStartTime = Time.fixedTime;
		}
	}

	public virtual void AddForceAnimator(Vector2 vec) {
		//Debug.Log (string.Format("--- AddForceAnimatorVy {0} ----------------",vy));
		if (vec != Vector2.zero) {
			GetComponent<Rigidbody2D>().AddForce (new Vector2(vec.x * dir, vec.y));
			addForceVxEnabled	= true;
			if (vec.y != 0.0f) {

				jumped = true;
				jumpStartTime = Time.fixedTime;
			}
		}
	}

	public virtual void AddVelocityVx(float vx) {
		//Debug.Log (string.Format("--- AddVelocityVx {0} ----------------",vx));
		addVelocityEnabled = true;
		addVelocityVx = vx * dir;
	}
	public virtual void AddVelocityVy(float vy) {
		//Debug.Log (string.Format("--- AddVelocityVy {0} ----------------",vy));
		addVelocityEnabled = true;
		addVelocityVy = vy;
	}

	public virtual void SetVelocityVx(float vx) {
		//Debug.Log (string.Format("--- setelocityVx {0} ----------------",vx));
		setVelocityVxEnabled = true;
		setVelocityVx = vx * dir;
	}
	public virtual void SetVelocityVy(float vy) {
		//Debug.Log (string.Format("--- setVelocityVy {0} ----------------",vy));
		setVelocityVyEnabled = true;
		setVelocityVy = vy;
	}

	public virtual void SetLightGravity() {
		//Debug.Log ("--- SetLightGravity ----------------");
		GetComponent<Rigidbody2D>().velocity 	 = Vector2.zero;
		GetComponent<Rigidbody2D>().gravityScale = 0.1f;
	}
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class BaseCharacterController : MonoBehaviour {
//
//	//=======外部パラメータ（Inspector表示）==================
//	public  Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
//	public  Vector2 velocityMax = new Vector2(100.0f, 50.0f);
//	public bool superArmor = false;
//	public bool superArmor_jumpAttackDmg = true;
//
//	//======= 外部パラメータ =================================
//
//	[System.NonSerialized] public Collider2D[] colObj;
//	[System.NonSerialized] public bool activeSts = true;
//	[System.NonSerialized] public float speed = 6.0f;
//	[System.NonSerialized] public float dir = 1.0f;
//	[System.NonSerialized] public float hp = 10.0f;
//	[System.NonSerialized] public float hpMax = 10.0f;
//	[System.NonSerialized] public float basScaleX = 1.0f;
//	[System.NonSerialized] public bool jumped = false;
//	[System.NonSerialized] public bool grounded = false;
//	[System.NonSerialized] public bool groundedPrev = false;
//	[System.NonSerialized] public bool IgnoreMoveDir = false;
//	[System.NonSerialized] public float gravityScale = 10.0f;
//	protected Rigidbody2D rbody2D;
//	[System.NonSerialized] public bool DontUseBaseVelocity = false;
//	public bool IsInvincible = false;
//	[SerializeField] protected Sprite SpriteforCheck;
//	//======= キャッシュ =================================
//	[System.NonSerialized] public Animator anime;
//
//	//======= 内部パラメータ =================================
//	protected float jumpStartTime = 0.0f;
//	protected float speedVx = 0.0f;
//	protected float speedVxAddPower = 0.0f;
//
//	protected float centerY = 0.5f;
//
//	protected Transform groundCheck_L;
//	protected Transform groundCheck_C;
//	protected Transform groundCheck_R;
//
//
//	//		アニメーション用
//	protected bool			addForceVxEnabled	= false;
//	protected float			addForceVxStartTime = 0.0f;
//
//	protected bool			addVelocityEnabled	= false;
//	protected float			addVelocityVx 		= 0.0f;
//	protected float			addVelocityVy 		= 0.0f;
//
//	protected bool			setVelocityVxEnabled= false;
//	protected bool			setVelocityVyEnabled= false;
//	protected float			setVelocityVx 		= 0.0f;
//	protected float			setVelocityVy 		= 0.0f;
//	//==========アニメーションイベント用コード===================
//	public void EnableSuperArmor() {
//		superArmor = true;
//	}
//	public void DisableSuperArmor() {
//		superArmor = false;
//	}
//
//	void Start () {
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//	protected virtual void Awake() {
//		anime = GetComponent<Animator> ();
//		groundCheck_L = transform.Find ("GroundCheck_L");
//		groundCheck_C = transform.Find ("GroundCheck_C");
//		groundCheck_R = transform.Find ("GroundCheck_R");
//		dir = (transform.localScale.x > 0.0f) ? 1 : -1;
//	}
//	public virtual void Move(float n) {
//		if (n != 0.0f) {
//			if (!IgnoreMoveDir) {
//				dir = Mathf.Sign (n);
//			}
//			speedVx = speed * n;
//			//anime.SetTrigger ("Run");
//		} else {
//			speedVx = 0;
//			//anime.SetTrigger ("Idle");
//		}
//	}
//
//	public bool Lookup(GameObject go, float near) {
//		if(Vector3.Distance(transform.position, go.transform.position) > near) {
//			dir = (transform.position.x < go.transform.position.x) ? 1 : -1;
//			return true;
//		}
//		return false;
//	}
//
//	protected virtual void FixedUpdate() {
//		
//		// 落下チェック
//		if (transform.position.y <= -30.0f) {
//			Dead (false);
//		}
//		// 地面チェック
//		groundedPrev = grounded;
//		grounded = false;
//		Collider2D[][] groundCheckCollider = new Collider2D[3][];
//		groundCheckCollider [0] = Physics2D.OverlapPointAll (groundCheck_L.position);
//		groundCheckCollider [1] = Physics2D.OverlapPointAll (groundCheck_C.position);
//		groundCheckCollider [2] = Physics2D.OverlapPointAll (groundCheck_R.position);
//
//		foreach (Collider2D[] groundCheckList in groundCheckCollider) {
//			foreach (Collider2D groundCheck in groundCheckList) {
//				if (groundCheck != null) {
//					if (!groundCheck.isTrigger) {
//
//						if (groundCheck.CompareTag ("Road")) {
//							grounded = true;
//							//grounded = true;
//						}
//					}
//				}
//			}
//		}
//		//キャラクター個別の処理
//		FixedUpdateCharacter();
//
//		// 移動計算
//		if (addForceVxEnabled) {
//			// 移動計算は物理演算にまかせる
//			if (Time.fixedTime - addForceVxStartTime > 0.5f) {
//				addForceVxEnabled = false;
//			}
//		} else {
//			// 移動計算
//			//Debug.Log (">>>> " + string.Format("speedVx {0} y {1} g{2}",speedVx,rigidbody2D.velocity.y,grounded));
//			GetComponent<Rigidbody2D>().velocity = new Vector2 (speedVx + speedVxAddPower, GetComponent<Rigidbody2D>().velocity.y);
//		}
//
//		// 最終的なVelocity計算
//		if (addVelocityEnabled) {
//			addVelocityEnabled = false;
//			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x + addVelocityVx, GetComponent<Rigidbody2D>().velocity.y + addVelocityVy);
//		}
//
//		// 強制的にVelocityの値をセット
//		if (setVelocityVxEnabled) {
//			setVelocityVxEnabled = false;
//			GetComponent<Rigidbody2D>().velocity = new Vector2 (setVelocityVx, GetComponent<Rigidbody2D>().velocity.y);
//		}
//		if (setVelocityVyEnabled) {
//			setVelocityVyEnabled = false;
//			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,setVelocityVy);
//		}
//		//移動計算
//		if (DontUseBaseVelocity) {
//			
//		} else {
//			//rbody2D.velocity = new Vector2 (speedVx, rbody2D.velocity.y);
//
//			//Velocityの値をチェック
//			float vx = Mathf.Clamp (rbody2D.velocity.x, velocityMin.x, velocityMax.x);
//			float vy = Mathf.Clamp (rbody2D.velocity.y, velocityMin.y, velocityMax.y);
//			rbody2D.velocity = new Vector2 (vx, vy);
//		}
//	}
//
//	protected virtual void FixedUpdateCharacter() {
//	}
//	public virtual void Dead(bool gameOver) {
//		if (!activeSts) {
//			return;
//		}
//		activeSts = false;
//	}
//
//	public virtual bool setHP(float _hp, float _hpMax) {
//		hp = _hp;
//		hpMax = _hpMax;
//		return (hp <= 0);
//	}
//
//
//	// === コード（アニメーションイベント用コード） ===============
//	public virtual void AddForceAnimatorVx(float vx) {
//		//Debug.Log (string.Format("--- AddForceAnimatorVx {0} ----------------",vx));
//		if (vx != 0.0f) {
//			GetComponent<Rigidbody2D>().AddForce (new Vector2(vx,0.0f));
//			addForceVxEnabled	= true;
//			addForceVxStartTime = Time.fixedTime;
//		}
//	}
//
//	public virtual void AddForceAnimatorVy(float vy) {
//		//Debug.Log (string.Format("--- AddForceAnimatorVy {0} ----------------",vy));
//		if (vy != 0.0f) {
//			GetComponent<Rigidbody2D>().AddForce (new Vector2(0.0f,vy));
//			jumped = true;
//			jumpStartTime = Time.fixedTime;
//		}
//	}
//
//	public virtual void AddForceAnimator(Vector2 vec) {
//		//Debug.Log (string.Format("--- AddForceAnimatorVy {0} ----------------",vy));
//		if (vec != Vector2.zero) {
//			GetComponent<Rigidbody2D>().AddForce (new Vector2(vec.x * dir, vec.y));
//			addForceVxEnabled	= true;
//			if (vec.y != 0.0f) {
//
//				jumped = true;
//				jumpStartTime = Time.fixedTime;
//			}
//		}
//	}
//
//	public virtual void AddVelocityVx(float vx) {
//		//Debug.Log (string.Format("--- AddVelocityVx {0} ----------------",vx));
//		addVelocityEnabled = true;
//		addVelocityVx = vx * dir;
//	}
//	public virtual void AddVelocityVy(float vy) {
//		//Debug.Log (string.Format("--- AddVelocityVy {0} ----------------",vy));
//		addVelocityEnabled = true;
//		addVelocityVy = vy;
//	}
//
//	public virtual void SetVelocityVx(float vx) {
//		//Debug.Log (string.Format("--- setelocityVx {0} ----------------",vx));
//		setVelocityVxEnabled = true;
//		setVelocityVx = vx * dir;
//	}
//	public virtual void SetVelocityVy(float vy) {
//		//Debug.Log (string.Format("--- setVelocityVy {0} ----------------",vy));
//		setVelocityVyEnabled = true;
//		setVelocityVy = vy;
//	}
//
//	public virtual void SetLightGravity() {
//		//Debug.Log ("--- SetLightGravity ----------------");
//		GetComponent<Rigidbody2D>().velocity 	 = Vector2.zero;
//		GetComponent<Rigidbody2D>().gravityScale = 0.1f;
//	}
//}
