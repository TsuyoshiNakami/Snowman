using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMove : MonoBehaviour {
    public float speed = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

            transform.position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        transform.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * speed;
    }
}
