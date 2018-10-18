using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class PresentGameManager : MonoBehaviour {
    public static int score = 0;

    [SerializeField] bool emitFoodEater = false;
    [SerializeField] GameObject resultWindow;
    [SerializeField] TextMeshProUGUI startText;

    Subject<Unit> timerSubject = new Subject<Unit>();
    SoundManager soundManager;

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
    [System.NonSerialized]public bool isTimerAvailable = false;

    [SerializeField, Header("制限時間")] float initialTimeLimit = 90;
    PresentManager presentManager;

    public float TimeLimit
    {
        get { return timeLimit; }
    }

    private void Start()
    {
        startText.gameObject.SetActive(false);
        score = 0;
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.PlayBGM("Main");
        StartCountDown();
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

        if(emitFoodEater && Random.Range(0, 1000) < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/Mouse"), new Vector3(25.17f, 0f, 0f), Quaternion.identity);
        }

    }

    void OnTimerEnd()
    {
        presentManager.DeleteAllPresents();
        resultWindow.gameObject.SetActive(true);
        resultWindow.GetComponent<ResultManager>().ShowResult();
    }
}
