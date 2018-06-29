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
	static PlayerController playerController;
	static bool isLoad = false;
	public static bool isFirstPlay = true;
	public int maxCoin = 6;
	public static bool[] getCoins;
	public static bool enableAllWarp;
	public bool enableAllWarpDebug = false;
	public int animeCount = -2;
	[SerializeField] string sceneNameSerial = "Stage1";
    [SerializeField] GameObject playerObj;
	public bool ReloadStage = false;
	CameraFol cameraFol;
	public static bool firstCheckPoint = false;
	public static bool firstCoinGet = false;
	SoundManager soundManager;
	static AsyncOperation ao;
	public static bool isTitle = true;
    public static int score = 0;
    
	void Start () {
        AddPlayer();
        SetCamera();

		enableAllWarp = enableAllWarpDebug;
		getCoins = new bool[maxCoin];
		for (int i = 0; i < maxCoin; i++) {
			getCoins [i] = false;
		}
		currentSceneName = sceneNameSerial;
		DontDestroyOnLoad (gameObject);
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();

        if (playerController == null)
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            cameraFol = GameObject.FindWithTag("MainCamera").GetComponent<CameraFol>();
        }
    }

	public static void LoadScene(string name) {

			isFirstPlay = false;

		if (isLoad)
			return;

		currentSceneName = name;
		isLoad = true;
		//	UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (name, UnityEngine.SceneManagement.LoadSceneMode.Single);
		ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (currentSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single); //Application.LoadLevelAsync(currentSceneName);
		ao.allowSceneActivation = false;
		Timer.InitTimer ();
	}

    void AddPlayer()
    {
        GameObject tempPlayerObj = Instantiate(playerObj, GameObject.Find("PlayerSpawnPoint").transform);
        tempPlayerObj.name = "Player";
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void SetCamera()
    {
        cameraFol = GameObject.FindWithTag("MainCamera").AddComponent<CameraFol>();
        cameraFol.smoothingY = 0.2f;
        cameraFol.offsetY = 0.3f;
        cameraFol.followPlayer = true;
    }
    public static PlayerController GetPlayerController()
    {
        return playerController;
    }
}
