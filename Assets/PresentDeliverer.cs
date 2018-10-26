using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentDeliverer : MonoBehaviour {

    public float moveDir = 1;
    public float moveSpeed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * moveDir * moveSpeed * Time.deltaTime);	
	}
}
