using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

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
    }

    void ShowTestMsg()
    {
        List<string> a = new List<string>();
        a.Add("あいうえお");
        a.Add("春はあけぼの。やうやう白くなりゆく山際、少し明かりて、紫だちたる雲の細くたなびきたる。");
        a.Add("夏は夜。月のころはさらなり、闇もなほ、蛍の多く飛びちがひたる。また、ただ一つ二つなど、ほのかにうち光て行くもをかし。雨など降るもをかし。");
        ShowMessage(a);
    }

    public void LoadScene(string name)
    {
        loadingSceneName = name;
        StartCoroutine(ILoadScene());
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
