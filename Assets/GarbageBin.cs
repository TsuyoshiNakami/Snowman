using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBin : MonoBehaviour {
    PresentManager presentManager;

    private void Start()
    {
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.CompareTag("Throwable"))
        {
            presentManager.DeletePresent(c.gameObject);
        }
    }
}
