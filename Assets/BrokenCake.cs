using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCake : MonoBehaviour {

    [SerializeField] float disappearTime = 1;

	// Use this for initialization
	void Start () {
        Invoke("Disappear", disappearTime);
	}
	
    void Disappear()
    {
        Destroy(gameObject);
    }
}
