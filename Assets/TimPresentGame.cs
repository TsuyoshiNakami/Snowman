using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimPresentGame : MonoBehaviour {
    Subject<Unit> tossSubject = new Subject<Unit>();
    public IObservable<Unit> OnToss
    {
        get { return tossSubject; }
    }
    Animator anime;
    [SerializeField] float cookTime = 1;
	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Cook()
    {
        anime.SetFloat("CookKind", Random.Range(0, 2));
        anime.SetTrigger("Cook");
        Invoke("Toss", cookTime);
    }

    public void Toss()
    {
        anime.SetTrigger("Toss");
    }
    public void OnTossAnimeEnd()
    {
        tossSubject.OnNext(Unit.Default);
    }
}
