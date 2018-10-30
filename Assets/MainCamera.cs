using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    static MainCamera mainCamera;

	// Use this for initialization
	void Start () {
		if(mainCamera != null)
        {
            Destroy(gameObject);
        }
        mainCamera = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
