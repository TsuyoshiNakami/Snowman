using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentDestroyCollision : MonoBehaviour {

    PresentManager presentManager;
	// Use this for initialization
	void Start () {
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Present>() != null)
        {
            presentManager.DeletePresent(collision.gameObject);
        }
    }
}
