using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSleigh : MonoBehaviour {
    [SerializeField] float arriveTime;
    [SerializeField] GameObject goalObj;

    float speed;
    float distance;
    Vector2 direction;

	// Use this for initialization
	void Start () {
        distance = Vector2.Distance(goalObj.transform.position, transform.position);
        speed = distance / arriveTime;
        direction = goalObj.transform.position - transform.position;
        direction.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        distance = Vector2.Distance(goalObj.transform.position, transform.position);
        if(distance <= speed * Time.deltaTime)
        {
            return;
        }
        transform.Translate(speed * direction * Time.deltaTime);
	}
}
