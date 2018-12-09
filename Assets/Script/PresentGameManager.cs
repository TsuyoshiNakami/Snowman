using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.SceneManagement;
using Zenject;
using Tsuyomi.Yukihuru.Scripts.Utilities;
using naichilab;

#if Engineer
using Rewired;
#endif


public class PresentGameManager : MonoBehaviour
{
    public static int score = 0;

    [SerializeField] ResultManager resultWindow;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] int initScore = 0;
    [SerializeField] PauseWindow pauseWindow;
    [SerializeField] NumberDisplay scoreDisplay;
    [SerializeField] NumberDisplay timerDisplay;

#if Engineer
    Player player;
#endif

    Subject<Unit> timerSubject = new Subject<Unit>();
    SoundManager soundManager;
    public bool enablePresentEmit = false;


    public IObservable<Unit> OnTimeUp
    {
        get { return timerSubject; }
    }

    Subject<Unit> presentGameStartSubject = new Subject<Unit>();
    public IObservable<Unit> OnPresentGameStart
    {
        get { return timerSubject; }
    }

    float timeLimit = 0;
    bool isRankingOpen = false;
    public bool gameFinished = false;
    [System.NonSerialized] public bool isTimerAvailable = false;

    [SerializeField, Header("制限時間")] public  float initialTimeLimit = 90;
    [Inject]
    PresentManager presentManager;
    PlayerController playerController;

    [Inject]
    IPresentGameDirector gameDirector;

    public float TimeLimit
    {
        get { return timeLimit; }
    }

    private void Awake()
    {
                if (ExistSubScene)
        {
            //SceneManager.LoadScene(GameScenes.GameEasy.ToString(), LoadSceneMode.Additive);
        }
    }
    private void Start()
    {

        startText.gameObject.SetActive(false);
        score = initScore;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.PlayBGM("Main");
#if Engineer
        player = ReInput.players.GetPlayer(0);
#endif
        StartCountDown();
        playerController = PlayerController.GetController();
        RankingManager.hasSendRanking = false;
    }

    bool ExistSubScene
    {
        get
        {
            return SceneManager.GetSceneByName("GameEasy") != null || SceneManager.GetSceneByName("GameHard") != null;
        }
    }
    void StartCountDown()
    {
        startText.gameObject.SetActive(true);
        StartCoroutine(ICountDown());
    }

    IEnumerator ICountDown()
    {
        startText.text = "3";
        yield return new WaitForSeconds(1);
        enablePresentEmit = true;
        startText.text = "2";
        yield return new WaitForSeconds(1);

        startText.text = "1";
        yield return new WaitForSeconds(1);

        startText.text = "スタート！";
        yield return new WaitForSeconds(1);
        startText.gameObject.SetActive(false);
        SetTimer(initialTimeLimit);
    }
    public void SetTimer(float f)
    {
        presentGameStartSubject.OnNext(Unit.Default);
        timeLimit = f;
        isTimerAvailable = true;
    }



    private void Update()
    {
        if (isRankingOpen)
        {
            if (Input.GetButtonDown(KeyConfig.Jump))
            {
                OnCloseRanking();
            }
        }

        scoreDisplay.SetNumber(score);
        timerDisplay.SetNumber((int)Mathf.Ceil(TimeLimit));
        if (gameFinished)
        {
            return;
        }


#if Engineer
        if (player.GetButtonDown("Decide"))
        {
            pauseWindow.OnDecideButtonPressed();
        }

        if(player.GetButtonDown("Home"))
        {
            pauseWindow.OnHomeButtonPressed();
        }
#else
        if (Input.GetButtonDown(KeyConfig.Decide))
        {
            pauseWindow.OnDecideButtonPressed();
        }

        if (Input.GetButtonDown(KeyConfig.Home))
        {
            pauseWindow.OnHomeButtonPressed();
        }
#endif


        if (!isTimerAvailable)
        {
            return;
        }

        


        if (Pauser.isPausing)
        {
            return;
        }
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            timerSubject.OnNext(Unit.Default);
            isTimerAvailable = false;
            OnTimerEnd();
        }
        gameDirector.GameUpdate(timeLimit);

    }

    void OnTimerEnd()
    {
        gameFinished = true;
        playerController.activeSts = false;
        presentManager.DeleteAllPresents();
        resultWindow.gameObject.SetActive(true);
        resultWindow.ShowResult();
        enablePresentEmit = true;
        gameDirector.OnTimerEnd();
        GameObject.Find("RankingLoader").GetComponent<RankingLoader>().OnCloseRanking.Subscribe(_ => {
            OnCloseRanking();
        });
    }

    public void OnOpenRanking()
    {
        resultWindow.SetButtonsInteractive(false);
        isRankingOpen = true;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);

        //SceneManager.LoadScene("RankingAdditive", LoadSceneMode.Additive);
    }

    public void OnCloseRanking()
    {
        resultWindow.SetButtonsInteractive(true);
        //SceneManager.UnloadSceneAsync("RankingAdditive");
        isRankingOpen = false;
        resultWindow.InitButtonFocus();
    }
}
