using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class OpeningSnowman : MonoBehaviour {
    [SerializeField] GameObject taube;
    [SerializeField] GameObject generatePoint;

    Subject<Unit> taubeAppearSubject = new Subject<Unit>();
    public IObservable<Unit> OnTaubeAppear
    {
        get { return taubeAppearSubject; }
    }

    GameObject taubeObjGlobal;
    public void GenerateTobe()
    {
        GameObject taubeObj = Instantiate(taube, generatePoint.transform.position, Quaternion.identity);
        taubeObjGlobal = taubeObj;
        taubeObj.GetComponent<PlayerController>().activeSts = false;
        taubeObjGlobal.GetComponent<Animator>().SetTrigger("Flyout");
        Invoke("SetAnime", 0.5f);
        taubeAppearSubject.OnNext(Unit.Default);
    }
    void SetAnime()
    {

    }
}
