using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.SceneManagement;

public class PresentGameManager : MonoBehaviour {
    public static int score = 0;

    [SerializeField] ResultManager resultWindow;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] int initScore = 0;
    Subject<Unit> timerSubject = new Subject<Unit>();
    SoundManager soundManager;
    public bool enablePresentEmit = false;
    [SerializeField] FoodEaterEmitter foodEaterEmitter;

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
    [System.NonSerialized]public bool isTimerAvailable = false;

    [SerializeField, Header("制限時間")] float initialTimeLimit = 90;
    PresentManager presentManager;
    PlayerController playerController;

    public float TimeLimit
    {
        get { return timeLimit; }
    }

    private void Start()
    {
        startText.gameObject.SetActive(false);
        score = initScore;
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.PlayBGM("Main");
        StartCountDown();
        playerController = PlayerController.GetController();
        RankingManager.hasSendRanking = false;
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
        if(isRankingOpen)
        {
            if(Input.GetButtonDown(KeyConfig.Jump))
            {
                CloseRanking();
            }
        }
        if(!isTimerAvailable)
        {
            return;
        }

        if(Pauser.isPausing)
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
        
        if(foodEaterEmitter.emitStartTime <= initialTimeLimit - timeLimit)
        {
            foodEaterEmitter.isStartedEmit = true;
        }
    }

    void OnTimerEnd()
    {
        gameFinished = true;
        playerController.activeSts = false;
        presentManager.DeleteAllPresents();
        resultWindow.gameObject.SetActive(true);
        resultWindow.ShowResult();
        enablePresentEmit = true;
        foodEaterEmitter.isStartedEmit = false;
    }

    public void OpenRanking()
    {
        resultWindow.SetButtonsInteractive(false);
        SceneManager.LoadScene("RankingAdditive", LoadSceneMode.Additive);
        isRankingOpen = true;
    }

    public void CloseRanking()
    {
        resultWindow.SetButtonsInteractive(true);
        SceneManager.UnloadSceneAsync("RankingAdditive");
        isRankingOpen = false;
        resultWindow.InitButtonFocus();
    }
}
