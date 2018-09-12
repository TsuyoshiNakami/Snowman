using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketEmitter : MonoBehaviour {

    [SerializeField] GameObject basket;
    [SerializeField] GameObject[] checkPoints;
    [SerializeField] float interval = 1;
    float timer = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

		if(timer >= interval)
        {
            GameObject newObj = Instantiate(basket, checkPoints[0].transform.position, Quaternion.identity);
            BasketMover basketMover = newObj.GetComponent<BasketMover>();
            basketMover.speed = 4;
            basketMover.checkPoints = checkPoints;
            timer = 0;
        }
	}
}
