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

    [SerializeField] string sceneNameSerial = "Stage1";
    [SerializeField] GameObject playerObj;
    [SerializeField] MessageWindowController messageWindowController;
	public bool ReloadStage = false;
	CameraFol cameraFol;
	SoundManager soundManager;
	public static bool isTitle = true;


    string loadingSceneName = "";

    
	void Start () {
		currentSceneName = sceneNameSerial;
		DontDestroyOnLoad (gameObject);
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();
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
