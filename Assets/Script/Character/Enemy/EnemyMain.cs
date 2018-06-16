using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMYSTS
{
	SELECT,
	WAIT,
	WALK,
	TURN,
	RUNTOPLAYER,
	JUMP,
	JUMPTOPLAYER,
	ESCAPE,
	ATTACK,
	ATTACKONSIGHT,
	THROW,
	FREEZ,
}
enum TURN
{
	WAIT,
	WALK,
	ATTACK,
	DEAD
}

public class EnemyMain : MonoBehaviour {
	// === 外部パラメータ(Inspector)========================
	[System.NonSerialized] public int debug_SelectRandomAIState = -1;
	public bool cameraSwitch = true;

	// === 外部パラメータ===================================
	[System.NonSerialized] public ENEMYSTS state = ENEMYSTS.SELECT;
	[System.NonSerialized] public bool cameraEnabled = false;

	// === キャッシュ=======================================
	protected EnemyController enemyCtrl;
	protected GameObject player;
	protected PlayerController playerCtrl;

	// === 内部パラメータ===================================
	protected float ActionTimeLength = 0.0f;
	protected float ActionTimeStart = 0.0f;
	protected float distanceToPlayer = 0.0f;
	protected float distanceToPlayerPrev = 0.0f;
	protected bool TurnAtEdge = true;
	protected int deadEventCount = 0;
	protected float deadEventTimer = 0;
	protected float deadEventTime = 0;
	protected bool isWait = false;
	protected bool IsAnime = true;
	public virtual void Awake() {
		enemyCtrl = GetComponent<EnemyController> ();
		player = PlayerController.GetGameObject();
		playerCtrl = player.GetComponent<PlayerController> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cameraEnabled = false;
	}

	public virtual void FixedUpdate() {
		if(BeginEnemyCommonWork()) {
			FixedUpdateAI ();
			EndEnemyCommonWork ();
		}
	}

	public virtual void FixedUpdateAI() {
	}
	public virtual void deadEvent() {
	}
	//=== コード（基本AI動作処理） ============================
	public bool BeginEnemyCommonWork() {
		if (enemyCtrl.hp <= 0) {
			if (deadEventTimer > 0) {
				deadEventTime += Time.deltaTime;
				if (deadEventTimer <= deadEventTime) {
					deadEventTimer = 0;
				} else {
					return false;
				}
			}
	
			deadEvent ();
			deadEventCount++;
			return false;
		}

		//空中は強制実行
		if (enemyCtrl.grounded) {// && !enemyCtrl.isFly) {
			//カメラに入っているか
			if (cameraSwitch && !cameraEnabled && !enemyCtrl.cameraOK) {
				//カメラに映っていない
				enemyCtrl.Move (0.0f);
				enemyCtrl.cameraRendered = false;
				if(IsAnime) enemyCtrl.anime.enabled = false;

				GetComponent<Rigidbody2D> ().Sleep ();
				return false;
			}
		}

	//	GetComponent<Rigidbody2D> ().WakeUp();
		if(IsAnime)enemyCtrl.anime.enabled = true;
			enemyCtrl.cameraRendered = true;

		if (!CheckAction ()) {
			return false;
		}
		return true;
	}

	public void EndEnemyCommonWork() {
		float time = Time.fixedTime - ActionTimeStart;
		if (time > ActionTimeLength) {
			state = ENEMYSTS.SELECT;
		}
	}

	public bool CheckAction() {
		return true;
	}

	public int SelectRandomAIState() {
		#if UNITY_EDITOR
		if (debug_SelectRandomAIState >= 0) {
			return debug_SelectRandomAIState;
		}
		#endif
		return Random.Range (0, 100 + 1);
	}

	public void SetAIState(ENEMYSTS sts, float t) {
		state = sts;
		ActionTimeStart = Time.fixedTime;
		ActionTimeLength = t;
	}

	public virtual void SetCombatAIState(ENEMYSTS sts) {
		state = sts;
		ActionTimeStart = Time.fixedTime;
		enemyCtrl.Move (0.0f);
	}

	public float GetDistancePlayer() {
		distanceToPlayerPrev = distanceToPlayer;
		distanceToPlayer = Vector3.Distance (
			transform.position, playerCtrl.transform.position);
		return distanceToPlayer;
	}

	public bool IsChangeDistancePlayer(float f) {
		return(Mathf.Abs (distanceToPlayer - distanceToPlayerPrev) > 1);
	}

	public float GetDistancePlayerX() {
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.y = 0;
		posA.z = 0;
		posB.y = 0;
		posB.z = 0;
		return Vector3.Distance (posA, posB);
	}
	public float GetDistancePlayerY() {
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.x = 0;
		posA.z = 0;
		posB.x = 0;
		posB.z = 0;
		return Vector3.Distance (posA, posB);
	}

}
