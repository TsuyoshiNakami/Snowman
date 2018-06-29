using UnityEngine;
using System.Collections;

public enum CAMERATARGET { // --- カメラのターゲットタイプ ---
	PLAYER,			// プレイヤー座標
	PLAYER_MARGIN,	// プレイヤー座標（前方視界を確保するマージン付き）
	PLAYER_GROUND,	// 過去にプレイヤーが接地した地面の座標（前方視界を確保するマージン付き）
}

public enum CAMERAHOMING { // --- カメラのホーミングタイプ ---
	DIRECT,			// ダイレクトにカメラ座標にターゲット座標を設定する
	LERP,			// カメラとターゲット座標を線形補完する
	SLERP,			// カメラとターゲット座標を曲線補完する
	STOP,			// カメラを止める
}

public class CameraFollow : MonoBehaviour 
{
	// === 外部パラメータ（インスペクタ表示） =====================
	[System.Serializable]
	public class Param {
		public CAMERATARGET tragetType 			= CAMERATARGET.PLAYER_GROUND;
		public CAMERAHOMING homingTypeX 			= CAMERAHOMING.LERP;
		public CAMERAHOMING homingTypeY 			= CAMERAHOMING.LERP;
		public Vector2 		margin 				= new Vector2 (2.0f, 2.0f);
		public Vector2 		homing 				= new Vector2 (0.1f, 0.2f);
		public bool			borderCheck 		= false;
		public GameObject	borderLeftTop;
		public GameObject	borderRightBottom;
		public bool			viewAreaCheck		= true;
		public Vector2		viewAreaMinMargin	= new Vector2(0.0f,0.0f); 
		public Vector2		viewAreaMaxMargin	= new Vector2(0.0f,2.0f); 

		public bool			orthographicEnabled = true;
		public float		screenOGSize		= 5.0f;
		public float		screenOGSizeHoming	= 0.1f;
		public float		screenPSSize		= 50.0f;
		public float		screenPSSizeHoming	= 0.1f;
	}
	public Param param;

	// === キャッシュ ==========================================
	GameObject 		 	player;
	Transform 		 	playerTrfm;
    PlayerController playerCtrl;

	float				screenOGSizeAdd = 0.0f;
	float				screenPSSizeAdd = 0.0f;
	Camera camera;
	// === コード（Monobehaviour基本機能の実装） ================
	void Awake () {
		camera = transform.GetComponent<Camera> ();
		player = GameObject.Find ("Player");
		playerTrfm 	= player.transform;
		playerCtrl  = player.GetComponent<PlayerController>();
	}

	void LateUpdate () {
		transform.position = playerTrfm.position - Vector3.forward * 10;
		return;
		float targetX 	= playerTrfm.position.x;
		float targetY 	= playerTrfm.position.y;

		float pX 		= transform.position.x;
		float pY 		= transform.position.y;
		float screenOGSize = Camera.main.orthographicSize;
		float screenPSSize = Camera.main.fieldOfView;

		// ターゲットの設定
		switch (param.tragetType) {
		case CAMERATARGET.PLAYER			:
			targetX = playerTrfm.position.x;
			targetY = playerTrfm.position.y;
			break;
		case CAMERATARGET.PLAYER_MARGIN	:
			targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;
			targetY = playerTrfm.position.y + param.margin.y;
			break;
		case CAMERATARGET.PLAYER_GROUND	:
			targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;
			targetY = playerCtrl.groundY + param.margin.y;
			break;
		}

		// カメラの移動限界境界線のチェック
		if (param.borderCheck) {
			float cX = playerTrfm.transform.position.x;
			float cY = playerTrfm.transform.position.y;
			if (cX < param.borderLeftTop.transform.position.x ||
				cX > param.borderRightBottom.transform.position.x ||
				cY > param.borderLeftTop.transform.position.y || 
				cY < param.borderRightBottom.transform.position.y) {
				return;
			}
		}

		switch (param.homingTypeY) {
		case CAMERAHOMING.DIRECT 		:
			pY = targetY;

			break;
		case CAMERAHOMING.LERP 			:
			//pX = transform.position.x + (targetX - transform.position.x) * homing.x;
			//pY = transform.position.y + (targetY - transform.position.y) * homing.y;
			pY = Mathf.Lerp(transform.position.y,targetY,param.homing.y);
		break;
		case CAMERAHOMING.SLERP 		:
			pY = Mathf.SmoothStep(transform.position.y,targetY,param.homing.y);

			break;
		case CAMERAHOMING.STOP 			:
			break;
		}
		Debug.Log (targetX + ":" + transform.position.x);
		transform.position = new Vector3 (targetX, pY, -10);
		return;
		// プレイヤーのカメラ内チェック
		if (param.viewAreaCheck) {
			float 	z   = playerTrfm.position.z - transform.position.z;
			Vector3 minMargin = param.viewAreaMinMargin;
			Vector3 maxMargin = param.viewAreaMaxMargin;
			Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0.0f,0.0f,z)) - minMargin;
			Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1.0f,1.0f,z)) - maxMargin;
			if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x) {
				targetX = playerTrfm.position.x;
			}
			if (playerTrfm.position.y < min.y || playerTrfm.position.y > max.y) {
				targetY = playerTrfm.position.y;
				playerCtrl.groundY = playerTrfm.position.y;
			}
		}

		// カメラ移動（ホーミング）
		switch (param.homingTypeX) {
		case CAMERAHOMING.DIRECT 		:
			pX = targetX;
			screenOGSize = param.screenOGSize;
			screenPSSize = param.screenPSSize;
			break;
		case CAMERAHOMING.LERP 			:
			//pX = transform.position.x + (targetX - transform.position.x) * homing.x;
			//pY = transform.position.y + (targetY - transform.position.y) * homing.y;
			pX = Mathf.Lerp(transform.position.x,targetX,param.homing.x);
			screenOGSize = Mathf.Lerp(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
			screenPSSize = Mathf.Lerp(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
			break;
		case CAMERAHOMING.SLERP 		:
			pX = Mathf.SmoothStep(transform.position.x,targetX,param.homing.x);
			screenOGSize = Mathf.SmoothStep(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
			screenPSSize = Mathf.SmoothStep(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
			break;
		case CAMERAHOMING.STOP 			:
			break;
		}
		switch (param.homingTypeY) {
		case CAMERAHOMING.DIRECT 		:
			pY = targetY;
			screenOGSize = param.screenOGSize;
			screenPSSize = param.screenPSSize;
			break;
		case CAMERAHOMING.LERP 			:
			//pX = transform.position.x + (targetX - transform.position.x) * homing.x;
			//pY = transform.position.y + (targetY - transform.position.y) * homing.y;
			pY = Mathf.Lerp(transform.position.y,targetY,param.homing.y);
			screenOGSize = Mathf.Lerp(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
			screenPSSize = Mathf.Lerp(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
			break;
		case CAMERAHOMING.SLERP 		:
			pY = Mathf.SmoothStep(transform.position.y,targetY,param.homing.y);
			screenOGSize = Mathf.SmoothStep(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
			screenPSSize = Mathf.SmoothStep(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
			break;
		case CAMERAHOMING.STOP 			:
			break;
		}
		transform.position 		= new Vector3 (pX,pY, transform.position.z);

	}

	// === コード（その他） ====================================
	public void SetCamera(Param cameraPara ) {
		param = cameraPara;
	}

	public void AddCameraSize(float ogAdd,float psAdd) {
		screenOGSizeAdd += ogAdd;
		screenPSSizeAdd += psAdd;
	}	
}


//using UnityEngine;
//using System.Collections;
//
//public enum CAMERATARGET { // --- カメラのターゲットタイプ ---
//	PLAYER,			// プレイヤー座標
//	PLAYER_MARGIN,	// プレイヤー座標（前方視界を確保するマージン付き）
//	PLAYER_GROUND,	// 過去にプレイヤーが接地した地面の座標（前方視界を確保するマージン付き）
//}
//
//public enum CAMERAHOMING { // --- カメラのホーミングタイプ ---
//	DIRECT,			// ダイレクトにカメラ座標にターゲット座標を設定する
//	LERP,			// カメラとターゲット座標を線形補完する
//	SLERP,			// カメラとターゲット座標を曲線補完する
//	STOP,			// カメラを止める
//}
//
//public class CameraFollow : MonoBehaviour 
//{
//	// === 外部パラメータ（インスペクタ表示） =====================
//	[System.Serializable]
//	public class Param {
//		public CAMERATARGET tragetType 			= CAMERATARGET.PLAYER_GROUND;
//		public CAMERAHOMING homingTypeX 			= CAMERAHOMING.LERP;
//		public CAMERAHOMING homingTypeY 			= CAMERAHOMING.LERP;
//		public Vector2 		margin 				= new Vector2 (2.0f, 2.0f);
//		public Vector2 		homing 				= new Vector2 (0.1f, 0.2f);
//		public bool			borderCheck 		= false;
//		public GameObject	borderLeftTop;
//		public GameObject	borderRightBottom;
//		public bool			viewAreaCheck		= true;
//		public Vector2		viewAreaMinMargin	= new Vector2(0.0f,0.0f); 
//		public Vector2		viewAreaMaxMargin	= new Vector2(0.0f,2.0f); 
//
//		public bool			orthographicEnabled = true;
//		public float		screenOGSize		= 5.0f;
//		public float		screenOGSizeHoming	= 0.1f;
//		public float		screenPSSize		= 50.0f;
//		public float		screenPSSizeHoming	= 0.1f;
//	}
//	public Param param;
//
//	// === キャッシュ ==========================================
//	GameObject 		 	player;
//	Transform 		 	playerTrfm;
//	PlayerController 	playerCtrl;
//
//	float				screenOGSizeAdd = 0.0f;
//	float				screenPSSizeAdd = 0.0f;
//	Camera camera;
//	// === コード（Monobehaviour基本機能の実装） ================
//	void Awake () {
//		camera = transform.GetComponent<Camera> ();
//		player 		= PlayerController.GetGameObject();
//		playerTrfm 	= player.transform;
//		playerCtrl  = player.GetComponent<PlayerController>();
//	}
//
//	void LateUpdate () {
//		float targetX 	= playerTrfm.position.x;
//		float targetY 	= playerTrfm.position.y;
//		float pX 		= transform.position.x;
//		float pY 		= transform.position.y;
//		float screenOGSize = Camera.main.orthographicSize;
//		float screenPSSize = Camera.main.fieldOfView;
//
//		// ターゲットの設定
//		switch (param.tragetType) {
//		case CAMERATARGET.PLAYER			:
//			targetX = playerTrfm.position.x;
//			targetY = playerTrfm.position.y;
//			break;
//		case CAMERATARGET.PLAYER_MARGIN	:
//			targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;
//			targetY = playerTrfm.position.y + param.margin.y;
//			break;
//		case CAMERATARGET.PLAYER_GROUND	:
//			targetX = playerTrfm.position.x + param.margin.x * playerCtrl.dir;
//			targetY = playerCtrl.groundY + param.margin.y;
//			break;
//		}
//
//		// カメラの移動限界境界線のチェック
//		if (param.borderCheck) {
//			float cX = playerTrfm.transform.position.x;
//			float cY = playerTrfm.transform.position.y;
//			if (cX < param.borderLeftTop.transform.position.x ||
//			    cX > param.borderRightBottom.transform.position.x ||
//			    cY > param.borderLeftTop.transform.position.y || 
//			    cY < param.borderRightBottom.transform.position.y) {
//				return;
//			}
//		}
//
//		// プレイヤーのカメラ内チェック
//		if (param.viewAreaCheck) {
//			float 	z   = playerTrfm.position.z - transform.position.z;
//			Vector3 minMargin = param.viewAreaMinMargin;
//			Vector3 maxMargin = param.viewAreaMaxMargin;
//			Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0.0f,0.0f,z)) - minMargin;
//			Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1.0f,1.0f,z)) - maxMargin;
//			if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x) {
//				targetX = playerTrfm.position.x;
//			}
//			if (playerTrfm.position.y < min.y || playerTrfm.position.y > max.y) {
//				targetY = playerTrfm.position.y;
//				playerCtrl.groundY = playerTrfm.position.y;
//			}
//		}
//
//		// カメラ移動（ホーミング）
//		switch (param.homingTypeX) {
//		case CAMERAHOMING.DIRECT 		:
//			pX = targetX;
//			screenOGSize = param.screenOGSize;
//			screenPSSize = param.screenPSSize;
//			break;
//		case CAMERAHOMING.LERP 			:
//			//pX = transform.position.x + (targetX - transform.position.x) * homing.x;
//			//pY = transform.position.y + (targetY - transform.position.y) * homing.y;
//			pX = Mathf.Lerp(transform.position.x,targetX,param.homing.x);
//			screenOGSize = Mathf.Lerp(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
//			screenPSSize = Mathf.Lerp(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
//			break;
//		case CAMERAHOMING.SLERP 		:
//			pX = Mathf.SmoothStep(transform.position.x,targetX,param.homing.x);
//			screenOGSize = Mathf.SmoothStep(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
//			screenPSSize = Mathf.SmoothStep(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
//			break;
//		case CAMERAHOMING.STOP 			:
//			break;
//		}
//		switch (param.homingTypeY) {
//		case CAMERAHOMING.DIRECT 		:
//			pY = targetY;
//			screenOGSize = param.screenOGSize;
//			screenPSSize = param.screenPSSize;
//			break;
//		case CAMERAHOMING.LERP 			:
//			//pX = transform.position.x + (targetX - transform.position.x) * homing.x;
//			//pY = transform.position.y + (targetY - transform.position.y) * homing.y;
//			pY = Mathf.Lerp(transform.position.y,targetY,param.homing.y);
//			screenOGSize = Mathf.Lerp(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
//			screenPSSize = Mathf.Lerp(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
//			break;
//		case CAMERAHOMING.SLERP 		:
//			pY = Mathf.SmoothStep(transform.position.y,targetY,param.homing.y);
//			screenOGSize = Mathf.SmoothStep(screenOGSize,param.screenOGSize,param.screenOGSizeHoming);
//			screenPSSize = Mathf.SmoothStep(screenPSSize,param.screenPSSize,param.screenPSSizeHoming);
//			break;
//		case CAMERAHOMING.STOP 			:
//			break;
//		}
//		transform.position 		= new Vector3 (pX,pY, transform.position.z);
//		camera.orthographic 	= param.orthographicEnabled;
//		camera.orthographicSize = screenOGSize + screenOGSizeAdd;
//		camera.fieldOfView 		= screenPSSize + screenPSSizeAdd;
//		camera.orthographicSize = Mathf.Clamp (camera.orthographicSize,  2.5f,   10.0f);
//		camera.fieldOfView		= Mathf.Clamp (camera.fieldOfView     , 30.0f,  100.0f);
//
//		// カメラの特殊ズーム効果計算
//		screenOGSizeAdd *= 0.99f;
//		screenPSSizeAdd *= 0.99f;
//	}
//
//	// === コード（その他） ====================================
//	public void SetCamera(Param cameraPara ) {
//		param = cameraPara;
//	}
//
//	public void AddCameraSize(float ogAdd,float psAdd) {
//		screenOGSizeAdd += ogAdd;
//		screenPSSizeAdd += psAdd;
//	}	
//}
//
