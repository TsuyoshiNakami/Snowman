﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PresentGameManager : MonoBehaviour {

    Subject<Unit> timerSubject = new Subject<Unit>();
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

        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
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
