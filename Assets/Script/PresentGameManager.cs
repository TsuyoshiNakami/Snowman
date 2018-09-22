﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class PresentGameManager : MonoBehaviour {
    public static int score = 0;


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
        score = 0;
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.PlayBGM("Main");
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

    }

    void OnTimerEnd()
    {
        presentManager.DeleteAllPresents();
    }
}