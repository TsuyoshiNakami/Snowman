using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using System;

using Random = UnityEngine.Random;

public class PresentGameDirectorEasy : MonoBehaviour, IPresentGameDirector
{
    Subject<Unit> generatePresentSubject = new Subject<Unit>();
    [SerializeField] public float generateInterval;
    [SerializeField] List<GameObject> presentEmitPoints;
    [Inject]
    PresentManager presentManager;

    [SerializeField] TimPresentGame tim;


    [SerializeField] GameObject TimObj;

    public IObservable<Unit> OnGeneratePresent
    {
        get
        {
            return generatePresentSubject;
        }
    }

    Animator timAnime;
    float generateTimer = 0;
    bool hurryUpMode;
    [SerializeField] int maxPresentInView = 20;

    private void Start()
    {
        timAnime = tim.GetComponent<Animator>();
    }
    public void GameUpdate(float timeLimit)
    {
    }

    public void PresentEmitUpdate()
    {
        int point = Random.Range(0, presentEmitPoints.Count);
        if (tim.state == TimPresentGame.State.Run)
        {
            generateTimer += Time.deltaTime;
            if (tim.runDest == null)
            {
                tim.OnTossEvent.First().Subscribe(_ =>
                {
                    TossPresent(point);
                });

                if (tim.runDest == null)
                {
                    tim.runDest = presentEmitPoints[point].transform;
                }
            }
        }

        if (presentManager.NumberOfPresentInView >= maxPresentInView)
        {
            return;
        }
    }

    public void TossPresent(int point)
    {


        if (hurryUpMode)
        {
            StartCoroutine(TossPresentInHurryUp());
        }
        else
        {
            StartCoroutine(ITossPresent());
        }
    }

    IEnumerator ITossPresent()
    {
        GameObject newPresentObj = presentManager.EmitPresentRandom(tim.transform.position);

        tim.OnTossAnimeEndEvent.First().Subscribe(_ =>
                {
                    tim.StartRun();
                });
        yield return null;

    }
    IEnumerator TossPresentInHurryUp()
    {
        GameObject newPresentObj = presentManager.EmitPresentRandom(tim.transform.position);
        bool flag = false;
        tim.OnTossAnimeEndEvent.First().Subscribe(_ =>
                {
                    flag = true;
                });
        while (!flag)
        {
            yield return null;
        }
        timAnime.SetTrigger("Toss");

        flag = false;
        tim.OnTossEvent.First().Subscribe(_ =>
                {
                    newPresentObj = presentManager.EmitPresentRandom(tim.transform.position);
                    tim.OnTossAnimeEndEvent.First().Subscribe(_2 =>
                            {
                                tim.StartRun();
                            });
                    flag = true;
                });

    }
    public void OnTimerEnd()
    {

    }

    public void HurryUp()
    {
        hurryUpMode = true;
    }
}
