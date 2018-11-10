using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PresentGameDirectorEasy : MonoBehaviour, IPresentGameDirector
{
    Subject<Unit> generatePresentSubject = new Subject<Unit>();
    [SerializeField]float generateInterval;
    [SerializeField] List<GameObject> presentEmitPoints;
    [Inject]
    PresentManager presentManager;



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
        generateTimer += Time.deltaTime;

        if (presentManager.NumberOfPresentInView >= maxPresentInView)
            {
                return;
            }
        if (generateTimer > generateInterval)
        {
               GameObject newPresentObj = presentManager.EmitPresentRandom(presentEmitPoints[point].transform.position);
            TimObj.transform.position = presentEmitPoints[point].transform.position;
            generateTimer = 0;
        }
    }

    public void OnTimerEnd()
    {

    }
}
