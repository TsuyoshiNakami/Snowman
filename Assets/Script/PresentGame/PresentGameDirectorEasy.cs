using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PresentGameDirectorEasy : MonoBehaviour, IPresentGameDirector
{
    Subject<Unit> generatePresentSubject = new Subject<Unit>();
    [SerializeField]public float generateInterval;
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

    float generateTimer = 0;

    [SerializeField] int maxPresentInView = 20;

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
                tim.OnTossAnimeEndEvent.First().Subscribe(_ =>
                {
                    tim.StartRun();
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
           GameObject newPresentObj = presentManager.EmitPresentRandom(tim.transform.position);
    }
    public void OnTimerEnd()
    {

    }
}
