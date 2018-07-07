using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEmitter : MonoBehaviour {

    [SerializeField] List<GameObject> presents;
    float timer = 0;
    float interval = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer > 3)
        {
            int index = Random.Range(0, presents.Count - 1);
            Instantiate(presents[index], transform.position, transform.rotation);
            timer = 0;
        }
	}
}
