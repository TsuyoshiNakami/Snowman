using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PresentGameDirectorHard : MonoBehaviour, IPresentGameDirector
{
    Subject<Unit> generatePresentSubject = new Subject<Unit>();
    float generateInterval;
    [SerializeField] List<GameObject> presentEmitPoints;
    [SerializeField] FoodEaterEmitter foodEaterEmitter;
    [Inject]
    PresentManager presentManager;
    float beltTimer = 0;
    [SerializeField] bool beltEmit = true;
    [SerializeField]float beltInterval = 3;
    [SerializeField] GameObject presentDelivererObj;
    [SerializeField] Transform presentDelivererPoint;
    [SerializeField] Transform presentDelivererPoint2;
    public IObservable<Unit> OnGeneratePresent
    {
        get
        {
            return generatePresentSubject;
        }
    }

    float generateTimer = 0;

    [SerializeField] int maxPresentInView = 20;

    public void GameUpdate(float gameTime)
    {
        if (foodEaterEmitter.emitStartTime <= gameTime)
        {
            foodEaterEmitter.isStartedEmit = true;
        }
    }

    public void OnTimerEnd()
    {

        foodEaterEmitter.isStartedEmit = false;
    }
    public void PresentEmitUpdate()
    {
        int point = Random.Range(0, presentEmitPoints.Count);

        beltTimer += Time.deltaTime;
        if (presentManager.NumberOfPresentInView >= maxPresentInView)
            {
                return;
            }



        if (beltTimer > beltInterval)
        {
            if (presentManager.NumberOfPresentInView >= maxPresentInView)
            {
                return;
            }



            beltTimer = 0;

            if (beltEmit)
            {
            GameObject newPresentObj = presentManager.EmitPresentRandom(presentDelivererPoint.position);
                GameObject newDeliverer = Instantiate(presentDelivererObj, presentDelivererPoint.position, Quaternion.identity);
                newPresentObj.GetComponent<Throwable>().OnHeld(newDeliverer);
                newDeliverer.GetComponent<PresentDeliverer>().moveDir = -1;
                 GameObject newDeliverer2 = Instantiate(presentDelivererObj, presentDelivererPoint2.position, Quaternion.identity);
            GameObject newPresentObj2 = presentManager.EmitPresentRandom(presentDelivererPoint2.position);
                newPresentObj2.GetComponent<Throwable>().OnHeld(newDeliverer2);

                newDeliverer2.GetComponent<PresentDeliverer>().moveDir = 1;
            }

        }
    }
}
