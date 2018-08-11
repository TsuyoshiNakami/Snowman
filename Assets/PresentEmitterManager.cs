using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEmitterManager : MonoBehaviour {

    [SerializeField] List<GameObject> presents;
    [SerializeField] List<GameObject> presentEmitPoints;
    float timer = 0;
    [SerializeField]float interval = 3;

    [SerializeField] Vector2 emitRangeMin;
    [SerializeField] Vector2 emitRangeMax;
    PresentManager presentManager;

    
	// Use this for initialization
	void Start () {
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer > interval)
        {
            if(presentManager.NumberOfPresentInView >= 5)
            {
                return;
            }
            int index = Random.Range(0, presents.Count);
            int point = Random.Range(0, presentEmitPoints.Count);
            GameObject newPresentObj = presentManager.EmitPresentRandom(presentEmitPoints[point].transform.position);
            timer = 0; 

            newPresentObj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(emitRangeMin.x, emitRangeMax.x), Random.Range(emitRangeMin.y, emitRangeMax.y));
        }
	}
}
