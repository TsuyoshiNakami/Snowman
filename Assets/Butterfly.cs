using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {

    [SerializeField] float interval = 0.3f;
    [SerializeField] float flyPower = 100;
    float timer = 0;
    Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

		if(interval <= timer)
        {
            timer -= interval;
            rigid.AddForce(new Vector2(0, flyPower + Random.Range(0, 30)));
        }
	}
}
