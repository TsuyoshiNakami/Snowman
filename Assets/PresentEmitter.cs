using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEmitter : MonoBehaviour {

    [SerializeField] List<GameObject> presents;
    float timer = 0;
    [SerializeField]float interval = 3;

    [SerializeField] Vector2 emitRangeMin;
    [SerializeField] Vector2 emitRangeMax;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer > interval)
        {
            int index = Random.Range(0, presents.Count);
            GameObject newPresentObj = Instantiate(presents[index], transform.position, transform.rotation);
            timer = 0; 

            newPresentObj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(emitRangeMin.x, emitRangeMax.x), Random.Range(emitRangeMin.y, emitRangeMax.y));
        }
	}
}
