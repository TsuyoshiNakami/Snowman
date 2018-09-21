using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowPointCollider : MonoBehaviour {
    List<GameObject> interrupters = new List<GameObject>();
    public bool FoundInterrupter
    {
        get
        {
            return interrupters.Count > 0;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.CompareTag("InputInterrupter")) {
            interrupters.Add(c.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {

        interrupters.Remove(c.gameObject);
    }
    public GameObject GetFirstInterrputer()
    {
        return interrupters[0];
    }

}
