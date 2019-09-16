using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimOpening : MonoBehaviour {
    Subject<Unit> suprisedBySnowmanSubject = new Subject<Unit>();

    public IObservable<Unit> OnSuprisedBySnowman
    {
        get { return suprisedBySnowmanSubject; }
    }


    Subject<Unit> hitSnowmanSubject = new Subject<Unit>();

    public IObservable<Unit> OnHitSnowman {
        get { return hitSnowmanSubject; }
    }

    public bool patSound = true;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    public void HitSnowman()
    {
        if(patSound)
        {
            soundManager.PlaySE("TimPat");
        }
        hitSnowmanSubject.OnNext(Unit.Default);
    }

    public void OnBlink()
    {
            soundManager.PlaySE("TimBlink");
    }
    public void SuprisedBySnowman()
    {
        suprisedBySnowmanSubject.OnNext(Unit.Default);
    }
	
}
