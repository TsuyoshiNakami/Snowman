using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEmitterManager : MonoBehaviour {
    [SerializeField] bool beltEmit = true;
    [SerializeField]float beltInterval = 3;
    [SerializeField] bool generatePresent = true;
    [SerializeField]float generateInterval = 3;
    [SerializeField] GameObject presentDelivererObj;
    [SerializeField] Transform presentDelivererPoint;
    [SerializeField] Transform presentDelivererPoint2;
    [SerializeField] List<GameObject> presents;
    [SerializeField] List<GameObject> presentEmitPoints;
    float generateTimer = 0;
    float beltTimer = 0;


    [SerializeField] Vector2 emitRangeMin;
    [SerializeField] Vector2 emitRangeMax;
    [SerializeField] int maxPresentInView = 20;
    PresentManager presentManager;
    PresentGameManager presentGameManager;
    
	// Use this for initialization
	void Start () {
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();

        presentGameManager = GameObject.Find("PresentGameManager").GetComponent<PresentGameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!presentGameManager.enablePresentEmit)
        {
            return;
        }
        beltTimer += Time.deltaTime;
        generateTimer += Time.deltaTime;

        if (beltTimer > beltInterval)
        {
            if (presentManager.NumberOfPresentInView >= maxPresentInView)
            {
                return;
            }
            int index = Random.Range(0, presents.Count);
            int point = Random.Range(0, presentEmitPoints.Count);


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

        if (generateTimer > generateInterval)
        {
            if (presentManager.NumberOfPresentInView >= maxPresentInView)
            {
                return;
            }
            int index = Random.Range(0, presents.Count);
            int point = Random.Range(0, presentEmitPoints.Count);


            generateTimer = 0;


            if(generatePresent)
            {
                GameObject newPresentObj = presentManager.EmitPresentRandom(presentEmitPoints[point].transform.position);
                //newPresentObj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(emitRangeMin.x, emitRangeMax.x), Random.Range(emitRangeMin.y, emitRangeMax.y));
            }
        }
	}
}
