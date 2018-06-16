using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class PlayerData {
	public int checkPointNumber = 0;
	public bool bossDefeated = false;
	public bool middleBossDefeated = false;
	public bool isPowerUp = false;
	public PlayerData() {
	}
}
public class GameManager : SingletonMonoBehaviourFast<GameManager> {
	public static string currentSceneName = "Stage1";
	public static int PauserFlag = 0;
	public static Vector2 WarpedPlayerPosition = new Vector2(-7, 1.5f);
	static PlayerController pc;
	 public static PlayerData playerData;
	static bool isLoad = false;
	public static bool isFirstPlay = true;
	public int maxCoin = 6;
	public static bool[] getCoins;
	public static bool enableAllWarp;
	public bool enableAllWarpDebug = false;
	public int animeCount = -2;
	[SerializeField] string sceneNameSerial = "Stage1";
	public bool ReloadStage = false;
	CameraFol cameraFol;
	public static bool firstCheckPoint = false;
	public static bool firstCoinGet = false;
	SoundManager soundManager;
	static AsyncOperation ao;
	public static bool isTitle = true;
    public static int score = 0;

	// Use this for initialization
	void Start () {


        pc = GameObject.Find ("Player").GetComponent<PlayerController> ();
		cameraFol = GameObject.FindWithTag ("MainCamera").GetComponent<CameraFol> ();
		enableAllWarp = enableAllWarpDebug;
		getCoins = new bool[maxCoin];
		for (int i = 0; i < maxCoin; i++) {
			getCoins [i] = false;
		}
		currentSceneName = sceneNameSerial;
		playerData = new PlayerData();
		DontDestroyOnLoad (gameObject);
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();

        if (pc == null)
        {

            pc = GameObject.Find("Player").GetComponent<PlayerController>();
            cameraFol = GameObject.FindWithTag("MainCamera").GetComponent<CameraFol>();
        }
    }
	public static void LoadScene(string name) {


			isFirstPlay = false;

		if (isLoad)
			return;

		PauserFlag++;

		currentSceneName = name;
		isLoad = true;
		//	UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (name, UnityEngine.SceneManagement.LoadSceneMode.Single);
		ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (currentSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single); //Application.LoadLevelAsync(currentSceneName);
		ao.allowSceneActivation = false;
		Timer.InitTimer ();
	}

}
