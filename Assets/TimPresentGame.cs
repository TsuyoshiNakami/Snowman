using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimPresentGame : MonoBehaviour {
    
    public enum State
    {
        Run,
        Cook,
        Toss
    }
    public State state;



    Subject<Unit> tossSubject = new Subject<Unit>();
    Subject<Unit> tossAnimeEndSubject = new Subject<Unit>();
    public IObservable<Unit> OnTossEvent
    {
        get { return tossSubject; }
    }

    public IObservable<Unit> OnTossAnimeEndEvent
    {
        get { return tossAnimeEndSubject; }
    }


    Animator anime;
    [SerializeField] float cookTime = 1;
    [SerializeField] float moveSpeed = 4;

    public Transform runDest;

    // Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();


    }
	
	// Update is called once per frame
	void Update () {
	        if(runDest != null)
        {
            Move();
        }	
	}

    public void Cook()
    {
        anime.SetFloat("CookKind", Random.Range(0, 2));
        anime.SetTrigger("Cook");
        Invoke("Toss", cookTime);
        state = State.Cook;
    }

    public void Toss()
    {
        anime.SetTrigger("Toss");
        state = State.Toss;
    }

    public void OnToss()
    {
        tossSubject.OnNext(Unit.Default);
    }
    public void OnTossAnimeEnd()
    {
        tossAnimeEndSubject.OnNext(Unit.Default);
    }

    public void StartRun()
    {
        state = State.Run;

    }
    public void Move()
    {
        float dir = transform.position.x < runDest.position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1) * 2;
        transform.Translate(Vector3.right * dir * moveSpeed * Time.deltaTime);

        if(Mathf.Abs(transform.position.x - runDest.position.x) < 0.6f)
        {
            runDest = null;
            Cook();
        }
    }
}
