using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using Tsuyomi.Yukihuru.Scripts.Utilities;

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
	public int maxCoin = 6;
	public static bool[] getCoins;
	public static bool enableAllWarp;
	public bool enableAllWarpDebug = false;
	public int animeCount = -2;
	[SerializeField] string sceneNameSerial = "Stage1";
    [SerializeField] GameObject playerObj;
    [SerializeField] MessageWindowController messageWindowController;
	public bool ReloadStage = false;
	CameraFol cameraFol;
	public static bool firstCheckPoint = false;
	public static bool firstCoinGet = false;
	SoundManager soundManager;
	static AsyncOperation ao;
	public static bool isTitle = true;


    string loadingSceneName = "";

    
	void Start () {

        //Invoke("ShowTestMsg", 5);

        if (GameObject.Find("Player") == null)
        {
            AddPlayer();
        }
        //SetCamera();

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

    public void PlaySE(string name)
    {
        soundManager.PlaySEOneShot(name);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !messageWindowController.isShowing)
        {
            ShowTestMsg();

        }

        if(Input.GetKeyDown(KeyCode.C)) {
            float max = Mathf.Max(Screen.width, Screen.height);
            //int scale = Mathf.RoundToInt(2048 / max);
            ScreenCapture.CaptureScreenshot("image.png", 1);
        }
    }

    void ShowTestMsg()
    {
        List<string> a = new List<string>();
        a.Add("@Face Tim");
        a.Add("キミは…！なんという格好をしてるんだ。頭部だけじゃないか。");
        a.Add("でも、そういう自分のスタイルを貫く感じ、すごくうらやましい。ボクにもそんな生き方ができるのかな？");
        a.Add("@Face Tobe");
        a.Add("本番ではしゃべる予定ないけど、テストでしゃべってみるぞ。");
        a.Add("しゃべるっていう漢字が表示できない…レアな漢字は事前に登録しておかないとダメなんだ。");
        a.Add("@Face Cake");
        a.Add("あたいはケーキ。16さい。");
        ShowMessage(a);
    }

    public static void LoadScene(GameScenes scene)
    {
        SceneLoader.LoadScene(scene);
        //loadingSceneName = name;
        //StartCoroutine(ILoadScene());
    }

    IEnumerator ILoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadingSceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (loadingSceneName == PresentGameConsts.sceneGame)
        {
            SetCamera();
        }
    }

    void AddPlayer()
    {
        GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint");
        GameObject tempPlayerObj = Instantiate(playerObj, spawnPoint.transform.position, spawnPoint.transform.rotation);
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

    public void ShowMessage(List<string> messages)
    {
        messageWindowController.StartMessage(messages);
    }
}
